namespace BG.Application.Interfaces.Services;

public interface IIdentificationTypeService
{
    Task<ApiResponse> GetAllAsync();
}