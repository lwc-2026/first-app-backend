using System.Threading.Tasks;
using System.Collections.Generic;
using BusinessModel.DTOs;
using Service.Requests;

namespace Service.Interfaces
{
    public interface IUsersService
    {
        Task<List<UserDto>> GetUsersList();
        Task<UserDto> GetUserById(string id);
        Task<UserDto> CreateUserAsync(CreateUserServiceRequest request);
        Task<UserDto> UpdateUserAsync(UpdateUserServiceRequest request);
        Task DeleteUserAsync(DeleteUserServiceRequest request);
    }
}