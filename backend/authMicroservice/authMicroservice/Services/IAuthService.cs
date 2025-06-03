using authMicroservice.DTO;
using authMicroservice.Entities;

namespace authMicroservice.Services
{
    public interface IAuthService
    {
        Task<bool> registerAsync(RegisterRequest registerRequest);
        Task<LoginResponse?> loginAsync(LoginRequest loginRequest);
        Task<LoginResponse?> refreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
