namespace BG.Application.Services;

public class PersonService(IAdoRepository adoRepository) : IPersonService
{
    private const string ModuleName = "Config.Person_";

    public async Task<ApiResponse> AddAsync(Person person)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse> DeleteAsync(int userId, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse> GetAsync(Dictionary<string, object> parameters)
    {
        var dt = await adoRepository.GetDataTableAsync($"{ModuleName}", parameters);

        return new ApiResponse { };
    }

    public async Task<ApiResponse> UpdateAsync(int id, Person person)
    {
        throw new NotImplementedException();
    }
}