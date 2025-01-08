namespace BG.Application.Interfaces.Services;

public interface IUserService
{
    Task<ApiResponse> AddAsync(User user);
    Task<ApiResponse> DeleteAsync(int userId, int id);
    Task<ApiResponse> GetAsync(Dictionary<string, object> parameters);
    Task<ApiResponse> UpdateAsync(int id, User user);
}