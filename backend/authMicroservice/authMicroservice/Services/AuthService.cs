using authMicroservice.Database;
using authMicroservice.DTO;
using authMicroservice.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace authMicroservice.Services
{
    public class AuthService(AuthDbContext authDbContext, IConfiguration configuration) : IAuthService
    {
        public async Task<LoginResponse?> loginAsync(LoginRequest loginRequest)
        {
            var user = await authDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user is null) return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }
            
            return await createLoginResponse(user);
        }

        public async Task<bool?> registerAsync(RegisterRequest registerRequest)
        {
            if (await authDbContext.Users.AnyAsync(u => u.Email == registerRequest.Email))
            {
                return false;
            }

            var newUser = new User();

            var passwordHash = new PasswordHasher<User>()
                .HashPassword(newUser, registerRequest.Password);

            newUser.UserId = Guid.NewGuid();
            newUser.Email = registerRequest.Email;
            newUser.PasswordHash = passwordHash;

            authDbContext.Users.Add(newUser);
            await authDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<LoginResponse?> refreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var user = await validateRefreshTokenAsync(refreshTokenRequest.RefreshToken);
            if (user == null) return null;

            return await createLoginResponse(user);
        }

        private async Task<LoginResponse> createLoginResponse(User user)
        {
            return new LoginResponse
            {
                Token = createToken(user),
                RefreshToken = await generateAndSaveRefreshTokenAsync(user)
            };

        }

        private async Task<User?> validateRefreshTokenAsync(string refreshToken)
        {
            var user = await authDbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            
            if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        private async Task<string> generateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = generateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1);
            
            await authDbContext.SaveChangesAsync();
            
            return refreshToken;
        }

        private string generateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string createToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtTokenSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("JwtTokenSettings:Issuer"),
                audience: configuration.GetValue<string>("JwtTokenSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
