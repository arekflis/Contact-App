using System.ComponentModel.DataAnnotations;

namespace authMicroservice.DTO
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "RefreshToken is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
