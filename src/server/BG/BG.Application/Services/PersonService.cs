using BG.Application.DTOs.Person;
using BG.Application.DTOs.User;
using BG.Infrastructure.Data;

namespace BG.Application.Services;

public class PersonService(IAdoRepository adoRepository, ApplicationDbContext context) : IPersonService
{
    private const string ModuleName = "Config.Person_";

    public async Task<ApiResponse> AddAsync(CreatePersonDto createPersonDto)
    {
        var parameters = new Dictionary<string, object>
        {
            { "person", CustomConverters.SerializeObjectCustom<CreatePersonDto>(createPersonDto) }
        };
        return await adoRepository.SpExecuteAsync<ApiResponse>($"{ModuleName}Insert", parameters);
    }

    public async Task<ApiResponse> DeleteAsync(int userId, int id)
    {
        var parameters = new Dictionary<string, object>
        {
            { "userId", userId },
            { "id", id }
        };
        return await adoRepository.SpExecuteAsync<ApiResponse>($"{ModuleName}Delete", parameters);
    }

    public async Task<ApiResponse> GetAsync(PersonaFilterDto personaFilterDto)
    {
        var parameters = new Dictionary<string, object>
        {
            { "person", CustomConverters.SerializeObjectCustom<PersonaFilterDto>(personaFilterDto) }
        };
        var dt = await adoRepository.GetDataTableAsync($"{ModuleName}", parameters);
        return CustomValidators.DataTableIsNull(dt)
            ? new ApiResponse { Code = "0", Message = "Data not found!" }
            : new ApiResponse { Code = "1", Payload = CustomConverters.DataTableToJson(dt) };
    }

    public async Task<ApiResponse> GetByIdAsync(int id)
    {
        var person = await context.Persons.FindAsync(id);
        return person is null
            ? new ApiResponse { Code = "0", Message = "Person not found!" }
            : new ApiResponse { Code = "1", Payload = CustomConverters.SerializeObjectCustom<PersonDto>(person) };
    }

    public async Task<ApiResponse> UpdateAsync(int id, UpdateUserDto updateUserDto)
    {
        var parameters = new Dictionary<string, object>
        {
            { "id", id },
            { "person", CustomConverters.SerializeObjectCustom<UpdateUserDto>(updateUserDto) }
        };
        return await adoRepository.SpExecuteAsync<ApiResponse>($"{ModuleName}Update", parameters);
    }
}