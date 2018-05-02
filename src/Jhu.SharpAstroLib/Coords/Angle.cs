using System;
using System.Text;
using System.Globalization;

namespace Jhu.SharpAstroLib.Coords
{
    public struct Angle
    {
        public static Angle Null
        {
            get { return new Angle() { isNull = true }; }
        }

        private bool isNull;
        private double degrees;

        public bool IsNull
        {
            get { return isNull; }
            set { isNull = value; }
        }

        public double Degrees
        {
            get { return this.degrees; }
            set { this.degrees = value; }
        }

        public double Radians
        {
            get { return this.degrees / 180.0 * Math.PI; }
            set { this.degrees = value * 180.0 / Math.PI; }
        }

        public double ArcMinutes
        {
            get { return this.degrees * 60.0; }
            set { this.degrees = value / 60.0; }
        }

        public double ArcSeconds
        {
            get { return this.degrees * 3600.0; }
            set { this.degrees = value / 3600.0; }
        }

        public Angle(double degree)
        {
            this.isNull = false;
            this.degrees = degree;
        }

        public static explicit operator double(Angle degree)
        {
            return degree.degrees;
        }

        public static implicit operator Angle(double degree)
        {
            return new Angle(degree);
        }

        public static Angle ParseHms(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            double degree;

            if (!TryParseHms(value, out degree))
            {
                throw new FormatException();
            }

            return degree;
        }

        public static Angle ParseDms(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            double degree;

            if (!TryParseDms(value, out degree))
            {
                throw new FormatException();
            }

            return degree;
        }

        public static Angle ParseHmsOrDecimal(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            double degree;

            if (!TryParseHmsOrDecimal(value, out degree))
            {
                throw new FormatException();
            }

            return degree;
        }

        public static Angle ParseDmsOrDecimal(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            double degree;

            if (!TryParseDmsOrDecimal(value, out degree))
            {
                throw new FormatException();
            }

            return degree;
        }

        public static bool TryParseHmsOrDecimal(string value, out double degree)
        {
            return
                TryParseHms(value, out degree) ||
                Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out degree);
        }

