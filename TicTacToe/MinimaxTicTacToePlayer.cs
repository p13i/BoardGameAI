using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BoardGameAI.Core;

namespace TicTacToe.Game
{
    public sealed class MinimaxTicTacToePlayer : MinimaxPlayer<TicTacToeToken>
    {
        public MinimaxTicTacToePlayer(string name, TicTacToeToken token) : base(name, token)
        {
        }


        public override Player<TicTacToeToken> Clone()
        {
            return new MinimaxTicTacToePlayer(Name, Token);
        }

        public Move<TicTacToeToken> GetNextMove(TicTacToeGame game)
        {
            TicTacToeGame clone = (TicTacToeGame) game.Clone();
            (Move<TicTacToeToken> nextMove, int _) = Minimax(
                moveAndPosition: (Move<TicTacToeToken>.Default, clone),
                depth: MinimaxDepth,
                alpha: int.MinValue,
                beta: int.MaxValue,
                currentPlayer: clone.CurrentPlayer,
                maximizingCurrentPlayer: true);

            return nextMove;
        }

        public override Move<TicTacToeToken> GetNextMove(IBoardGame<TicTacToeToken> game)
        {
            return GetNextMove((TicTacToeGame)game);
        }

        protected override int MinimaxDepth => 5;
    }
}
