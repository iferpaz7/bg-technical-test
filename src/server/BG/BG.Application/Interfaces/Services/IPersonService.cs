using BG.Application.DTOs.Person;
using BG.Application.DTOs.User;

namespace BG.Application.Interfaces.Services;

public interface IPersonService
{
    Task<ApiResponse> AddAsync(CreatePersonDto createPersonDto);
    Task<ApiResponse> DeleteAsync(int userId, int id);
    Task<ApiResponse> GetAsync(PersonaFilterDto personaFilterDto);
    Task<ApiResponse> GetByIdAsync(int id);
    Task<ApiResponse> UpdateAsync(int id, UpdatePersonDto updatePersonDto);
}