using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Jhu.SharpAstroLib.Coords
{
    public class AngleFormatInfo
    {
        #region Property storage variables

        private NumberFormatInfo numberFormat;
        private string numberFormatString;
        private DegreeStyle degreeStyle;
        private DegreeWrapAroundStyle degreeWrapAroundStyle;
        private double increment;
        private bool leadingPositiveSign;
        private string positiveSign;
        private string negativeSign;
        private string figureSpace;
        private string degreeSymbol;
        private string arcMinuteSymbol;
        private string arcSecondSymbol;
        private string hourSymbol;
        private string minuteSymbol;
        private string secondSymbol;

        #endregion
        #region Properties

        public NumberFormatInfo NumberFormat
        {
            get { return numberFormat; }
            set { numberFormat = value; }

        }
        public string NumberFormatString
        {
            get { return numberFormatString; }
            set { numberFormatString = value; }
        }

        public DegreeStyle DegreeStyle
        {
            get { return degreeStyle; }
            set { degreeStyle = value; }
        }

        public DegreeWrapAroundStyle DegreeWrapAroundStyle
        {
            get { return degreeWrapAroundStyle; }
            set { degreeWrapAroundStyle = value; }
        }

        public double Increment
        {
            get { return increment; }
            set { increment = value; }
        }

        public bool LeadingPositiveSign
        {
            get { return leadingPositiveSign; }
            set { leadingPositiveSign = value; }
        }

        public string PositiveSign
        {
            get { return positiveSign; }
            set { positiveSign = value; }
        }

        public string NegativeSign
        {
            get { return negativeSign; }
            set { negativeSign = value; }
        }

        public string FigureSpace
        {
            get { return figureSpace; }
            set { figureSpace = value; }
        }

        public string DegreeSymbol
        {
            get { return degreeSymbol; }
            set { degreeSymbol = value; }
        }

        public string ArcMinuteSymbol
        {
            get { return arcMinuteSymbol; }
            set { arcMinuteSymbol = value; }
        }

        public string ArcSecondSymbol
        {
            get { return arcSecondSymbol; }
            set { arcSecondSymbol = value; }
        }

        public string HourSymbol
        {
            get { return hourSymbol; }
            set { hourSymbol = value; }
        }

        public string MinuteSymbol
        {
            get { return minuteSymbol; }
            set { minuteSymbol = value; }
        }

        public string SecondSymbol
        {
            get { return secondSymbol; }
            set { secondSymbol = value; }
        }

        #endregion
        #region Constructors and initializer

        protected AngleFormatInfo()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.numberFormat = CultureInfo.InvariantCulture.NumberFormat;
            this.numberFormatString = "n";
            this.degreeStyle = DegreeStyle.Decimal;
            this.degreeWrapAroundStyle = DegreeWrapAroundStyle.PlusMinus180;
            this.increment = 1;                 // increment in degrees
            this.leadingPositiveSign = false;

            this.negativeSign = Constants.AsciiSymbols[(int)Symbol.NegativeSign];
            this.positiveSign = Constants.AsciiSymbols[(int)Symbol.PositiveSign];
            this.figureSpace = Constants.AsciiSymbols[(int)Symbol.Space];
            this.degreeSymbol = Constants.AsciiSymbols[(int)Symbol.DegreeSymbol];
            this.arcMinuteSymbol = Constants.AsciiSymbols[(int)Symbol.ArcMinuteSymbol];
            this.arcSecondSymbol = Constants.AsciiSymbols[(int)Symbol.ArcSecondSymbol];
            this.hourSymbol = Constants.AsciiSymbols[(int)Symbol.HourSymbol];
            this.minuteSymbol = Constants.AsciiSymbols[(int)Symbol.MinuteSymbol];
            this.secondSymbol = Constants.AsciiSymbols[(int)Symbol.SecondSymbol];
        }

        public static AngleFormatInfo DefaultHours
        {
            get
            {
                return new AngleFormatInfo()
                {
                    degreeStyle = DegreeStyle.Hours,
                    increment = 0.15 / 3600.0,
                    numberFormatString = ".00",
                    hourSymbol = Constants.AsciiSymbols[(int)Symbol.Colon],
                    minuteSymbol = Constants.AsciiSymbols[(int)Symbol.Colon],
                    secondSymbol = "",
                };
            }
        }

        public static AngleFormatInfo DefaultDegrees
        {
            get
            {
                return new AngleFormatInfo()
                {
                    degreeStyle = DegreeStyle.Symbols,
                    increment = 0.01 / 3600.0,
                    numberFormatString = ".00",
                    degreeSymbol = Constants.AsciiSymbols[(int)Symbol.Colon],
                    arcMinuteSymbol = Constants.AsciiSymbols[(int)Symbol.Colon],
                    arcSecondSymbol = "",
                };
            }
        }

        public AngleFormatInfo AsciiSymbols
        {
            get
            {
                return new AngleFormatInfo();
            }
        }

        #endregion
    }
}
