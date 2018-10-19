using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicToolKit
{
    public static class SqliteManager
    {
        private static Dictionary<string, SQLiteConnection> SqliteDic { get; set; }
        public static SQLiteConnection GetSQLite(string _dbPath, string _dbName)
        {
            string connStr = "Data Source=" + _dbPath + _dbName + ".db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10";
            SQLiteConnection Sqlite = new SQLiteConnection(_dbPath);
            if (SqliteDic == null)
            {
                SqliteDic = new Dictionary<string, SQLiteConnection>();
            }
            if (SqliteDic.ContainsKey(_dbName) == false)
            {
                SqliteDic.Add(_dbName, Sqlite);
            }
            return Sqlite;
        }
    }
}
