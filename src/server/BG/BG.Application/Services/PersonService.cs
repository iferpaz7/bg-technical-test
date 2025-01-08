namespace BG.Application.Services;

public class PersonService(IAdoRepository adoRepository) : IPersonService
{
    private const string ModuleName = "Config.Person_";

    public async Task<ApiResponse> AddAsync(Person person)
    {
        var parameters = new Dictionary<string, object>
        {
            { "person", CustomConverters.SerializeObjectCustom<Person>(person) }
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

    public async Task<ApiResponse> GetAsync(Dictionary<string, object> parameters)
    {
        var dt = await adoRepository.GetDataTableAsync($"{ModuleName}", parameters);
        return CustomValidators.DataTableIsNull(dt)
            ? new ApiResponse { code = "0", message = "Data not found!" }
            : new ApiResponse { code = "1", payload = CustomConverters.DataTableToJson(dt) };
    }

    public async Task<ApiResponse> UpdateAsync(int id, Person person)
    {
        var parameters = new Dictionary<string, object>
        {
            { "id", id },
            { "person", CustomConverters.SerializeObjectCustom<Person>(person) }
        };
        return await adoRepository.SpExecuteAsync<ApiResponse>($"{ModuleName}Update", parameters);
    }
}