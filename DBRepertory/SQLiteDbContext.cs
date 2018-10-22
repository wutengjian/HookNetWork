using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicToolKit;
using DBModels;
using BasicExpansion;

namespace DBRepertory
{
    public class SQLiteDbContext : DbContext
    {
        public SQLiteDbContext() : base("HookNetWorkSQLite")
        {
        }
        public void CreateTable(string TableName, Dictionary<string, string> ColDic, bool IsCover = false)
        {
            string connStr = ConfigurationManager.ConnectionStrings["HookNetWorkSQLite"].ConnectionString;
            SQLiteConnection Sqlite = SqliteManager.GetSQLite(connStr);
            Sqlite.CreateTable(TableName, ColDic, IsCover);
        }
        public DbSet<ArticleFileInfo> ArticleFiles { set; get; }
    }
}
