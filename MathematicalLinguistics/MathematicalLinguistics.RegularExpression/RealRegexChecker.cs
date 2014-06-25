using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MathematicalLinguistics.RegularExpression
{
    public class RealRegexChecker : IRegexExpressionChecker
    {
        private Regex _regex;

        public void Compile(string regex)
        {
            _regex = new Regex(regex);
        }

        public bool Check(string input)
        {
            return _regex.IsMatch(input);
        }
    }
}
