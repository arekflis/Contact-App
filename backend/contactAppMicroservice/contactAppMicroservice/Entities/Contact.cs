using System.ComponentModel.DataAnnotations;

namespace contactAppMicroservice.Entities
{
    public class Contact
    {
        [Key]
        public Guid ContactId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash {  get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid? SubcategoryId { get; set; }
        public Subcategory? Subcategory { get; set; }
        

    }
}
