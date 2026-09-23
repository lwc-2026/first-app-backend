using System;
using System.Collections.Generic;
using System.Text;
using Service.Interfaces;
using Service.Requests;
using DataAccess.Entities;
using DataAccess.Dbcontexts;
using BusinessModel.DTOs;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext context;

        public UsersService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<UserDto>> GetUsersList()
        {
            return await context.Users.Select(x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
            }).ToListAsync();
        }

        public async Task<UserDto> CreateUserAsync(CreateUserServiceRequest request)
        {
            var hmac = new System.Security.Cryptography.HMACSHA512();
            AppUser user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key,
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var user = await context.Users.Where(x => x.Id == id).Select(x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
            }).FirstOrDefaultAsync() ?? throw new Exception("User not found");
            return user;
        }

        public async Task<UserDto> UpdateUserAsync(UpdateUserServiceRequest request)
        {
            
            AppUser? user = await context.Users.FindAsync(request.Id);

            if (user == null) throw new Exception("User not found");

            if (request.Email != null) user.Email = request.Email;
            if (request.Username != null) user.Username = request.Username;
            if (request.Password != null) 
            {
                var hmac = new System.Security.Cryptography.HMACSHA512();
                user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                user.PasswordSalt = hmac.Key;
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return await GetUserById(request.Id);
        }

        public async Task DeleteUserAsync(DeleteUserServiceRequest request)
        {
            var user = await context.Users.FindAsync(request.Id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
