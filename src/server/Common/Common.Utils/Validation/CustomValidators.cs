namespace Common.Utils.Validation;

public class CustomValidators
{
    public static bool DataTableIsNull(DataTable dt)
    {
        var isNull = dt is not { Rows.Count: > 0 };
        return isNull;
    }

    public static bool DataSetIsNull(DataSet ds, bool removeEmptyDt = false)
    {
        switch (removeEmptyDt)
        {
            case true:
                {
                    var tablesToRemove = ds.Tables.Cast<DataTable>().Where(dt => dt.Rows.Count == 0).ToList();

                    foreach (var dt in tablesToRemove)
                    {
                        ds.Tables.Remove(dt);
                    }

                    break;
                }
        }

        return ds == null || ds.Tables.Count == 0;
    }

}