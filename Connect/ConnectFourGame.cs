using BoardGameAI.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConnectFour
{
    public class ConnectFourGame : IMinimaxGame<ConnectFourToken>
    {
        public const int Four = 4;

        public int Width { get; }
        public int Height { get; }
        public Player<ConnectFourToken>[] Players { get; }
        public Grid<ConnectFourToken> Grid { get; private set; }
        private int _currentPlayerIndex;
        public int RoundNumber { get; private set; }

        public Player<ConnectFourToken> CurrentPlayer => Players[_currentPlayerIndex];

        public Player<ConnectFourToken> NextPlayer => Players[(_currentPlayerIndex + 1) % Players.Length];

        public ConnectFourGame(int width, int height, params Player<ConnectFourToken>[] players)
        {
            if (width < Four || height < Four)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(width)} and {nameof(height)} must be greater than {Four}");
            }

            if (players == null || players.Length < 2)
            {
                throw new ArgumentOutOfRangeException($"{nameof(players)} must be at least {2}");
            }

            if (players.Select(player => player.Token).Distinct().Count() != players.Length)
            {
                throw new ArgumentException($"{nameof(players)} must all have unique tokens");
            }

            Width = width;
            Height = height;
            Players = players;
            Grid = new Grid<ConnectFourToken>(Width, Height, ConnectFourToken.Blank);
            _currentPlayerIndex = 0;
            RoundNumber = 1;
        }

        public Player<ConnectFourToken> GetOtherPlayer(Player<ConnectFourToken> player)
        {
            if (player.Equals(CurrentPlayer))
            {
                return NextPlayer;
            }
            else if (player.Equals(NextPlayer))
            {
                return CurrentPlayer;
            }

            throw new ArgumentException($"Player {player} is unknown");
        }

        private void CheckColumnNumber(int columnNumber)
        {
            if (columnNumber < 1 || columnNumber > Width)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(columnNumber)}={columnNumber} must be between {1} and {Width}");
            }
        }

        private void CheckPlayerActionAllowed(Player<ConnectFourToken> player)
        {
            if (!object.Equals(CurrentPlayer, player))
            {
                throw new InvalidOperationException($"{player} may not act now");
            }
        }

        private bool IsColumnFull(int col)
        {
            return !IsBlank(col, Height);
        }

        private bool IsBlank(int col, int row)
        {
            return Grid[col, row] == ConnectFourToken.Blank;
        }

        public bool TryMove(Move<ConnectFourToken> move)
        {
            Player<ConnectFourToken> player = move.Player;
            int col = move.Coordinate.X;

            CheckColumnNumber(col);
            CheckPlayerActionAllowed(player);

            if (IsColumnFull(col))
            {
                return false;
            }

            int lowestBlankRow = 1;

            while (!IsBlank(col, lowestBlankRow))
            {
                lowestBlankRow++;
            }

            Grid[col, lowestBlankRow] = player.Token;

            // Advance to the next player
            _currentPlayerIndex = (_currentPlayerIndex + 1) % Players.Length;
            RoundNumber++;
            return true;
        }

        IBoardGame<ConnectFourToken> IBoardGame<ConnectFourToken>.Clone()
        {
            return Clone();
        }

        public bool IsGameOver(out Player<ConnectFourToken> winningPlayer)
        {
            // 1. Check all the rows
            foreach (TokenWindow tokenWindow in GetAllTokenWindows())
            {
                if (IsWinningTokenWindow(tokenWindow))
                {
                    winningPlayer = GetPlayerWithToken(tokenWindow.Tokens[0]);
                    return true;
                }
            }

            if (Enumerable.Range(1, Width).All(IsColumnFull))
            {
                winningPlayer = null;
                return true;
            }

            winningPlayer = default;
            return false;
        }

        public bool IsBoardFull()
        {
            throw new NotImplementedException();
        }

        public bool IsMoveAllowed(Move<ConnectFourToken> move)
        {
            return !IsColumnFull(move.Coordinate.X);
        }

        private Player<ConnectFourToken> GetPlayerWithToken(ConnectFourToken token)
        {
            return Players
                .Where(player => player.Token.Equals(token))
                .First();
        }

        public override string ToString()
        {
            StringBuilder rowBuilder = new StringBuilder();

            for (int row = Height; row >= 1; row--)
            {
                StringBuilder colBuilder = new StringBuilder();

                colBuilder.Append('|');
                for (int col = 1; col <= Width; col++)
                {
                    ConnectFourToken token = Grid[col, row];

                    colBuilder.Append(token.GetString());
                    colBuilder.Append('|');
                }

                colBuilder.Append(Environment.NewLine);

                // Add this new row to the rowBuilder
                rowBuilder.Append(colBuilder);
            }

            return rowBuilder.ToString();
        }

        [DebuggerStepThrough]
        public IMinimaxGame<ConnectFourToken> Clone()
        {
            ConnectFourGame clone = new ConnectFourGame(Width, Height, Players.Select(p => p.Clone()).ToArray());
            clone.Grid = Grid.Clone();
            clone._currentPlayerIndex = _currentPlayerIndex;
            clone.RoundNumber = RoundNumber;

            return clone;
        }

        public IEnumerable<TokenWindow> GetAllTokenWindows()
        {
            // Rows
            for (int row = 1; row <= Height; row++)
            {
                for (int col = 1; col <= Width - 3; col++)
                {
                    Coordinate[] coordinates =
                    {
                        new Coordinate(col, row), new Coordinate(col + 1, row), new Coordinate(col + 2, row),
                        new Coordinate(col + 3, row)
                    };
                    ConnectFourToken[] tokens =
                        {Grid[coordinates[0]], Grid[coordinates[1]], Grid[coordinates[2]], Grid[coordinates[3]]};

                    yield return new TokenWindow(coordinates, tokens);
                }
            }

            // Columns
            for (int col = 1; col <= Width; col++)
            {
                for (int row = 1; row <= Height - 3; row++)
                {
                    Coordinate[] coordinates =
                    {
                        new Coordinate(col, row), new Coordinate(col, row + 1), new Coordinate(col, row + 2),
                        new Coordinate(col, row + 3)
                    };
                    ConnectFourToken[] tokens =
                        {Grid[coordinates[0]], Grid[coordinates[1]], Grid[coordinates[2]], Grid[coordinates[3]]};

                    yield return new TokenWindow(coordinates, tokens);
                }
            }


            // First diagonal (positive)
            for (int col = 1; col <= Width - 3; col++)
            {
                for (int row = 1; row <= Height - 3; row++)
                {
                    Coordinate[] coordinates = { new Coordinate(col, row), new Coordinate(col + 1, row + 1), new Coordinate(col + 2, row + 2), new Coordinate(col + 3, row + 3) };
                    ConnectFourToken[] tokens = { Grid[coordinates[0]], Grid[coordinates[1]], Grid[coordinates[2]], Grid[coordinates[3]] };

                    yield return new TokenWindow(coordinates, tokens);
                }
            }

            // Second diagonal (negative)
            for (int col = 1; col <= Width - 3; col++)
            {
                for (int row = 4; row <= Height - 3; row++)
                {
                    Coordinate[] coordinates = { new Coordinate(col, row), new Coordinate(col + 1, row - 1), new Coordinate(col + 2, row - 2), new Coordinate(col + 3, row - 3) };
                    ConnectFourToken[] tokens = { Grid[coordinates[0]], Grid[coordinates[1]], Grid[coordinates[2]], Grid[coordinates[3]] };

                    yield return new TokenWindow(coordinates, tokens);
                }
            }

        }

        public bool IsWinningTokenWindow(TokenWindow tokenWindow)
        {
            return tokenWindow.Tokens.Distinct().Count() == 1 &&
                   tokenWindow.Tokens.All(t => t != ConnectFourToken.Blank);
        }

        public IEnumerable<(Move<ConnectFourToken>, IMinimaxGame<ConnectFourToken>)> GetChildGameStates()
        {
            for (int i = 1; i <= Width; i++)
            {
                Move<ConnectFourToken> move = new Move<ConnectFourToken>(CurrentPlayer, new Coordinate(i, 1));
                if (IsMoveAllowed(move))
                {
                    IMinimaxGame<ConnectFourToken> clone = Clone();
                    clone.TryMove(move);
                    yield return (move, clone);
                }
            }
        }

        public int Evaluation(Player<ConnectFourToken> player)
        {
            int score = 0;

            foreach (TokenWindow tokenArray in GetAllTokenWindows())
            {
                score += ScoreWindow(tokenArray, player.Token);
            }

            return score;
        }

        private int ScoreWindow(TokenWindow tokenWindow, ConnectFourToken playerToken)
        {
            // Ignore blank windows
            if (tokenWindow.Tokens.All(t => t == ConnectFourToken.Blank))
            {
                return 0;
            }

            int score = 0;

            // This player has four in a row
            if (tokenWindow.Tokens.All(t => t == playerToken))
            {
                score += 100;
            }
            // This player has three placed with one blank in a window
            else if (tokenWindow.Tokens.Count(t => t == playerToken) == 3 &&
                     tokenWindow.Tokens.Count(t => t == ConnectFourToken.Blank) == 1)
            {
                score += 50;
            }
            // Player has two tokens and two blank spaces in a window
            else if (tokenWindow.Tokens.Count(t => t == playerToken) == 2 &&
                     tokenWindow.Tokens.Count(t => t == ConnectFourToken.Blank) == 2)
            {
                score += 20;
            }
            // Player has two tokens and two blank spaces in a window
            else if (tokenWindow.Tokens.Count(t => t == playerToken) == 1 &&
                     tokenWindow.Tokens.Count(t => t == ConnectFourToken.Blank) == 3)
            {
                score += 10;
            }
            // Other player player has four in a row
            else if (tokenWindow.Tokens.All(t => t != playerToken && t != ConnectFourToken.Blank))
            {
                score -= 100;
            }
            // Other player has three tokens and one blank space in a window
            else if (tokenWindow.Tokens.Count(t => t != playerToken && t != ConnectFourToken.Blank) == 3 &&
                     tokenWindow.Tokens.Count(t => t == ConnectFourToken.Blank) == 1)
            {
                score -= 50;
            }
            // Other player has two tokens and two blank spaces in a window
            else if (tokenWindow.Tokens.Count(t => t != playerToken && t != ConnectFourToken.Blank) == 2 &&
                     tokenWindow.Tokens.Count(t => t == ConnectFourToken.Blank) == 2)
            {
                score -= 20;
            }
            // Other player has two tokens and two blank spaces in a window
            else if (tokenWindow.Tokens.Count(t => t != playerToken && t != ConnectFourToken.Blank) == 1 &&
                     tokenWindow.Tokens.Count(t => t == ConnectFourToken.Blank) == 3)
            {
                score -= 10;
            }


            return score;




        }
    }
}
