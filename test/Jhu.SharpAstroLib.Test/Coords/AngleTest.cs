using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SharpAstroLib.Coords
{
    [TestClass]
    public class AngleTest
    {
        [TestMethod]
        public void TryParseHmsTest()
        {
            double a;

            Assert.IsTrue(Angle.TryParseHms("12:54:12", out a));
        }
    }
}
