using contactAppMicroservice.Validation;
using System.ComponentModel.DataAnnotations;

namespace contactAppMicroservice.DTO.Request
{
    public class ContactRequest
    {
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(20, ErrorMessage = "First Name cannot be longer than 20 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(20, ErrorMessage = "Last Name cannot be longer than 20 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W)[\S]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DateInThePast(ErrorMessage = "Date of Birth must be in the past")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Category Id is required")]
        public Guid CategoryId { get; set; }

        public Guid? SubcategoryId { get; set; }

        [MaxLength(30, ErrorMessage = "Subcategory Name cannot be longer than 30 characters")]
        public string? SubcategoryName { get; set; }
    }
}
