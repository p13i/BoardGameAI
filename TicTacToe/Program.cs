using System;

namespace TicTacToe
{
    class Program
    {
        private static Player GetPlayer(string arg, Token token)
        {
            if (arg.Contains("ai", StringComparison.InvariantCultureIgnoreCase))
            {
                return new AIPlayer("player-1 (AI)", token);
            }
            else
            {
                return new HumanPlayer($"player-1 ({arg})", token);
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

            Player playerOne = GetPlayer(args[0], Token.X);

            Player playerTwo = GetPlayer(args[1], Token.O);

            TicTacToe game = new TicTacToe(playerOne, playerTwo);

            Player winningPlayer;

            do
            {
                Move nextMove;
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
