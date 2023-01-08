using System.ComponentModel;
using System.Data;

namespace CdPo.PrintForm.Handler.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Преобразовать в DataTable
    /// </summary>
    /// <param name="collection"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string tableName)
    {
        // ReSharper disable once InconsistentNaming
        var TType = typeof(T);
        var props = TypeDescriptor.GetProperties(TType);
        var dt = new DataTable(tableName);
        foreach (PropertyDescriptor p in props)
        {
            dt.Columns.Add(p.Name, p.PropertyType);
        }


        foreach (var item in collection)
        {
            var row = dt.NewRow();
            foreach (PropertyDescriptor column in props)
            {
                row[column.Name] = TType.GetProperty(column.Name)?.GetValue(item, null);
            }

            dt.Rows.Add(row);
        }

        return dt;
    }
}