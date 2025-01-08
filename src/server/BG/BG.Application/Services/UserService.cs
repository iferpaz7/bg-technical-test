using BG.Infrastructure.Data;

namespace BG.Application.Services;

public class UserService(ApplicationDbContext applicationDbContext) : IUserService
{
    public async Task<ApiResponse> AddAsync(CreateUserDto createUserDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse> DeleteAsync(int userId, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse> GetAsync(Dictionary<string, object> parameters)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse> UpdateAsync(int id, UpdateUserDto updateUserDto)
    {
        throw new NotImplementedException();
    }
}