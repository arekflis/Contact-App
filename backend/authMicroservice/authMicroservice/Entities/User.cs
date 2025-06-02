using System.ComponentModel.DataAnnotations;

namespace authMicroservice.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
