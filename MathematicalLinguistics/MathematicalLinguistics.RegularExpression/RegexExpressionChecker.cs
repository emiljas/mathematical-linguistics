using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.RegularExpression
{
    public class State
    {
        public State NextState { get; set; }
        public char[] Characters { get; set; }
        public bool IsAcceptable { get; set; }

        private bool _valid;

        int _repeatNumber = 0;
        public bool OneOrMore { get; set; }

        public bool IsValid(char character)
        {
            _valid = Characters.Contains(character);
            
            if (OneOrMore && _repeatNumber >= 1 && !_valid)
            {
                bool validInNextState = NextState.IsValid(character);
                NextState = NextState.NextState;

                return validInNextState;
            }

            return _valid;
        }

        public State GetNextState(char character)
        {
            ++_repeatNumber;

            if (OneOrMore && _valid)
                return this;

            return NextState;
        }
    }

    public class CharacterGroup
    {
        public char[] Characters { get; set; }
        public int Move { get; set; }
    }

    public class RegexExpressionChecker : IRegexExpressionChecker
    {
        private State _firstState;
        private State _currentState;

        protected string _regex;
        private int _regexIndex;

        public void Compile(string regex)
        {
            _regex = ExpandConstantRepetition(regex);
            State state = null;

            for (_regexIndex = 0; _regexIndex < _regex.Length; )
            {
                if (_regex[_regexIndex] == '+')
                {
                    _currentState.OneOrMore = true;
                    ++_regexIndex;
                    continue;
                }
                if (IsCharacterGroup())
                {
                    state = new State();
                    var group = ParseCharacterGroup(_regexIndex);
                    state.Characters = group.Characters;
                    _regexIndex += group.Move;
                }
                else
                {
                    state = new State();
                    state.Characters = new char[] { _regex[_regexIndex] };
                    ++_regexIndex;
                }

                if (_currentState != null)
                {
                    _currentState.NextState = state;
                }

                if (_firstState == null)
                {
                    _firstState = state;
                }

                _currentState = state;
            }

            if (state != null)
                state.IsAcceptable = true;
        }

        public bool Check(string input)
        {
            bool isAcceptable = false;
            char currentCharacter;
            State currentState = _firstState;

            for (int i = 0; i < input.Length; ++i)
            {
                isAcceptable = false;
                currentCharacter = input[i];

                if (currentState == null)
                    return false;

                if(!currentState.IsValid(currentCharacter))
                    return false;
                else if (currentState.IsAcceptable)
                    isAcceptable = true;

                currentState = currentState.GetNextState(currentCharacter);
            }

            return isAcceptable;
        }

        private bool IsCharacterGroup()
        {
            return _regex[_regexIndex] == '[';
        }

        protected CharacterGroup ParseCharacterGroup(int startIndex)
        {
            int groupLength = _regex.IndexOf(']', startIndex) - startIndex - 1;
            var group = _regex.Substring(startIndex + 1, groupLength);
            var characters = new List<char>();

            int dashIndex;
            while ((dashIndex = group.IndexOf('-')) != -1)
            {
                int rangeBeginIndex = dashIndex - 1;
                int rangeEndIndex = dashIndex + 1;

                if (rangeEndIndex >= group.Length)
                {
                    characters.Add('-');
                    group = group.Remove(dashIndex, 1);

                    continue;
                }

                char rangeBegin = group[rangeBeginIndex];
                char rangeEnd = group[rangeEndIndex];

                group = group.Remove(rangeBeginIndex, 3);

                char character = rangeBegin;
                while (character != rangeEnd)
                {
                    characters.Add(character);
                    ++character;
                }
                characters.Add(character);
            }

            for (int i = 0; i < group.Length; ++i)
            {
                characters.Add(group[i]);
            }

            return new CharacterGroup
            {
                Characters = characters.ToArray(),
                Move = groupLength + 2
            };
        }


        public string ExpandConstantRepetition(string regex)
        {
            regex = ExpandRoundSection(regex);
            regex = ExpandSquareSections(regex);
            
            return regex;
        }

        private string ExpandRoundSection(string regex)
        {
            int bracketOpenningIndex;

            while ((bracketOpenningIndex = regex.IndexOf('(')) != -1)
            {
                int bracketClosingIndex;
                int open = 1, close = 0;

                for (bracketClosingIndex = bracketOpenningIndex + 1; open != close; ++bracketClosingIndex)
                {
                    var c = regex[bracketClosingIndex];

                    if (c == '(') ++open;
                    if (c == ')') ++close;
                }

                var bracketContent = regex.Substring(bracketOpenningIndex + 1, bracketClosingIndex - bracketOpenningIndex - 2);

                var repetitionNumberAsString = "";
                for (int i = bracketClosingIndex + 1; regex[i] != '}'; ++i)
                {
                    if (regex[i] != '{' && regex[i] != '}')
                        repetitionNumberAsString += regex[i];
                }

                int repetitionNumber = int.Parse(repetitionNumberAsString);

                var expanded = "";
                for (int i = 0; i < repetitionNumber; ++i)
                    expanded += bracketContent;

                regex = regex.Remove(bracketOpenningIndex, bracketClosingIndex - bracketOpenningIndex + 3);
                regex = regex.Insert(bracketOpenningIndex, expanded);
            }

            return regex;
        }

        private string ExpandSquareSections(string regex)
        {
            int curlyBracketOpenningIndex;
            while ((curlyBracketOpenningIndex = regex.IndexOf('{')) != -1)
            {
                int curlyBracketClosingIndex = regex.IndexOf('}', curlyBracketOpenningIndex);

                int bracketOpenningIndex = 0;
                int bracketClosingIndex = curlyBracketOpenningIndex - 1;
                for (int i = bracketClosingIndex; ; --i)
                {
                    if (regex[i] == '[')
                    {
                        bracketOpenningIndex = i;
                        break;
                    }
                }

                int number = int.Parse(regex.Substring(curlyBracketOpenningIndex + 1, curlyBracketClosingIndex - curlyBracketOpenningIndex - 1));

                var bracket = regex.Substring(bracketOpenningIndex, bracketClosingIndex - bracketOpenningIndex + 1);

                string expanded = "";
                for (int i = 0; i < number; ++i)
                    expanded += bracket;

                regex = regex.Remove(bracketOpenningIndex, curlyBracketClosingIndex - bracketOpenningIndex + 1);
                regex = regex.Insert(bracketOpenningIndex, expanded);
            }

            return regex;
        }
    }
}
