using System;

namespace TicTacToe
{
    public struct TicTacToeToken
    {
        public static TicTacToeToken Blank = new TicTacToeToken("_");
        public static TicTacToeToken X = new TicTacToeToken("X");
        public static TicTacToeToken O = new TicTacToeToken("O");

        private TicTacToeToken(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(TicTacToeToken a, TicTacToeToken b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(TicTacToeToken a, TicTacToeToken b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TicTacToeToken))
            {
                return false;
            }

            TicTacToeToken other = (TicTacToeToken)obj;
            return Equals(Name, other.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
