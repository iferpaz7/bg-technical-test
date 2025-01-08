using BG.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BG.Infrastructure.Repositories.Implementations;

public class AdoRepository(IConfiguration configuration) : IAdoRepository
{
    public async Task<DataSet> GetDataSetAsync(string query, Dictionary<string, object> parameters,
        bool withTableNames,
        bool timeout)
    {
        var ds = new DataSet();
        await using var conn =
            new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        await using var cmd = new SqlCommand(query, conn);
        var wasOpen = cmd.Connection.State == ConnectionState.Open;
        try
        {
            parameters ??= new Dictionary<string, object>();

            if (parameters.Count > 0)
                foreach (var parameter in parameters.Where(p => !string.IsNullOrEmpty(p.Key)))
                    ParametersUtils.AddSqlParameter(cmd, parameter.Key, parameter.Value ?? DBNull.Value);

            const string outParam = "@tableNames";
            if (withTableNames) ParametersUtils.AddSqlParameterOut(cmd, outParam, SqlDbType.VarChar, 500);

            if (!timeout) cmd.CommandTimeout = 0;

            cmd.CommandType = CommandType.StoredProcedure;
            if (!wasOpen) await conn.OpenAsync();

            using (var adapter = new SqlDataAdapter())
            {
                adapter.SelectCommand = cmd;
                ds = new DataSet();
                await Task.Run(() => adapter.Fill(ds));
            }

            if (withTableNames)
            {
                var tableNames = ParametersUtils.GetParameter(cmd, outParam).ToString()?.Split(',');
                await Task.Run(() =>
                {
                    if (tableNames != null)
                        Parallel.ForEach(tableNames, (tableName, state, index) =>
                        {
                            if (!string.IsNullOrEmpty(tableName)) ds.Tables[(int)index].TableName = tableName;
                        });
                });
            }
        }
        finally
        {
            if (!wasOpen) await cmd.Connection.CloseAsync();
        }

        return ds;
    }

    public async Task<DataTable> GetDataTableAsync(string query, Dictionary<string, object> parameters)
    {
        var dt = new DataTable();
        await using var conn =
            new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        await using var cmd = new SqlCommand(query, conn);
        var wasOpen = cmd.Connection.State == ConnectionState.Open;
        try
        {
            parameters ??= [];

            if (parameters.Count > 0)
                foreach (var parameter in parameters.Where(p => !string.IsNullOrEmpty(p.Key)))
                    ParametersUtils.AddSqlParameter(cmd, parameter.Key, parameter.Value ?? DBNull.Value);

            cmd.CommandType = CommandType.StoredProcedure;
            if (!wasOpen) await conn.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
        }
        finally
        {
            if (!wasOpen) await cmd.Connection.CloseAsync();
        }

        return dt;
    }

    public async Task<int> OnlyExecuteAsync(string query, Dictionary<string, object> parameters,
        bool useStoredProcedure, bool timeout)
    {
        await using var conn =
            new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        await using var cmd = new SqlCommand(query, conn);
        var wasOpen = cmd.Connection.State == ConnectionState.Open;
        try
        {
            parameters ??= new Dictionary<string, object>();

            if (parameters.Count > 0)
                foreach (var parameter in parameters.Where(p => !string.IsNullOrEmpty(p.Key)))
                    ParametersUtils.AddSqlParameter(cmd, parameter.Key, parameter.Value ?? DBNull.Value);

            if (!timeout) cmd.CommandTimeout = 0;

            cmd.CommandType = useStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            if (!wasOpen) await conn.OpenAsync();

            return await cmd.ExecuteNonQueryAsync();
        }
        finally
        {
            if (!wasOpen) await cmd.Connection.CloseAsync();
        }
    }

    public async Task<T> SpExecuteAsync<T>(string name, Dictionary<string, object> parameters, bool timeout)
        where T : class, new()
    {
        var response = new T();
        await using var conn =
            new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        await using var cmd = new SqlCommand(name, conn);
        var wasOpen = cmd.Connection.State == ConnectionState.Open;
        try
        {
            parameters ??= new Dictionary<string, object>();

            if (parameters.Count > 0)
                foreach (var parameter in parameters.Where(p => !string.IsNullOrEmpty(p.Key)))
                    ParametersUtils.AddSqlParameter(cmd, parameter.Key, parameter.Value ?? DBNull.Value);

            if (!timeout) cmd.CommandTimeout = 0;

            cmd.CommandType = CommandType.StoredProcedure;
            if (!wasOpen) await conn.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // Reflection to access properties dynamically
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    // Check if property name matches a column name (case-insensitive)
                    var columnName = reader.GetSchemaTable()
                        ?.Rows.Cast<DataRow>()
                        .Where(row =>
                            row["ColumnName"].ToString().Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                        .Select(row => row["ColumnName"].ToString())
                        .FirstOrDefault();

                    if (columnName == null) continue;

                    var index = reader.GetOrdinal(columnName);
                    if (index != -1 && !reader.IsDBNull(index))
                        // Set property value based on data type
                        property.SetValue(response, reader.GetValue(index));
                }
            }
        }
        finally
        {
            if (!wasOpen) await cmd.Connection.CloseAsync();
        }

        return response;
    }
}