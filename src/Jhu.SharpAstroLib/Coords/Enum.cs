using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SharpAstroLib.Coords
{
    public enum Symbol : int
    {
        NegativeSign = 0,
        PositiveSign = 1,
        Space = 2,
        DecimalSymbol = 3,
        Colon = 4,
        DegreeSymbol = 5,
        ArcMinuteSymbol = 6,
        ArcSecondSymbol = 7,
        HourSymbol = 8,
        MinuteSymbol = 9,
        SecondSymbol = 10,
    }

    public enum Unit
    {
        Radian,
        Degree,
        ArcSecond,
        ArcMinute
    }

    public enum AngleStyle
    {
        Any,
        Decimal,
        Dms,
        Hms,
    }

    public enum AngleWrapAroundStyle
    {
        ZeroTo360,
        PlusMinus180
    }
}
