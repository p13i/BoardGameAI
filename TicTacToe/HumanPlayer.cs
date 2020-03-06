using System;

namespace TicTacToe
{
    public sealed class HumanPlayer : Player
    {
        public HumanPlayer(string name, Token token) : base(name, token) { }

        public override Player Clone()
        {
            return new HumanPlayer(Name, Token);
        }

        public override Move GetNextMove(TicTacToe game)
        {
            string input;
            Coordinate result;

            do
            {
                Console.Write($"Player {this}, enter a pair [{1}, {game.Width}], [{1}, {game.Height}]: ");
                input = Console.ReadLine();
            }
            while (!TryGetCoordinate(input, out result));

            return new Move(this, result);
        }

        private bool TryGetCoordinate(string input, out Coordinate coordinate)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                coordinate = default;
                return false;
            }

            string[] split = input.Split(",");

            if (split.Length != 2)
            {
                coordinate = default;
                return false;
            }

            if (!int.TryParse(split[0], out int x) || !int.TryParse(split[1], out int y))
            {
                coordinate = default;
                return false;
            }

            coordinate = new Coordinate(x, y);
            return true;
        }
    }
}
