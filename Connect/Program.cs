using System;
using System.Linq;
using BoardGameAI.Core;

namespace ConnectFour
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                // Display help
                Console.WriteLine("Program: Play connect four");
                Console.WriteLine("Usage:");
                Console.WriteLine("     Program <player one name> <player two name>");
                return 1;
            }

            // Play the game
            Console.WriteLine("Connect Four!");

            Player<ConnectFourToken> playerOne = GetPlayerForName(args[0], ConnectFourToken.X);
            Player<ConnectFourToken> playerTwo = GetPlayerForName(args[1], ConnectFourToken.O);

            const int width = 7;
            const int height = 6;

            ConnectFourGame game = new ConnectFourGame(width, height, playerOne, playerTwo);

            Player<ConnectFourToken> winningPlayer = Play(game);

            Console.WriteLine(winningPlayer == null ? "Draw or AI fails to find solution!" : $"{winningPlayer.Name} wins!");
            Console.WriteLine(game.ToString());

            return 0;
        }

        public static Player<ConnectFourToken> Play(ConnectFourGame game)
        {
            Player<ConnectFourToken> winningPlayer;
            do
            {
                Console.WriteLine($"Round #{game.RoundNumber}");

                Console.WriteLine(game.ToString());

                Move<ConnectFourToken> nextMove = game.CurrentPlayer.GetNextMove(game);

                if (nextMove == null)
                {
                    winningPlayer = null;
                    break;
                }

                Console.WriteLine(nextMove.ToString());

                game.TryMove(nextMove);

            } while (!game.IsGameOver(out winningPlayer));

            return winningPlayer;
        }

        private static Player<ConnectFourToken> GetPlayerForName(string name, ConnectFourToken token)
        {
            if (name.Contains("ai", StringComparison.InvariantCultureIgnoreCase))
            {
                return new MinimaxConnectFourPlayer($"AI {token}", token);
            }
            else
            {
                return new HumanPlayer(name, token);
            }
        }

        public static string ReadPlayerName(string arg, int playerNumber)
        {
            string name = arg;

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine($"Enter player #{playerNumber} name: ");
                name = Console.ReadLine();
            }

            return name; 
        }
    }
}
