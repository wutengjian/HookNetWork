using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBRepertory
{
    public class SqlServerBulkCopy
    {
        private static Dictionary<string, string> mapping;
        public static string ConnStr;
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                tb.Rows.Add(values);
            }
            return tb;
        }
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                return t.IsValueType ? Nullable.GetUnderlyingType(t) : t;
            }
            return t;
        }
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        public static void SqlBulkCopyToServer(DataTable sourceDataTable, string targetTableName)
        {
            SqlConnection conn = new SqlConnection(ConnStr);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);  //用其它源的数据有效批量加载sql server表中
            bulkCopy.BulkCopyTimeout = 120;
            bulkCopy.DestinationTableName = targetTableName;  //服务器上目标表的名称
            bulkCopy.BatchSize = sourceDataTable.Rows.Count;  //每一批次中的行数
            try
            {
                conn.Open();
                if (sourceDataTable != null && sourceDataTable.Rows.Count != 0)
                {
                    foreach (string key in mapping.Keys)
                    {
                        bulkCopy.ColumnMappings.Add(key, mapping[key]);
                    }
                    bulkCopy.BatchSize = 10000;
                    //将提供的数据源中的所有行复制到目标表中
                    bulkCopy.WriteToServer(sourceDataTable);
                }
            }
            catch (Exception ex)
            {
                if (bulkCopy != null)
                    bulkCopy.Close();
            }
            finally
            {
                conn.Close();
                if (bulkCopy != null)
                    bulkCopy.Close();
            }
        }
        public static void SqlBulkMapping(Dictionary<string, string> dic)
        {
            mapping = dic;
        }
    }
}