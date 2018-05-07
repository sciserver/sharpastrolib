using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SharpAstroLib.Coords
{
    public class Transformation
    {
        private double[] r;

        public static Transformation Identity
        {
            get { return new Transformation(); }
        }

        public static Transformation Eq2GalJ2000
        {
            get
            {
                return new Transformation()
                {
                    r = Constants.Eq2GalJ2000
                };
            }
        }

        public static Transformation Gal2EqJ2000
        {
            get
            {
                return new Transformation()
                {
                    r = Constants.Gal2EqJ2000
                };
            }
        }

        public static Transformation Rotation(double alpha, double beta, double gamma)
        {
            return new Transformation()
            {
                r = GetMatrixFromEulerAngles(alpha, beta, gamma)
            };
        }

        public static Transformation InverseRotation(double alpha, double beta, double gamma)
        {
            return new Transformation()
            {
                r = GetInverseMatrixFromEulerAngles(alpha, beta, gamma)
            };
        }

        public static Transformation Rotation(Point v, double theta)
        {
            return new Transformation()
            {
                r = GetMatrixFromAxis(v, theta)
            };
        }

        public Point Apply(Point point)
        {
            if (r == null)
            {
                return point;
            }
            else
            {
                return MatMul(r, point);
            }
        }

        private static double[] GetMatrixFromEulerAngles(double alpha, double beta, double gamma)
        {
            // Rotate around x
            double ang = alpha * Constants.Degree2Radian;
            double[] r1 = new double[]
            {
                1, 0, 0,
                0, Math.Cos(ang), -Math.Sin(ang),
                0, Math.Sin(ang), Math.Cos(ang)
            };

            // Rotate around y (Dec)
            ang = -beta * Constants.Degree2Radian;
            double[] r2 = new double[]
            {
                Math.Cos(ang), 0, Math.Sin(ang),
                0, 1, 0,
                -Math.Sin(ang), 0, Math.Cos(ang)
            };

            // Rotate around z (RA)
            ang = gamma * Constants.Degree2Radian;
            double[] r3 = new double[]
            {
                Math.Cos(ang), -Math.Sin(ang), 0,
                Math.Sin(ang), Math.Cos(ang), 0,
                0, 0, 1
            };

            double[] r = MatMul(MatMul(r3, r2), r1);

            return r;
        }

        private static double[] GetInverseMatrixFromEulerAngles(double alpha, double beta, double gamma)
        {
            // Rotate around x
            double ang = -alpha * Constants.Degree2Radian;
            double[] r1 = new double[]
            {
                1, 0, 0,
                0, Math.Cos(ang), -Math.Sin(ang),
                0, Math.Sin(ang), Math.Cos(ang)
            };

            // Rotate around y (Dec)
            ang = beta * Constants.Degree2Radian;
            double[] r2 = new double[]
            {
                Math.Cos(ang), 0, Math.Sin(ang),
                0, 1, 0,
                -Math.Sin(ang), 0, Math.Cos(ang)
            };

            // Rotate around z (RA)
            ang = -gamma * Constants.Degree2Radian;
            double[] r3 = new double[]
            {
                Math.Cos(ang), -Math.Sin(ang), 0,
                Math.Sin(ang), Math.Cos(ang), 0,
                0, 0, 1
            };

            double[] r = MatMul(r1, MatMul(r2, r3));

            return r;
        }

        private static double[] GetMatrixFromAxis(Point v, double theta)
        {
            double costh = Math.Cos(theta * Constants.Degree2Radian);
            double sinth = Math.Sin(theta * Constants.Degree2Radian);


            double[] r0 = new double[]
            {
                costh + v.X * v.X * (1 - costh), v.X * v.Y * (1 - costh) - v.Z * sinth, (v.X * v.Z) * (1 - costh) + v.Y * sinth,
                v.X * v.Y * (1 - costh) + v.Z * sinth, costh + v.Y * v.Y * (1 - costh), v.Y * v.Z * (1 - costh) - v.X * sinth,
                v.X * v.Z * (1 - costh) - v.Y * sinth, v.Y * v.Z * (1 -costh) + v.X * sinth, costh + v.Z * v.Z * (1 - costh)
            };

            return r0;
        }

        private static double[] MatMul(double[] a, double[] b)
        {
            double[] r = new double[9];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    r[3 * i + j] = 0;

                    for (int k = 0; k < 3; k++)
                    {
                        r[3 * i + j] += a[3 * i + k] * b[3 * k + j];
                    }
                }
            }

            return r;
        }

        private static Point MatMul(double[] r, Point b)
        {
            double x = r[3 * 0 + 0] * b.X + r[3 * 0 + 1] * b.Y + r[3 * 0 + 2] * b.Z;
            double y = r[3 * 1 + 0] * b.X + r[3 * 1 + 1] * b.Y + r[3 * 1 + 2] * b.Z;
            double z = r[3 * 2 + 0] * b.X + r[3 * 2 + 1] * b.Y + r[3 * 2 + 2] * b.Z;

            return new Point()
            {
                X = x,
                Y = y,
                Z = z
            };
        }
    }
}
