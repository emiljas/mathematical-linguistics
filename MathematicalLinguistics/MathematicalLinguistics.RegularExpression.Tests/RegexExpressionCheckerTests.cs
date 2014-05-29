using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace MathematicalLinguistics.RegularExpression.Tests
{
    public class RegularExpressionCheckerTests : RegexExpressionChecker
    {
        private const string FourCharactersRegex = "abcd";
        private const string TwoNumbersAddingRegex = "[0-9]+[+][0-9]+";

        [Theory]
        
        [InlineData(FourCharactersRegex, "abcd")]
        
        //[InlineData(TwoNumbersAddingRegex, "1+2")]
        public void TestCheck_ValidInput(string regex, string input)
        {
            base.Compile(regex);

            bool result = base.Check(input);

            Assert.True(result);
        }

        [Theory]
        
        [InlineData(FourCharactersRegex, "abcde")]
        [InlineData(FourCharactersRegex, "abc")]
        [InlineData(FourCharactersRegex, "")]
        public void TestCheck_InvalidInput(string regex, string input)
        {
            base.Compile(regex);

            bool result = base.Check(input);

            Assert.False(result);
        }

        [Theory]
        [InlineData("qwe[4-7]ty", 3, 5, new char[] { '4', '5', '6', '7' })]
        [InlineData("[a-cA-C]", 0, 8, new char[]{ 'a', 'b', 'c', 'A', 'B', 'C' })] 
        [InlineData("[xa-bcd]", 0, 8, new char[]{ 'x', 'a', 'b', 'c', 'd' })]
        public void TestParseCharacterGroup(string regex, int startIndex, int move, char[] characters)
        {
            base._regex = regex;
            var group = base.ParseCharacterGroup(startIndex);
            Assert.Equal(characters, group.Characters);
            Assert.Equal(move, group.Move);
        }
    }
}
