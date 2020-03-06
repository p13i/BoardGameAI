using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    public struct TokenWindow
    {
        public TokenWindow(Grid<Token> grid, params Coordinate[] coordinates)
        {
            Pairs = new (Coordinate, Token)[coordinates.Length];

            for (int i = 0; i < coordinates.Length; i++)
            {
                Coordinate coordinate = coordinates[i];

                Pairs[i] = (coordinate, grid[coordinate]);
            }
        }

        internal readonly (Coordinate, Token)[] Pairs { get; }

        internal IEnumerable<Token> Tokens => Pairs.Select(pair => pair.Item2);
        internal IEnumerable<Coordinate> Coordinates => Pairs.Select(pair => pair.Item1);

        public override string ToString()
        {
            return '[' + string.Join(", ", Pairs.Select(pair => $"{pair.Item1} = {pair.Item2}")) + ']';
        }
    }
}
