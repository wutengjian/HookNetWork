using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BasicToolKit
{
       public static class ObjectExtension
    {
        public static string RegexMatch(this string input, string pattern, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Regex.Match(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
            }
            else
            {
                return Regex.Match(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[key].Value;
            }
        }
        public static bool RegexIsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        public static string HtmlReplace(this string info)
        {
            info = Regex.Replace(info, "(<[^<>]*>|&nbsp;)", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            info = Regex.Replace(info, "\\r", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            info = Regex.Replace(info, "\\s{2,}", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return info.Replace('\'', '’');
        }
        /// <summary>
        /// 含中文字符串转ASCII
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToAscii(this String str)
        {
            int code;
            char[] chars = str.ToCharArray();
            StringBuilder sb = new StringBuilder(255);
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if (c > 255)
                {
                    sb.Append("\\u");
                    code = (c >> 8);
                    string tmp = code.ToString("X");
                    if (tmp.Length == 1) sb.Append("0");
                    sb.Append(tmp);
                    code = (c & 0xFF);
                    tmp = code.ToString("X");
                    if (tmp.Length == 1) sb.Append("0");
                    sb.Append(tmp);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return (sb.ToString());
        }
        public static String StringToAsciiTen(this String str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    //将中文字符转为10进制整数，然后转为16进制unicode字符
                    outStr += "\\u" + ((int)str[i]).ToString("x");
                }
            }
            return outStr;
        }
        /// <summary>
        /// ASCII转含中文字符串
        /// </summary>
        /// <param name="textAscii">ASCII字符串</param>
        /// <returns></returns>
        public static string AsciiToString(this string textAscii)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(textAscii))
            {
                string[] strlist = textAscii.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }

            }
            return outStr;
        }
    }
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
            StringBuilder str = new StringBuilder("create table if not exists " + TableName + " (");
            string sqlStr = string.Empty;
            foreach (string key in ColDic.Keys)
            {
                str.Append(key + " " + ColDic[key] + ",");
            }
            sqlStr = str.ToString().TrimEnd(',') + ");";
            if (IsCover)
            {
                SqliteConn.DropTable(TableName);
            }
            SqliteConn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sqlStr, SqliteConn);
            cmd.ExecuteNonQuery();
            SqliteConn.Close();
        }
        public static void DropTable(this SQLiteConnection SqliteConn, string TableName)
        {
            StringBuilder str = new StringBuilder();
            string sqlStr = "drop table if exists " + TableName + " ;";
            SqliteConn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sqlStr, SqliteConn);
            cmd.ExecuteNonQuery();
            SqliteConn.Close();
        }
        public static void TruncateTable(this SQLiteConnection SqliteConn, string TableName)
        {
            StringBuilder str = new StringBuilder();
            string sqlStr = "DELETE FROM sqlite_sequence WHERE name = '" + TableName + "'; ";
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
