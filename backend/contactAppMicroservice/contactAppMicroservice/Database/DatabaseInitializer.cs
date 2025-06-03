using contactAppMicroservice.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace contactAppMicroservice.Database
{
    public class DatabaseInitializer(ContactDbContext contactDbContext)
    {
        public async Task initializeDatabase()
        {
            contactDbContext.Database.EnsureCreated();

            if (!contactDbContext.Categories.Any())
            {
                await addCategories();
                await addSubcategories();
            }

            if (!contactDbContext.Contacts.Any())
            {
                await addContacts();
            }
        }

        private async Task addSubcategories()
        {
            var category = await contactDbContext.Categories.FirstOrDefaultAsync(c => c.Name == "służbowy");
            if (category != null)
            {
                await addSubcategory("szef", category);
                await addSubcategory("klient", category);
                await addSubcategory("współpracownik", category);
            }

            category = await contactDbContext.Categories.FirstOrDefaultAsync(c => c.Name == "prywatny");
            if (category != null)
            {
                await addSubcategory("żona", category);
                await addSubcategory("mąż", category);
                await addSubcategory("przyjaciel", category);
            }

            category = await contactDbContext.Categories.FirstOrDefaultAsync(c => c.Name == "inny");
            if (category != null)
            {
                await addSubcategory("znajomy z siłowni", category);
            }
        }

        private async Task addSubcategory(string name, Category category)
        {
            var subcategory = new Subcategory
            {
                SubcategoryId = Guid.NewGuid(),
                Name = name,
                Category = category,
                CategoryId = category.CategoryId
            };

            contactDbContext.Subcategories.Add(subcategory);
            await contactDbContext.SaveChangesAsync();
        }

        private async Task addCategory(string name)
        {
            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = name
            };

            contactDbContext.Categories.Add(category);
            await contactDbContext.SaveChangesAsync();
        }

        private async Task addCategories()
        {
            await addCategory("służbowy");
            await addCategory("prywatny");
            await addCategory("inny");
        }

        private async Task addContacts()
        {
            var category = await contactDbContext.Categories.FirstOrDefaultAsync(c => c.Name == "służbowy");
            var subcategory = await contactDbContext.Subcategories.FirstOrDefaultAsync(s => s.Name == "szef");

            if (category != null && subcategory != null)
            {
                await addContact("Marek", "Towarek", "M@rekTowarek", "marektowarek@wp.pl", "123456789",
                    new DateOnly(1990, 1, 1), category, subcategory);
                await addContact("Eliza", "Orzeszkowa", "Eliz@Orzeszkowa", "elizaorzeszkowa@gmail.com", "123123123",
                    new DateOnly(2003, 1, 1), category, subcategory);
            }

            category = await contactDbContext.Categories.FirstOrDefaultAsync(c => c.Name == "prywatny");
            subcategory = await contactDbContext.Subcategories.FirstOrDefaultAsync(s => s.Name == "mąż");

            if (category != null && subcategory != null)
            {
                await addContact("Robert", "Lewandowski", "R@bertLewandowski", "robertlewandowski@wp.pl", "123456789",
                    new DateOnly(1989, 12, 12), category, subcategory);
                await addContact("Maciej", "Maciak", "M@ciakM@ciak", "maciejmaciak@gmail.com", "987698765",
                    new DateOnly(2000, 10, 10), category, subcategory);
            }
        }

        private async Task addContact(string firstName, string lastName, string password, string email,
            string phoneNumber, DateOnly dateOfBirth, Category category, Subcategory subcategory)
        {
            var contact = new Contact
            {
                ContactId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                Category = category,
                CategoryId = category.CategoryId,
                Subcategory = subcategory,
                SubcategoryId = subcategory?.SubcategoryId
            };

            var passwordHash = new PasswordHasher<Contact>().HashPassword(contact, password);
            contact.PasswordHash = passwordHash;

            contactDbContext.Contacts.Add(contact);
            await contactDbContext.SaveChangesAsync();
        }
    }
}