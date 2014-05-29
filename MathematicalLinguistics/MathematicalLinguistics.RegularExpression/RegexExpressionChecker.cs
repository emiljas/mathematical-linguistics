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

        public bool IsValid(char character)
        {
            return Characters.Contains(character);
        }

        public State GetNextState(char character)
        {
            return NextState;
        }
    }

    public class CharacterGroup
    {
        public char[] Characters { get; set; }
        public int Move { get; set; }
    }

    public class RegexExpressionChecker
    {
        private State _firstState;
        private State _currentState;

        protected string _regex;
        private int _regexIndex;

        public void Compile(string regex)
        {
            _regex = regex;
            State state = null;

            for (_regexIndex = 0; _regexIndex < regex.Length; )
            {
                state = new State();

                if (IsCharacterGroup())
                {
                    var group = ParseCharacterGroup(_regexIndex);
                    state.Characters = group.Characters;
                    _regexIndex += group.Move;
                }
                else
                {
                    state.Characters = new char[]{ regex[_regexIndex] };
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

            var ranges = group.Replace("-", "").ToCharArray();

            var characters = new List<char>();
            for (int i = 0; i < ranges.Length; i += 2)
            {
                char rangeBegin = ranges[i];
                char rangeEnd = ranges[i + 1];

                char character = rangeBegin;
                while (character != rangeEnd)
                {
                    characters.Add(character);
                    ++character;
                }
                characters.Add(character);
            }

            return new CharacterGroup
            {
                Characters = characters.ToArray(),
                Move = groupLength + 2
            };
        }

    }
}