        public static bool TryParseDmsOrDecimal(string value, out double degree)
        {
            return
                TryParseDms(value, out degree) ||
                Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out degree);
        }

        public static bool TryParseHms(string value, out double degree)
        {
            var res = TryParseDms(value, Constants.HmsSeparators, out degree);
            degree *= 15;
            return res;
        }

        public static bool TryParseDms(string value, out double degree)
        {
            return TryParseDms(value, Constants.DmsSeparators, out degree);
        }

        private static bool TryParseDms(string value, char[] separators, out double degree)
        {
            degree = double.NaN;

            if (value == null)
            {
                return false;
            }

            // Break string into parts

            var parts = value.Split(separators);

            // There can be three or four parts. Three parts means fractional seconds
            // are attached to the seconds part, in case of four parts, they're separate, but
            // stich them now and handle them together

            if (parts.Length < 3 || parts.Length > 4)
            {
                return false;
            }
            else if (parts.Length == 4)
            {
                // Stitch fractional part to seconds
                parts[2] += parts[3];
            }

            // Now we have to parse the three parts only
            int degrees, minutes;
            double seconds;

            if (!int.TryParse(parts[0], out degrees) ||
                !int.TryParse(parts[1], out minutes) ||
                !double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out seconds))
            {
                // parsing any of the three parts failed
                return false;
            }

            // So far so good, but check ranges
            if (degrees < -90 || degrees > 90)
            {
                return false;
            }

            if (minutes < 0 || minutes > 59)
            {
                return false;
            }

            if (seconds < 0 || seconds >= 60)
            {
                return false;
            }

            // Everything seems fine, convert to degrees

            degree = degrees + minutes / 60.0 + seconds / 3600.0;

            return true;
        }

        /// <summary>
        /// Breaks a decimal degree into degree, minutes and seconds.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="deg"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        public static void GetDmsParts(double value, out double deg, out double min, out double sec)
        {
            value += Constants.DegreeTolerance;

            deg = Math.Floor(value);
            min = Math.Floor((value - deg) * 60);
            sec = ((value - deg) * 60 - min) * 60;
        }

        /// <summary>
        /// Breaks a decimal degree in hours, minutes and seconds.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        public static void GetHmsParts(double value, out double hour, out double min, out double sec)
        {
            value = value / 15 + Constants.DegreeTolerance;

            hour = Math.Floor(value);
            min = Math.Floor((value - hour) * 60);
            sec = ((value - hour) * 60 - min) * 60;
        }

        /// <summary>
        /// Returns the angle as a formatted string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string FormatDegree(double value, AngleFormatInfo angleFormat)
        {
            var sb = new StringBuilder();

            if (angleFormat.DegreeWrapAroundStyle == AngleWrapAroundStyle.PlusMinus180 &&
                value > 180)
            {
                value -= 360;
            }

            if (angleFormat.LeadingPositiveSign && value > 0)
            {
                sb.Append(angleFormat.PositiveSign);
            }
            else if (value < 0)
            {
                sb.Append(angleFormat.NegativeSign);
            }
            else
            {
                sb.Append(angleFormat.FigureSpace);
            }

            switch (angleFormat.DegreeStyle)
            {
                case AngleStyle.Decimal:
                    FormatDecimal(value, sb, angleFormat);
                    break;
                case AngleStyle.Dms:
                    FormatSymbols(value, sb, angleFormat);
                    break;
                case AngleStyle.Hms:
                    FormatHours(value, sb, angleFormat);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Formats a decimal degree to the D.FF° format.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sb"></param>
        private static void FormatDecimal(double value, StringBuilder sb, AngleFormatInfo angleFormat)
        {
            sb.Append(Math.Abs(value).ToString(angleFormat.NumberFormatString, angleFormat.NumberFormat));

            sb.Append(angleFormat.DegreeSymbol);
        }

        /// <summary>
        /// Formats a degree in DD°MM'SS".FF format from a decimal degree value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sb"></param>
        private static void FormatSymbols(double value, StringBuilder sb, AngleFormatInfo angleFormat)
        {
            double deg, min, sec;
            GetDmsParts(value, out deg, out min, out sec);

            var secfloor = Math.Floor(sec);
            var secfrac = sec - secfloor;

            // Display degree always
            sb.Append(Math.Abs(deg).ToString("0", angleFormat.NumberFormat));
            sb.Append(angleFormat.DegreeSymbol);

            // Display minute and seconds only if increment makes it necessary
            if (angleFormat.Increment < 1)
            {
                sb.Append(min.ToString("00", angleFormat.NumberFormat));
                sb.Append(angleFormat.ArcMinuteSymbol);
            }

            if (angleFormat.Increment < 1 / 60.0)
            {
                sb.Append(secfloor.ToString("00", angleFormat.NumberFormat));
                sb.Append(angleFormat.ArcSecondSymbol);
            }

            if (angleFormat.Increment < 1 / 3600.0)
            {
                sb.Append(secfrac.ToString(angleFormat.NumberFormatString, angleFormat.NumberFormat));
            }
        }

        /// <summary>
        /// Formats a degree in HH:MM:SS.FF format from a decimal degree value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sb"></param>
        private static void FormatHours(double value, StringBuilder sb, AngleFormatInfo angleFormat)
        {
            double hour, min, sec;

            GetHmsParts(value, out hour, out min, out sec);

            var secfloor = Math.Floor(sec);
            var secfrac = sec - secfloor;

            // Display degree always
            sb.Append(Math.Abs(hour).ToString("0", angleFormat.NumberFormat));
            sb.Append(angleFormat.HourSymbol);

            // Display minute and seconds only if increment makes it necessary
            if (angleFormat.Increment < 15)
            {
                sb.Append(min.ToString("00", angleFormat.NumberFormat));
                sb.Append(angleFormat.MinuteSymbol);
            }

            if (angleFormat.Increment < 15 / 60.0)
            {
                sb.Append(secfloor.ToString("00", angleFormat.NumberFormat));
                sb.Append(angleFormat.SecondSymbol);
            }

            if (angleFormat.Increment < 15 / 3600.0)
            {
                sb.Append(secfrac.ToString(angleFormat.NumberFormatString, angleFormat.NumberFormat));
            }
        }

        public string ToString(AngleFormatInfo angleFormat)
        {
            return FormatDegree(degrees, angleFormat);
        }
    }
}
