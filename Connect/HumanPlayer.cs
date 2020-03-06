using System;
using System.Collections.Generic;
using System.Text;
using BoardGameAI.Core;

namespace ConnectFour
{
    public sealed class HumanPlayer : Player<ConnectFourToken>
    {
        public HumanPlayer(string name, ConnectFourToken token) : base(name, token) { }

        public override Player<ConnectFourToken> Clone()
        {
            return new HumanPlayer(Name, Token);
        }

        public override Move<ConnectFourToken> GetNextMove(IBoardGame<ConnectFourToken> game)
        {
            string input;
            Move<ConnectFourToken> move;

            do
            {
                Console.Write($"Player {this}, enter a column number [{1}, {game.Width}]: ");
                input = Console.ReadLine();
            }
            while (!int.TryParse(input, out int result) || !game.IsMoveAllowed(move = new Move<ConnectFourToken>(this, new Coordinate(result, 1))));

            return move;
        }
    }
}
