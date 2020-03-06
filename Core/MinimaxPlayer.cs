using System;
using BoardGameAI.Core;

namespace BoardGameAI.Core
{
    public abstract class MinimaxPlayer<TToken> : Player<TToken>
    {
        protected MinimaxPlayer(string name, TToken token) : base(name, token)
        {
        }

        protected abstract int MinimaxDepth { get; }

        protected static (Move<TToken>, int) Minimax(
            (Move<TToken>, IMinimaxGame<TToken>) moveAndPosition,
            int depth,
            int alpha,
            int beta,
            Player<TToken> currentPlayer,
            bool maximizingCurrentPlayer) {
            (Move<TToken> move, IMinimaxGame<TToken> game) = moveAndPosition;

            (Move<TToken> bestMove, int bestEval) = (default, default);

            if (game.IsGameOver(out _) || depth == 0)
            {
                int evaluation = game.Evaluation(currentPlayer);

                (bestMove, bestEval) = (move, evaluation);
            }
            else if (maximizingCurrentPlayer)
            {
                int maxEval = int.MinValue;
                Move<TToken> maxMove = Move<TToken>.Default;

                foreach ((Move<TToken> childMove, IMinimaxGame<TToken> childGame) in game.GetChildGameStates())
                {
                    (Move<TToken> moveResult, int evalResult) = Minimax((childMove, childGame), depth - 1, alpha, beta, (currentPlayer), maximizingCurrentPlayer: false);

                    if (evalResult > maxEval)
                    {
                        maxMove = childMove;
                        maxEval = evalResult;
                    }

                    alpha = Math.Max(alpha, evalResult);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                (bestMove, bestEval) = (maxMove, maxEval);
            }
            else
            {
                int minEval = int.MaxValue;
                Move<TToken> minMove = Move<TToken>.Default;

                foreach ((Move<TToken> childMove, IMinimaxGame<TToken> childGame) in game.GetChildGameStates())
                {
                    (Move<TToken> moveResult, int evalResult) = Minimax((childMove, childGame), depth - 1, alpha, beta, (currentPlayer), maximizingCurrentPlayer: true);

                    if (evalResult < minEval)
                    {
                        minMove = childMove;
                        minEval = evalResult;
                    }

                    beta = Math.Min(beta, evalResult);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                (bestMove, bestEval) = (minMove, minEval);
            }

            return (bestMove, bestEval);
        }
    }
}