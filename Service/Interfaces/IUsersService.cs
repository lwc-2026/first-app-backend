using System.Threading.Tasks;
using DataAccess.Entities;
using Service.Requests;

namespace Service.Interfaces
{
    public interface IUsersService
    {
        Task<List<AppUser>> GetUsersList();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> CreateUserAsync(CreateUserServiceRequest request);
        Task<AppUser> UpdateUserAsync(UpdateUserServiceRequest request);
        Task DeleteUserAsync(DeleteUserServiceRequest request);
    }
}