using System;
namespace ConnectFour
{
    public enum ConnectFourToken
    {
        Blank = 0,
        X = 1,
        O = 2,
    }

    public static class TokenExtensions
    {
        public static string GetString(this ConnectFourToken token)
        {
            switch (token)
            {
                case ConnectFourToken.Blank:
                    return " ";
                case ConnectFourToken.X:
                    return "R";
                case ConnectFourToken.O:
                    return "O";
                default:
                    throw new ArgumentOutOfRangeException($"TicTacToeToken value {token} is unknown");
            }
        }
    }
}
