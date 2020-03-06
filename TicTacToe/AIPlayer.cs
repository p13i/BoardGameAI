using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    public sealed class AIPlayer : Player
    {
        private const int MINIMAX_DEPTH = 4;

        public AIPlayer(string name, Token token) : base(name, token)
        {
        }

        public override Player Clone()
        {
            return new AIPlayer(Name, Token);
        }

        public override Move GetNextMove(TicTacToe game)
        {
            TicTacToe clone = game.Clone();
            (Move nextMove, int _) = Minimax(
                moveAndPosition: (Move.Default, clone),
                depth: MINIMAX_DEPTH,
                alpha: int.MinValue,
                beta: int.MaxValue,
                currentPlayer: clone.CurrentPlayer,
                maximizingCurrentPlayer: true);

            return nextMove;
        }


        public static (Move, int) Minimax(
            (Move, TicTacToe) moveAndPosition,
            int depth,
            int alpha,
            int beta,
            Player currentPlayer,
            bool maximizingCurrentPlayer)
        {
            (Move move, TicTacToe game) = moveAndPosition;

            (Move bestMove, int bestEval) = (default, default);

            if (game.IsGameOver(out _) || depth == 0)
            {
                int evaluation = Evaluation(game, currentPlayer);

                (bestMove, bestEval) = (move, evaluation);
            }
            else if (maximizingCurrentPlayer)
            {
                int maxEval = int.MinValue;
                Move maxMove = Move.Default;

                foreach ((Move childMove, TicTacToe childGame) in GetChildGameStates(game))
                {
                    (Move moveResult, int evalResult) = Minimax((childMove, childGame), depth - 1, alpha, beta, (currentPlayer), maximizingCurrentPlayer: false);

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
                Move minMove = Move.Default;

                foreach ((Move childMove, TicTacToe childGame) in GetChildGameStates(game))
                {
                    (Move moveResult, int evalResult) = Minimax((childMove, childGame), depth - 1, alpha, beta, (currentPlayer), maximizingCurrentPlayer: true);

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


        public static IEnumerable<(Move, TicTacToe)> GetChildGameStates(TicTacToe game)
        {
            for (int x = 1; x <= game.Width; x++)
            {
                for (int y = 1; y <= game.Height; y++) { 
                    Move move = new Move(game.CurrentPlayer, new Coordinate(x, y));
                    if (game.IsMoveAllowed(move))
                    {
                        TicTacToe clone = game.Clone();
                        if (!clone.TryMove(move))
                        {
                            throw new InvalidOperationException();
                        }

                        yield return (move, clone);
                    }
                }
            }
        }

        /// <summary>
        /// The evaluation of the game state is measured by the number of instances that pair of the current player's tokens are found together
        /// minus the same for the opposing player
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static int Evaluation(TicTacToe game, Player player)
        {
            Player otherPlayer = game.GetOtherPlayer(player);

            int sum = 0;
            
            if (game.IsGameOver(out Player winningPlayer) && winningPlayer != null)
            {
                if (Equals(player, winningPlayer))
                {
                    sum += 100;
                }
                else if (Equals(otherPlayer, winningPlayer))
                {
                    sum -= 80;
                }
                else  // player is null or unknown
                {
                    sum += 0;
                }
            }

            foreach (TokenWindow window in game.GetAllTokenWindows())
            {
                int score = ScoreWindow(game, window, player);
                sum += score;
            }

            return sum;
        }

        private static int ScoreWindow(TicTacToe game, TokenWindow tokenWindow, Player player)
        {
            Player otherPlayer = game.GetOtherPlayer(player);

            Token playerToken = player.Token;
            Token otherPlayerToken = otherPlayer.Token;

            int score = 0;

            {
                Coordinate center = new Coordinate(game.Width / 2 + 1, game.Height / 2 + 1);
                if (tokenWindow.Pairs.Any(pair => pair.Item1 == center && pair.Item2 == playerToken)) {
                    score += 10;
                }
            }

            return score;
        }
    }
}
