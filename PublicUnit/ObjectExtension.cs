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
        public static string RegexMatch(string input, string pattern, string key = null)
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
    }
}
