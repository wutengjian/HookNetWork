using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader
{
    public abstract class DownloadBase
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
        public abstract void Run();
    }
}
