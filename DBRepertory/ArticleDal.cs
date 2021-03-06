﻿using Dapper;
using DBModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepertory
{
    public class ArticleDal
    {
        string ConnStr = "Data Source=JiannyWu;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";
        public void SaveList(List<ArticleInfo> ArticleList)
        {
            List<string> HashList = GetHashlist();
            ArticleList = ArticleList.Where(x => HashList.Contains(x.HashCode) == false).ToList();
            var data = SqlServerBulkCopy.ToDataTable<ArticleInfo>(ArticleList);
            Dictionary<string, string> SqlMapping = new Dictionary<string, string>();
            SqlMapping.Add("HashCode", "HashCode");
            SqlMapping.Add("CreateTime", "CreateTime");
            SqlMapping.Add("DataTitle", "DataTitle");
            SqlMapping.Add("DataContent", "DataContent");
            SqlMapping.Add("DataType", "DataType");
            SqlMapping.Add("KeyWordSort", "KeyWordSort");
            SqlMapping.Add("DataSource", "DataSource");
            SqlMapping.Add("DataSourceLink", "DataSourceLink");
            SqlMapping.Add("ArticleTime", "ArticleTime");
            SqlServerBulkCopy.SqlBulkMapping(SqlMapping);
            SqlServerBulkCopy.ConnStr = ConnStr;
            SqlServerBulkCopy.SqlBulkCopyToServer(data, "Article");
        }
        public void InsertBulk(List<ArticleInfo> ArticleList)
        {
            List<string> HashList = GetHashlist();
            ArticleList = ArticleList.Where(x => HashList.Contains(x.HashCode) == false).ToList();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = string.Format(@"INSERT INTO [dbo].[Article] ([HashCode] ,[CreateTime] ,[DataTitle] ,[DataContent] ,[DataType] ,[KeyWordSort] ,[DataSource] ,[DataSourceLink] ,[ArticleTime])
					 VALUES (@HashCode ,@CreateTime ,@DataTitle ,@DataContent ,@DataType ,@KeyWordSort ,@DataSource ,@DataSourceLink ,@ArticleTime ) ");
                conn.Execute(sql, ArticleList, commandTimeout: 60);
                conn.Close();
            }
        }
        public List<ArticleInfo> Getlist()
        {
            List<ArticleInfo> List = new List<ArticleInfo>();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                List = conn.Query<ArticleInfo>("select * from [dbo].[Article] with(nolock)", commandTimeout: 300).ToList();
                conn.Close();
            }
            return List;
        }
        public List<string> GetHashlist()
        {
            List<string> List = new List<string>();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                List = conn.Query<string>("select HashCode from [dbo].[Article] with(nolock)", commandTimeout: 300).ToList();
                conn.Close();
            }
            return List;
        }
    }
}
