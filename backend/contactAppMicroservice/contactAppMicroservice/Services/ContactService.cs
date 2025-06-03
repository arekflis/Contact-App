using AutoMapper;
using contactAppMicroservice.Database;
using contactAppMicroservice.DTO.Response;
using contactAppMicroservice.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace contactAppMicroservice.Services
{
    public class ContactService(ContactDbContext contactDbContext, IMapper mapper): IContactService
    {
       
        public async Task<List<ContactResponse>?> getAllContacts()
        {
            var contacts = await contactDbContext.Contacts.ToListAsync();

            return mapper.Map<List<ContactResponse>>(contacts);
        }

        public async Task<ContactDetailsResponse?> getContactById(Guid contactId)
        {
            var contact = await contactDbContext.Contacts
                .Include(c => c.Category)
                .Include(c => c.Subcategory)
                .FirstOrDefaultAsync(c => contactId == c.ContactId);

            if (contact is null)
                return null;

            return mapper.Map<ContactDetailsResponse>(contact);
        }

        public async Task<bool> deleteContact(Guid contactId, string password)
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
    }
}
