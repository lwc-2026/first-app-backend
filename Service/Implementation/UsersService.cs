using System;
using System.Collections.Generic;
using System.Text;
using Service.Interfaces;
using Service.Requests;
using DataAccess.Entities;
using DataAccess.Dbcontexts;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _context;

        public UsersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetUserAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task CreateUserAsync(CreateUserRequest request)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                Email = request.Email,
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }
    }
}
