using System;
using System.Collections.Generic;
using System.Text;
using BoardGameAI.Core;

namespace ConnectFour
{
    public struct TokenWindow
    {
        public TokenWindow(Coordinate[] coordinates, ConnectFourToken[] tokens)
        {
            if (coordinates == null || tokens == null)
            {
                throw new ArgumentNullException($"{nameof(coordinates)} and {nameof(tokens)} must not be null");
            }

            if (coordinates.Length != ConnectFourGame.Four || tokens.Length != ConnectFourGame.Four)
            {
                throw new ArgumentOutOfRangeException($"{nameof(coordinates)} and {nameof(tokens)} must be of length {ConnectFourGame.Four}");
            }

            Coordinates = coordinates;
            Tokens = tokens;
        }

        public Coordinate[] Coordinates { get; }
        public ConnectFourToken[] Tokens { get; }

        public IEnumerable<(Coordinate, ConnectFourToken)> Pairs { 
            get 
            { 
                for (int i = 1; i <= ConnectFourGame.Four; i++)
                {
                    yield return (Coordinates[i - 1], Tokens[i - 1]);
                }
            } 
        }
    }
}
