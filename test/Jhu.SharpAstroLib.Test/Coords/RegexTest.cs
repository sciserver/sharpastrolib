using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SharpAstroLib.Coords
{
    [TestClass]
    public class RegexTest
    {
        private bool Match(string input, string pattern)
        {
            var r = new Regex(pattern, RegexOptions.CultureInvariant);
            var m = r.Match(input);
            return m.Success;
        }

        [TestMethod]
        public void DecimalFormatTest()
        {
            Assert.IsTrue(Match("123", Constants.DecimalFormatString));
            Assert.IsTrue(Match("12.3", Constants.DecimalFormatString));
            Assert.IsTrue(Match("123.", Constants.DecimalFormatString));
            Assert.IsTrue(Match("123.", Constants.DecimalFormatString));
            Assert.IsTrue(Match("+123.", Constants.DecimalFormatString));
            Assert.IsTrue(Match("-123.", Constants.DecimalFormatString));
        }

        [TestMethod]
        public void HmsFormatTest()
        {
            Assert.IsTrue(Match("12:34:56.43", Constants.HmsFormatString));
            Assert.IsTrue(Match("12h34m56.43s", Constants.HmsFormatString));
            Assert.IsTrue(Match("-12:34:56.43", Constants.HmsFormatString));
            Assert.IsTrue(Match("+12h34m56.43s", Constants.HmsFormatString));
        }

        [TestMethod]
        public void DmsFormatTest()
        {
            Assert.IsTrue(Match("12:34:56.43", Constants.DmsFormatString));
            Assert.IsTrue(Match("12d34m56.43s", Constants.DmsFormatString));
            Assert.IsTrue(Match("-12:34:56.43", Constants.DmsFormatString));
            Assert.IsTrue(Match("+12d34m56.43s", Constants.DmsFormatString));
            Assert.IsTrue(Match("+12°34'56.43\"", Constants.DmsFormatString));
        }

        [TestMethod]
        public void AnyAngleFormatTest()
        {
            Assert.IsTrue(Match("123", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("12.3", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("123.", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("123.", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("+123.", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("-123.", Constants.AnyAngleFormatString));

            Assert.IsTrue(Match("12:34:56.43", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("12h34m56.43s", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("-12:34:56.43", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("+12h34m56.43s", Constants.AnyAngleFormatString));

            Assert.IsTrue(Match("12:34:56.43", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("12d34m56.43s", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("-12:34:56.43", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("+12d34m56.43s", Constants.AnyAngleFormatString));
            Assert.IsTrue(Match("+12°34'56.43\"", Constants.AnyAngleFormatString));
        }
    }
}
