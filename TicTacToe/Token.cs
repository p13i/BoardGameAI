using System;

namespace TicTacToe
{
    public struct Token
    {
        public static Token BLANK = new Token("_");
        public static Token X = new Token("X");
        public static Token O = new Token("O");

        private Token(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Token a, Token b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Token a, Token b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Token))
            {
                return false;
            }

            Token other = (Token)obj;
            return Equals(Name, other.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
