﻿using System;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace Jhu.SharpAstroLib.Coords
{
    [Serializable]
    [Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native)]
    public struct Point : INullable
    {
        public static Point Null
        {
            get { return new Point() { isNull = true }; }
        }

        private bool isNull;
        private double lon;
        private double lat;
        private double x;
        private double y;
        private double z;

        public bool IsNull
        {
            get { return isNull; }
            set { isNull = value; }
        }

        public double Lon
        {
            get
            {
                if (lon == Constants.SqlNaN)
                {
                    Cartesian2Spherical(x, y, z, out lon, out lat);
                }

                return lon;
            }
            set { lon = value; }
        }

        public double Lat
        {
            get
            {
                if (lat == Constants.SqlNaN)
                {
                    Cartesian2Spherical(x, y, z, out lon, out lat);
                }

                return lat;
            }
            set { lat = value; }
        }

        public double X
        {
            get
            {
                if (x == Constants.SqlNaN)
                {
                    Spherical2Cartesian(lon, lat, out x, out y, out z);
                }

                return x;
            }
            set { x = value; }
        }

        public double Y
        {
            get
            {
                if (y == Constants.SqlNaN)
                {
                    Spherical2Cartesian(lon, lat, out x, out y, out z);
                }

                return y;
            }
            set { y = value; }
        }

        public double Z
        {
            get
            {
                if (z == Constants.SqlNaN)
                {
                    Spherical2Cartesian(lon, lat, out x, out y, out z);
                }

                return z;
            }
            set { z = value; }
        }

        public static Point FromSpherical(double lon, double lat)
        {
            return new Point()
            {
                isNull = false,
                lon = lon,
                lat = lat,
                x = Constants.SqlNaN,
                y = Constants.SqlNaN,
                z = Constants.SqlNaN
            };
        }

        public static Point FromCartesian(double x, double y, double z)
        {
            return new Point()
            {
                isNull = false,
                lon = Constants.SqlNaN,
                lat = Constants.SqlNaN,
                x = x,
                y = y,
                z = z
            };
        }

        public static Point FromBoth(double lon, double lat, double x, double y, double z)
        {
            return new Point()
            {
                isNull = false,
                lon = lon,
                lat = lat,
                x = x,
                y = y,
                z = z
            };
        }

        public static void Cartesian2Spherical(double x, double y, double z, out double theta, out double phi)
        {
            Cartesian2SphericalRadians(x, y, z, out theta, out phi);

            theta *= Constants.Radian2Degree;
            phi *= Constants.Radian2Degree;
        }

        public static void Cartesian2SphericalRadians(double x, double y, double z, out double theta, out double phi)
        {
            double epsilon = Constants.DoublePrecision2x;
            double rdec;

            if (z >= 1)
            {
                rdec = Math.PI / 2;
            }
            else if (z <= -1)
            {
                rdec = -Math.PI / 2;
            }
            else
            {
                rdec = Math.Asin(z);
            }

            phi = rdec;

            double cd = Math.Cos(rdec);

            if (cd > epsilon || cd < -epsilon)  // is the vector pointing to the poles?
            {
                if (y > epsilon || y < -epsilon)  // is the vector in the x-z plane?
                {
                    double arg = x / cd;
                    double acos;

                    if (arg <= -1)
                    {
                        acos = Math.PI;
                    }
                    else if (arg >= 1)
                    {
                        acos = 0;
                    }
                    else
                    {
                        acos = Math.Acos(arg);
                    }

                    if (y < 0.0)
                    {
                        theta = 2 * Math.PI - acos;
                    }
                    else
                    {
                        theta = acos;
                    }
                }
                else
                {
                    theta = (x < 0.0 ? Math.PI : 0.0);
                }
            }
            else
            {
                theta = 0.0;
            }
        }

        public static void Spherical2Cartesian(double theta, double phi, out double x, out double y, out double z)
        {
            Spherical2CartesianRadians(theta * Constants.Degree2Radian, phi * Constants.Degree2Radian, out x, out y, out z);
        }

        public static void Spherical2CartesianRadians(double theta, double phi, out double x, out double y, out double z)
        {
            double epsilon = Constants.DoublePrecision2x;

            double diff;
            double cd = Math.Cos(phi);
            diff = Constants.HalfPI - phi;

            // First, compute Z, consider cases, where declination is almost
            // +/- 90 degrees
            if (diff < epsilon && diff > -epsilon)
            {
                x = 0.0;
                y = 0.0;
                z = 1.0;
                return;
            }

            diff = -Constants.HalfPI - phi;

            if (diff < epsilon && diff > -epsilon)
            {
                x = 0.0;
                y = 0.0;
                z = -1.0;
                return;
            }

            z = Math.Sin(phi);

            // If we get here, then 
            // at least z is not singular

            double quadrant;
            double qint;
            int iint;

            quadrant = theta / Math.PI * 2; // how close is it to an integer?

            // if quadrant is (almost) an integer, force x, y to particular
            // values of quad:
            // quad,   (x,y)
            // 0       (1,0)
            // 1,      (0,1)
            // 2,      (-1,0)
            // 3,      (0,-1)
            // q>3, make q = q mod 4, and reduce to above
            // q mod 4 should be 0.

            qint = (double)((int)quadrant);

            if (Math.Abs(qint - quadrant) < epsilon)
            {
                iint = (int)qint;
                iint %= 4;

                if (iint < 0)
                {
                    iint += 4;
                }

                switch (iint)
                {
                    case 0:
                        x = cd;
                        y = 0.0;
                        break;
                    case 1:
                        x = 0.0;
                        y = cd;
                        break;
                    case 2:
                        x = -cd;
                        y = 0.0;
                        break;
                    case 3:
                    default:
                        x = 0.0;
                        y = -cd;
                        break;
                }
            }
            else
            {
                x = Math.Cos(theta) * cd;
                y = Math.Sin(theta) * cd;
            }
        }

        public static Point Parse(SqlString value)
        {
            double lon, lat;

            if (!TryParse(value.Value, out lon, out lat))
            {
                throw new FormatException();
            }

            return new Point()
            {
                isNull = false,
                lon = lon,
                lat = lat,
                x = Constants.SqlNaN,
                y = Constants.SqlNaN,
                z = Constants.SqlNaN,
            };
        }

        private static bool TryParse(string value, out double lon, out double lat)
        {
            lon = lat = Constants.SqlNaN;

            // First identify parts seperated by whitespaces, colons, semicolons,
            // anything that's not used in degree notation

            var parts = value.Split(Constants.Separators, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                // Coordinates have two parts

                return false;
            }

            // Now try to parse coordinates as decimal values

            if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lon) &&
                double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lat))
            {
                // Coordinates are decimal numbers

                return true;
            }

            // Now try to interpret them as HMS and DMS values

            if (Angle.TryParseHms(parts[0], out lon) &&
                Angle.TryParseDms(parts[1], out lat))
            {
                // Coordinates are indeed HMS and DMS values

                return true;
            }

            // If everything fails, it must be some invalid string (possibly object name)

            return false;
        }

        public string ToStringRaDec(AngleFormatInfo raFormat, AngleFormatInfo decFormat)
        {
            return new Angle(lon).ToString(raFormat) + " " + new Angle(lat).ToString(decFormat);
        }

        public override string ToString()
        {
            return ToStringRaDec(AngleFormatInfo.DefaultHours, AngleFormatInfo.DefaultDegrees);
        }
    }
}
