using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Downloader
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
    }
}
