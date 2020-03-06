using System;
using BoardGameAI.Core;
using TicTacToe.Game;

namespace TicTacToe
{
    class Program
    {
        private static Player<TicTacToeToken> GetPlayer(string arg, TicTacToeToken token)
        {
            if (arg.Contains("ai", StringComparison.InvariantCultureIgnoreCase))
            {
                return new MinimaxTicTacToePlayer("player-1 (AI)", token);
            }
            else
            {
                return new HumanTicTacToePlayer($"player-1 ({arg})", token);
            }
        }

        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                // Print help
                Console.WriteLine("Program: <ai/<name>> <ai/<name>>");
                return 1;
            }

            Console.WriteLine("Tic Tac Toe");

            Player<TicTacToeToken> playerOne = GetPlayer(args[0], TicTacToeToken.X);

            Player<TicTacToeToken> playerTwo = GetPlayer(args[1], TicTacToeToken.O);

            TicTacToeGame game = new TicTacToeGame(playerOne, playerTwo);

            Player<TicTacToeToken> winningPlayer;

            do
            {
                Move<TicTacToeToken> nextMove;
                do
                {
                    nextMove = game.CurrentPlayer.GetNextMove(game);
                }
                while (!game.TryMove(nextMove));

                Console.WriteLine($"Round #{game.RoundNumber}");
                Console.WriteLine(game.ToString());
                Console.WriteLine();
            }
            while (!game.IsGameOver(out winningPlayer));

            if (winningPlayer != null) { 
                Console.Write("Winner: ");
                Console.WriteLine(winningPlayer.ToString());
            }
            else
            {
                Console.WriteLine("Draw!");
            }

            return 0;
        }
    }
}
