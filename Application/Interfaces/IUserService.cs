using Application.DTOs.Account;
using Application.DTOs.User;
using Application.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<List<string>> GetAllUserNamesAsync();
        Task<List<UserSelectionDto>> GetAllUserIdAndNamesAsync();
        Task<List<UserDto>> GetUsersByIdsAsync(List<string> userIds);
        Task<PagedResponse<List<UserDto>>> GetAllUsersAsync(int pageNumber, int pageSize, string? search = null);
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<Response<string>> CreateUserAsync(RegisterRequest request);
        Task<Response<string>> UpdateUserAsync(string userId, UpdateUserRequest request);
        Task<Response<string>> DeleteUserAsync(string userId);
    }
}
