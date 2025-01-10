namespace BG.Application.Interfaces.Services;

public interface IAccountService
{
    Task<ApiResponse> LoginAsync(LoginDto loginDto);
}