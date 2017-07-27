using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jhu.SharpAstroLib.Util
{
    public static class RegexConverter
    {
        private const string RemovePattern = @"\(\?[ixmsnx]*\) | \?\<[a-z]+\>";
        private static readonly Regex RemovePatternRegex = new Regex(RemovePattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        public static string ToJavascript(string pattern)
        {
            return RemovePatternRegex.Replace(pattern, "");
        }
    }
}
