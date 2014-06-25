using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace MathematicalLinguistics.RegularExpression.Tests
{
    public class RealRegexCheckerTests
    {
        private RealRegexChecker _regexChecker
            = new RealRegexChecker();

        [Theory]
        [InlineData(RegexConst.IPAndSubnetMask, "192.168.1.2", true)]
        public void T(string regex, string input, bool expected)
        {
            _regexChecker.Compile(regex);
            bool result = _regexChecker.Check(input);

            Assert.Equal(expected, result);
        }
    }
}
