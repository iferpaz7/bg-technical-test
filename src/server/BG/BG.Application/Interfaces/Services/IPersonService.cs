
namespace BG.Application.Interfaces.Services;

public interface IPersonService
{
    Task<ApiResponse> AddAsync(Person person);
    Task<ApiResponse> DeleteAsync(int userId, int id);
    Task<ApiResponse> GetAsync(Dictionary<string, object> parameters);
    Task<ApiResponse> UpdateAsync(int id, Person person);
}