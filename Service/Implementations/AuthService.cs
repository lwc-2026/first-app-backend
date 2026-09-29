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
using BusinessModel.DTOs;

namespace Service.Implementations
{
    public class AuthService(IConfiguration configuration, AppDbContext appDbContext, TimeProvider timeProvider) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly TimeProvider _timeProvider = timeProvider;

        public async Task<AuthDto?> AuthenticateAsync(string username, string password)
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
                return null;
            }

            // Password is correct, proceed with generating JWT token
            var token = this.GenerateJwtToken(user);
            var refreshToken = this.GenerateRefreshToken(user, out var rawRefreshToken); 

            // Save Refresh Token to the database
            _appDbContext.RefreshTokens.Add(refreshToken);
            await _appDbContext.SaveChangesAsync();

            return new AuthDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = rawRefreshToken,
                ExpiresAt = token.ValidTo,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt
            };
        }

        public async Task<AuthDto?> RefreshTokenAsync(string refreshToken)
        {
            var hash = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(refreshToken));

            var oldRefreshToken = await _appDbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == Convert.ToBase64String(hash)
                    && rt.ExpiresAt > _timeProvider.GetUtcNow().DateTime
                    && rt.RevokedAt == null
                    && rt.ReplacedByToken == null);

            if (oldRefreshToken == null)
            {
                return null;
            }

            var newRefreshToken = this.GenerateRefreshToken(oldRefreshToken.User, out var rawNewRefreshToken);

            oldRefreshToken.RevokedAt = _timeProvider.GetUtcNow().DateTime;
            oldRefreshToken.ReplacedByToken = newRefreshToken.Token;

            // Save the new refresh token and revoke the refresh token to the database
            _appDbContext.RefreshTokens.Add(newRefreshToken);

            await _appDbContext.SaveChangesAsync();

            var token = this.GenerateJwtToken(oldRefreshToken.User);
            return new AuthDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = rawNewRefreshToken,
                ExpiresAt = token.ValidTo,
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
            };
        }

        public async Task LogoutAsync()
        {
            // implement logout logic here


        }

        private JwtSecurityToken GenerateJwtToken(AppUser user)
        {
            var jwtSecret = _configuration["JwtSettings:Secret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT Secret is not configured.");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: _timeProvider.GetUtcNow().AddMinutes(int.TryParse(_configuration["JwtSettings:AccessTokenExpirationMinutes"], out var minutes) ? minutes : 60).DateTime,
                signingCredentials: credentials
            );
            return token;
        }

        private RefreshToken GenerateRefreshToken(AppUser user, out string rawRefreshToken)
        {           
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var randomBytes = Convert.ToBase64String(randomNumber);
            rawRefreshToken = randomBytes;
            // adding hash to refresh token
            var hash = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(randomBytes));

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(hash),
                ExpiresAt = _timeProvider.GetUtcNow().AddDays(int.TryParse(_configuration["JwtSettings:RefreshTokenExpirationDays"], out var days) ? days : 7).DateTime,
                User = user,
                UserId = user.Id
            };
            return refreshToken;
        }
    }
}
