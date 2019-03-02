using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader
{
    public abstract class DownloadBase : IDownload
    {
        public string RootUrl = string.Empty;
        public string RootAddress = string.Empty;
        public HttpRequestFactory httpFactory = null;
        public string _headers = string.Empty;
        public List<string> FileList = null;
        public DownloadBase()
        {
            //DirectoryInfo folder = new DirectoryInfo(RootAddress);
            //if (folder.Exists)
            //{
            //    foreach (FileInfo file in folder.GetFiles("*.html"))
            //    {
            //        FileList.Add(file.FullName);
            //    }
            //}
        }
        public abstract void Init();
        public abstract void Run();
        public virtual void DeleteFile(string srcPath)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(srcPath);
                    FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    foreach (FileSystemInfo i in fileinfo)
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                        }
                        else
                        {
                            File.Delete(i.FullName);      //删除指定文件
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
