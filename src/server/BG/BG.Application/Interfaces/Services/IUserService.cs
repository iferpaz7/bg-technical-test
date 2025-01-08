namespace BG.Application.Interfaces.Services;

public interface IUserService
{
    Task<ApiResponse> AddAsync(CreateUserDto createUserDto);
    Task<ApiResponse> DeleteAsync(int userId, int id);
    Task<ApiResponse> GetAsync(Dictionary<string, object> parameters);
    Task<ApiResponse> UpdateAsync(int id, UpdateUserDto updateUserDto);
}