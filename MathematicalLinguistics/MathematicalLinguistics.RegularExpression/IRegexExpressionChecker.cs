using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.RegularExpression
{
    public interface IRegexExpressionChecker
    {
        void Compile(string regex);
        bool Check(string input);
    }
}
