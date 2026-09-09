using System;
using System.Collections.Generic;
using System.Text;
using Service.Interfaces;
using Service.Requests;
using DataAccess.Entities;
using DataAccess.Dbcontexts;
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
        public async Task<AppUser> GetUserAsync(string id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<List<AppUser>> GetUsersList()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<AppUser> CreateUserAsync(CreateUserServiceRequest request)
        {
            AppUser user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();
            return await GetUserAsync(user.Id);
        }

        public async Task<AppUser> GetUserById(string id)
        {
            try
            {
                return await context.Users.FindAsync(id);
            }
            catch (Exception)
            {
                throw new Exception("User not found");
            }
            
        }

        public async Task<AppUser> UpdateUserAsync(UpdateUserServiceRequest request)
        {
            
            if(request.Id != null)
            {
                AppUser user = await context.Users.FindAsync(request.Id);

                if (user != null)
                {
                    if (request.Email != null) user.Email = request.Email;
                    if (request.Username != null) user.Username = request.Username;
                    if (request.Password != null) user.Password = request.Password;
                    context.Users.Update(user);
                    await context.SaveChangesAsync();
                    return await context.Users.FindAsync(user.Id);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }

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
