using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PublicUnit
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
}
