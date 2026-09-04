using System.Threading.Tasks;
using DataAccess.Entities;
using Service.Requests;

namespace Service.Interfaces
{
    public interface IUsersService
    {
        Task<AppUser?> GetUserAsync(string id);
        Task CreateUserAsync(CreateUserRequest request);
    }
}