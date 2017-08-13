using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpeakingChamber.Extension
{
    public static class StringExtensions
    {
        public static string SupperTrim(this string str, string pattern = " ")
        {
            if (str == null) return null;
            if (pattern == null)
                throw new ArgumentNullException("pattern cannot be null");
            str = str.Trim(pattern.ToCharArray());
            while (str.Contains($"{pattern}{pattern}"))
            {
                str = str.Replace($"{pattern}{pattern}", $"{pattern}");
            }
            return str;
        }
    }
}
