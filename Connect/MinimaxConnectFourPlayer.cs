using System;
using System.Collections.Generic;
using System.Text;
using BoardGameAI.Core;

namespace ConnectFour
{ 
    class MinimaxConnectFourPlayer : MinimaxPlayer<ConnectFourToken>
    {
        public MinimaxConnectFourPlayer(string name, ConnectFourToken token) : base(name, token)
        {
        }

        protected override int MinimaxDepth => 7;

        public override Move<ConnectFourToken> GetNextMove(IBoardGame<ConnectFourToken> game)
        {
            ConnectFourGame clone = (ConnectFourGame)game.Clone();
            (Move<ConnectFourToken> nextMove, int _) = Minimax(
                moveAndPosition: (Move<ConnectFourToken>.Default, clone),
                depth: MinimaxDepth,
                alpha: int.MinValue,
                beta: int.MaxValue,
                currentPlayer: clone.CurrentPlayer,
                maximizingCurrentPlayer: true);

            if (!game.IsMoveAllowed(nextMove))
            {
                throw new InvalidOperationException();
            }

            return nextMove;
        }

        public override Player<ConnectFourToken> Clone()
        {
            return new MinimaxConnectFourPlayer(Name, Token);
        }
    }
}
