using System.Threading.Tasks;
using DataAccess.Entities;
using Service.Requests;

namespace Service.Interfaces
{
    public interface IUsersService
    {
        Task<List<AppUser>> GetUsersList();
        Task<AppUser?> GetUserById(string id);
        Task<AppUser?> GetUserAsync(string id);
        Task CreateUserAsync(CreateUserServiceRequest request);
    }
}