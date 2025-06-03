using System.ComponentModel.DataAnnotations;

namespace contactAppMicroservice.DTO.Request
{
    public class DeleteContactRequest
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W)[\S]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character")]
        public string Password { get; set; } = string.Empty;
    }
}
