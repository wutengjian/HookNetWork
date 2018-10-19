using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BasicExpansion
{
    public class FileHelper
    {
        public static void SavaFile(string RootAddress, string FileName, string Content)
        {
            //判断路径是否存在,不存在就创建
            if (!Directory.Exists(RootAddress))
                Directory.CreateDirectory(RootAddress);
            String FilePath = RootAddress + FileName;
            StreamWriter file = null;
            try
            {
                //文件覆盖方式添加内容
                file = new StreamWriter(FilePath, false);
                file.Write(Content);
            }
            catch (Exception ex)
            {
                //文件覆盖方式添加内容
                @FilePath = FilePath.Replace(".txt", "@.txt");
                file = new StreamWriter(FilePath, false);
                file.Write(Content);
            }
            finally
            {
                file.Close();
                file.Dispose();
            }


        }
        public static string GetHttpFileName(string url, string FileType)
        {
            return Regex.Replace(url, "(\\:|\\/|\\.|\\?|\\&)+", "_", RegexOptions.IgnoreCase | RegexOptions.Singleline) + FileType;
        }
        public static string MD5Encrypt32(string Content)
        {
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像  // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(Content));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
    }
}
