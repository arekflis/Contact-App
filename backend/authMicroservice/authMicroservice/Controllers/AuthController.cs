using authMicroservice.Database;
using authMicroservice.DTO;
using authMicroservice.Entities;
using authMicroservice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<string>> register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await authService.registerAsync(registerRequest) is false)
                return BadRequest("User already exists.");

            return Ok("User created successfully.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authService.loginAsync(loginRequest);

            if (result is null) 
                return BadRequest("Invalid username or password.");

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponse>> refreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authService.refreshTokenAsync(refreshTokenRequest);

            if (result is null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }
    }
}
