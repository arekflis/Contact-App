using AutoMapper;
using contactAppMicroservice.Database;
using contactAppMicroservice.DTO.Request;
using contactAppMicroservice.DTO.Response;
using contactAppMicroservice.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace contactAppMicroservice.Services.ContactServices
{
    public class ContactService(ContactDbContext contactDbContext, IMapper mapper): IContactService
    {
        private const string otherCategoryName = "inny";       
        public async Task<List<ContactResponse>?> getAllContactsAsync()
        {
            var contacts = await contactDbContext.Contacts.ToListAsync();

            return mapper.Map<List<ContactResponse>>(contacts);
        }

        public async Task<ContactDetailsResponse?> getContactByIdAsync(Guid contactId)
        {
            var contact = await contactDbContext.Contacts
                .Include(c => c.Category)
                .Include(c => c.Subcategory)
                .FirstOrDefaultAsync(c => contactId == c.ContactId);

            if (contact is null)
                return null;

            return mapper.Map<ContactDetailsResponse>(contact);
        }

        public async Task<bool> deleteContactAsync(Guid contactId, string password)
        {
            var contact = await contactDbContext.Contacts.FindAsync(contactId);

            if (contact == null)
                return false;

            if (new PasswordHasher<Contact>().VerifyHashedPassword(contact, contact.PasswordHash, password) 
                == PasswordVerificationResult.Failed)
                    return false;

            contactDbContext.Contacts.Remove(contact);
            await contactDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ContactDetailsResponse> addNewContactAsync(ContactRequest contactRequest)
        {
            var newContact = await validateContactRequestAsync(contactRequest, false);
            
            contactDbContext.Contacts.Add(newContact);
            await contactDbContext.SaveChangesAsync();

            return mapper.Map<ContactDetailsResponse>(newContact);
        }

        public async Task updateContactAsync(Guid contactId, ContactRequest contactRequest)
        {
            var contactToUpdate = await contactDbContext.Contacts.FindAsync(contactId);
            if (contactToUpdate is null)
                throw new ArgumentException("Contact does not exist");

            if (new PasswordHasher<Contact>().VerifyHashedPassword(contactToUpdate, contactToUpdate.PasswordHash,
                contactRequest.Password) == PasswordVerificationResult.Failed)
                throw new ArgumentException("Invalid contact id or password");

            bool skipMailCheck = contactToUpdate.Email == contactRequest.Email;
            var updatedContact = await validateContactRequestAsync(contactRequest, skipMailCheck);
            updatedContact.ContactId = contactToUpdate.ContactId;
            
            contactDbContext.Entry(contactToUpdate).CurrentValues.SetValues(updatedContact);
            await contactDbContext.SaveChangesAsync();
        }

        private async Task<Contact> validateContactRequestAsync(ContactRequest contactRequest, bool skipEmailCheck)
        {
            if (contactRequest.SubcategoryId != null && contactRequest.SubcategoryName != null)
                throw new ArgumentException("Invalid subcategories parameters");

            if (!skipEmailCheck)
            {
                var contact = await contactDbContext.Contacts.FirstOrDefaultAsync(c => c.Email == contactRequest.Email);
                if (contact != null)
                    throw new ArgumentException("Email already exists");
            }
            
            var category = await contactDbContext.Categories.FindAsync(contactRequest.CategoryId);
            if (category == null)
                throw new ArgumentException("Invalid category id");

            Subcategory? subcategory = await validateSubcategoryAsync(category, contactRequest.SubcategoryName,
                    contactRequest.SubcategoryId);

            return createContactFromRequest(category, subcategory, contactRequest);
        }

        private Contact createContactFromRequest(Category category, Subcategory? subcategory, ContactRequest contactRequest)
        {
            var contact = mapper.Map<Contact>(contactRequest);

            contact.ContactId = Guid.NewGuid();
            contact.Category = category;
            contact.Subcategory = subcategory;
            contact.SubcategoryId = subcategory?.SubcategoryId;

            var passwordHash = new PasswordHasher<Contact>().HashPassword(contact, contactRequest.Password);
            contact.PasswordHash = passwordHash;

            return contact;
        }

        private async Task<Subcategory?> validateSubcategoryAsync(Category category, string? subcategoryName, Guid? subcategoryId)
        {
            if (subcategoryName == null && subcategoryId == null)
                return null;

            if (subcategoryId != null)
                return await getSubcategoryByIdAsync(category, subcategoryId);

            if (category.Name == otherCategoryName && subcategoryName != null)
                return await getOrCreateOtherSubcategoryAsync(subcategoryName, category);
            
            throw new ArgumentException("Invalid subcategories parameters");
        }

        private async Task<Subcategory> getSubcategoryByIdAsync(Category category, Guid? subcategoryId)
        {
            var subcategory = await contactDbContext.Subcategories
                   .Include(s => s.Category)
                   .FirstOrDefaultAsync(s => s.SubcategoryId == subcategoryId);

            if (subcategory == null || subcategory.Category.CategoryId != category.CategoryId)
                throw new ArgumentException("Invalid subcategories parameters");
            return subcategory;
        }

        private async Task<Subcategory> getOrCreateOtherSubcategoryAsync(string subcategoryName, Category category)
        {
            var subcategory = await contactDbContext.Subcategories
                    .Include(s => s.Category)
                    .FirstOrDefaultAsync(s => s.Name == subcategoryName);

            if (subcategory is null)
            {
                subcategory = new Subcategory
                {
                    SubcategoryId = Guid.NewGuid(),
                    Name = subcategoryName,
                    CategoryId = category.CategoryId,
                    Category = category
                };
                contactDbContext.Subcategories.Add(subcategory);
                await contactDbContext.SaveChangesAsync();
            }

            return subcategory;
        }
    }
}
