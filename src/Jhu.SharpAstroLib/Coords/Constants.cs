﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jhu.SharpAstroLib.Coords
{
    public static class Constants
    {
        public static readonly double DoublePrecision = Math.Pow(2, -53);
        public static readonly double DoublePrecision2x = 2 * DoublePrecision;

        public const float FloatTolerance = 5e-6f;
        public const double DegreeTolerance = 2.777777778e-7;

        public const double Degree2Radian = Math.PI / 180;
        public const double Radian2Degree = 180 / Math.PI;
        public const double HalfPI = 0.5 * Math.PI;
        public const double SqlNaN = -9999;

        public static readonly char[] Separators = new char[] { ' ', '\t', ',', ';' };
        public static readonly char[] HmsSeparators = new char[] { ':', 'h', 'H', 'm', 'M', 's', 'S'  };
        public static readonly char[] DmsSeparators = new char[] { ':', 'd', 'D', 'm', 'M', 's', 'S', '°', '\'', '"' };

        public static readonly string[] AsciiSymbols = new string[]
        {
            "-", "+", " ", ".", ":", "°", "'", "\"", "h", "m", "s"
        };

        public static readonly string[] UnicodeSymbols = new string[]
        {
            "\u2212",       // figure dash
            "+",
            "\u2007",        // figure space (aligns digits better)
            ".",
            ":",
            "\u00B0",
            "\u2032",    // prime
            "\u2033",    // double prime
            "\u02B0",         // superscript h
            "\u1D50",       // superscript m
            "\u02E2",       // superscript s
        };

        public static readonly string[] HtmlSymbols = new string[]
        {
            "-", "+", ".", "&nbsp;", ":", "&deg;", "'", "&quot;", "h", "m", "s"
        };

        public const string SignFormatString = @"([\+-])?";
        public const string IntegerFormatString = @"(\d+)";
        public const string DecimalFormatString = @"(\d+\.\d+|\d+|\.\d+|\d+\.)";
        public const string NumericFormatString = SignFormatString + DecimalFormatString;
        public const string HmsFormatString = SignFormatString + @"(\d{1,3}[:h°]\d{1,2}[:m]" + NumericFormatString + @"s?)";
        public const string DmsFormatString = SignFormatString + @"(\d{1,3}[:d°]\d{1,2}[:m']" + NumericFormatString + @"[s""]?)";

        public static readonly Regex HmsRegex = new Regex(@"^\s*" + HmsFormatString, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }
}
