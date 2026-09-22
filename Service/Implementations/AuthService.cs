using System;
using System.Threading.Tasks;
using Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using DataAccess.Dbcontexts;
using DataAccess.Entities;

namespace Service.Implementations
{
    public class AuthService(IConfiguration configuration, AppDbContext appDbContext) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return null;
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            if (!CryptographicOperations.FixedTimeEquals(computedPasswordHash, user.PasswordHash))
            {
                Console.WriteLine("Password verification failed.");
                return null;
            }

            Console.WriteLine("Password verification succeeded.");
            // Password is correct, proceed with generating JWT token
            var jwtSecret = _configuration["JwtSettings:Secret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT Secret is not configured.");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task LogoutAsync()
        {
            // implement logout logic here


        }
    }
}
