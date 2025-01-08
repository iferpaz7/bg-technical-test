using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;

namespace Common.Utils.Data;

public class CustomConverters
{
    public static string DataTableToJson(DataTable table)
    {
        var contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
        var options = new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        };
        return JsonConvert.SerializeObject(table, options);
    }
    public static object DataSetToJson(DataSet ds, bool oldConverter = false)
    {
        if (oldConverter)
        {
            var root = new ArrayList();

            foreach (DataTable dt in ds.Tables)
            {
                var table = (from DataRow dr in dt.Rows
                             select dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col])).ToList();

                root.Add(table);
            }

            return JsonConvert.SerializeObject(root);
        }

        var contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
        var options = new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        };
        return JsonConvert.SerializeObject(ds, options);
    }
    public static string SerializeObjectCustom<T>(object data)
    {
        var contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
        var options = new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        };
        return JsonConvert.SerializeObject(data, options);
    }
}