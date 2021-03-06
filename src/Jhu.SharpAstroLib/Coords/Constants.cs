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
        public const double Degree2ArcMin = 60.0;
        public const double Degree2ArcSec = 3600.0;
        public const double ArcMin2Degree = 1.0 / 60.0;
        public const double ArcMin2Radian = 1.0 / 60.0 / 180.0 * Math.PI;
        public const double ArcSec2Degree = 1.0 / 3600.0;
        public const double ArcSec2Radian = 1.0 / 3600.0 / 180.0 * Math.PI;
        public const double Radian2Degree = 180 / Math.PI;
        public const double HalfPI = 0.5 * Math.PI;
        public const double SqlNaN = -9999;

        public static readonly char[] Separators = new char[] { ' ', '\t', ',', ';' };
        public static readonly char[] HmsSeparators = new char[] { ':', 'h', 'H', 'm', 'M', 's', 'S' };
        public static readonly char[] DmsSeparators = new char[] { ':', 'd', 'D', 'm', 'M', 's', 'S', '°', '\'', '"' };

        public static readonly string[] AsciiSymbols = new string[]
        {
            "-", "+", " ", ".", ":", "°", "'", "\"", "h", "m", "s"
        };

        public static readonly string[] UnicodeSymbols = new string[]
        {
            "\u2212",       // figure dash
            "+",
            "\u2007",       // figure space (aligns digits better)
            ".",
            ":",
            "\u00B0",       // degree
            "\u2032",       // prime
            "\u2033",       // double prime
            "\u02B0",       // superscript h
            "\u1D50",       // superscript m
            "\u02E2",       // superscript s
        };

        public static readonly string[] HtmlSymbols = new string[]
        {
            "-", "+", ".", "&nbsp;", ":", "&deg;", "'", "&quot;", "<sup>h</sup>", "<sup>m</sup>", "<sup>s</sup>"
        };

        public static readonly string[] LatexSymbols = new string[]
        {
            "-", "+", "~", ".", ":", "^{\\circ}", "'", "''", "^{h}", "^{m}", "^{s}"
        };

        private const string BeginFormatPart = @"(?ix)^\s*";
        private const string EndFormatPart = @"\s*$";
        private const string SignFormatPart = @"(?<sign>[\+-]?)";
        private const string IntegerFormatPart = @"\d+";
        private const string DecimalFormatPart = @"\d+\.\d+|\d+|\.\d+|\d+\.";
        private const string HmsFormatPart = @"(?<hour>\d{1,3})[:h](?<min>\d{1,2})[:m](?<sec>" + DecimalFormatPart + @")s?";
        private const string DmsFormatPart = @"(?<deg>\d{1,3})[:d°](?<min>\d{1,2})[:m'](?<sec>" + DecimalFormatPart + @")[s""]?";

        public const string DecimalFormatString = BeginFormatPart + SignFormatPart + "(?<decimal>" + DecimalFormatPart + ")" + EndFormatPart;
        public const string HmsFormatString = BeginFormatPart + SignFormatPart + HmsFormatPart + EndFormatPart;
        public const string DmsFormatString = BeginFormatPart + SignFormatPart + DmsFormatPart + EndFormatPart;
        public const string AnyAngleFormatString =
            BeginFormatPart + SignFormatPart + "(?:" +
            "(?<decimal>" + DecimalFormatPart + ")" + "|" +
            HmsFormatPart + "|" +
            DmsFormatPart + ")" +
            EndFormatPart;

        /// <summary>
        /// Rotation matrix to convert equatorial coordinats to galactic
        /// J2000
        /// </summary>
        public static readonly double[] Eq2GalJ2000 = 
            {-0.054875657707, -0.873437051953, -0.483835073621,
             +0.494109437203, -0.444829721222, +0.746982183981,
             -0.867666137554, -0.198076337284, +0.455983813693};

        /// <summary>
        /// Rotation matrix to convert galactic coordinats to equatorial
        /// J2000
        /// </summary>
        public static readonly double[] Gal2EqJ2000 = 
            {-0.05487566,  0.49410944, -0.86766614,
             -0.87343705, -0.44482972, -0.19807634,
             -0.48383507,  0.74698218,  0.45598381};
    }
}
