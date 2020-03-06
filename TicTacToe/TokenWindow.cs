using System.Collections.Generic;
using System.Linq;
using BoardGameAI.Core;

namespace TicTacToe
{
    public struct TokenWindow<TToken>
    {
        public TokenWindow(Grid<TToken> grid, params Coordinate[] coordinates)
        {
            Pairs = new (Coordinate, TToken)[coordinates.Length];

            for (var i = 0; i < coordinates.Length; i++)
            {
                var coordinate = coordinates[i];

                Pairs[i] = (coordinate, grid[coordinate]);
            }
        }

        internal readonly (Coordinate, TToken)[] Pairs { get; }

        internal IEnumerable<TToken> Tokens => Pairs.Select(pair => pair.Item2);
        internal IEnumerable<Coordinate> Coordinates => Pairs.Select(pair => pair.Item1);

        public override string ToString()
        {
            return '[' + string.Join(", ", Pairs.Select(pair => $"{pair.Item1} = {pair.Item2}")) + ']';
        }
    }
}