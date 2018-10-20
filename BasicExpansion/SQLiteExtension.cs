using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicExpansion
{
    public static class SQLiteExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SqliteConn"></param>
        /// <param name="TableName"></param>
        /// <param name="ColDic"></param>
        /// <param name="IsCover">是否覆盖:默认不覆盖</param>
        public static void CreateTable(this SQLiteConnection SqliteConn, string TableName, Dictionary<string, string> ColDic, bool IsCover = false)
        {
            string sqlStr = string.Format("");
            SqliteConn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sqlStr, SqliteConn);
            cmd.ExecuteNonQuery();
            SqliteConn.Close();
        }
        public static void InsertData(this SQLiteConnection SqliteConn)
        {
            SqliteConn.Open();
            SQLiteCommand cmd = new SQLiteCommand(SqliteConn);
            cmd.CommandText = "INSERT INTO student VALUES(1, '小红', '男')";//插入几条数据  
            cmd.ExecuteNonQuery();
            SqliteConn.Close();
        }
        public static void GetData(this SQLiteConnection SqliteConn)
        {
            string sqlStr = string.Format("");
            SqliteConn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sqlStr, SqliteConn);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (SqliteConn.State == ConnectionState.Open)
                {
                    //连接成功
                }
                //reader.FieldCount;   //2  获取列数
                //reader.Depth;        //0  嵌套深度
                //reader.HasRows;      //true  是否包含行
                //reader.IsClosed;     //false SqlDataReader是否关闭 
                //reader.RecordsAffected;      //-1 执行T-SQL语句所插入、修改、删除的行数
                //reader.VisibleFieldCount;    //2  未隐藏的字段数目(一共就两列)

                while (reader.Read())
                {
                    //reader["PersonName"];
                    //reader[1]; 通过数字索引或字符串索引访问
                    //reader.IsDBNull(1);      //是否是null值
                    //reader.GetString(1);     //Get什么类型就返回什么类型，这没啥好说的。
                }
                //reader.GetName(1);               //PersonName 由数字获得列名
                //reader.GetOrdinal("PersonName"); //1 由列名获取其在reader中的数字索引
                if (reader.NextResult())
                {
                    //reader.GetString(1);
                }
            }
            SqliteConn.Close();
        }
    }
}
