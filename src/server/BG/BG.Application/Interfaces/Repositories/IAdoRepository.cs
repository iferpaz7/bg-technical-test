using System.Data;

namespace BG.Application.Interfaces.Repositories;

public interface IAdoRepository
{
    public Task<DataSet> GetDataSetAsync(string query, Dictionary<string, object>? parameters = null, bool withTableNames = true,
        bool timeout = true);

    public Task<DataTable> GetDataTableAsync(string query, Dictionary<string, object>? parameters = null);

    public Task<int> OnlyExecuteAsync(string query, Dictionary<string, object>? parameters = null,
        bool useStoredProcedure = true, bool timeout = true);
    
    Task<T> SpExecuteAsync<T>(string name, Dictionary<string, object>? parameters = null, bool timeout = true)
        where T : class, new();
}