using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BoardGameAI.Core;

namespace TicTacToe
{
    public sealed class TicTacToeGame : IBoardGame<TicTacToeToken>, IMinimaxGame<TicTacToeToken>
    {
        public int Width { get; }
        public int Height { get; }

        private readonly Grid<TicTacToeToken> _grid;

        public TicTacToeGame(Player<TicTacToeToken> playerOne, Player<TicTacToeToken> playerTwo) : this(3, 3, playerOne, playerTwo) {}

        private TicTacToeGame(int width, int height, Player<TicTacToeToken> currentPlayer, Player<TicTacToeToken> nextPlayer) : this(width, height, new Grid<TicTacToeToken>(width, height, TicTacToeToken.Blank), currentPlayer, nextPlayer)
        {
        }

        private TicTacToeGame(int width, int height, Grid<TicTacToeToken> grid, Player<TicTacToeToken> currentPlayer, Player<TicTacToeToken> nextPlayer)
        {
            RoundNumber = 0;
            Width = width;
            Height = height;
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            CurrentPlayer = currentPlayer ?? throw new ArgumentNullException(nameof(currentPlayer));
            NextPlayer = nextPlayer ?? throw new ArgumentNullException(nameof(nextPlayer));

            if (Players.Length != 2 || Players.Select(p => p.Token).Distinct().Count() != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(Player<TicTacToeToken>.Token));
            };
        }

        public Player<TicTacToeToken> CurrentPlayer { get; private set; }

        public IBoardGame<TicTacToeToken> Clone()
        {
            return new TicTacToeGame(
                Width, Height, 
                _grid.Clone(), 
                CurrentPlayer.Clone(), 
                NextPlayer.Clone());
        }

        public Player<TicTacToeToken> NextPlayer { get; private set; }

        public Player<TicTacToeToken>[] Players => new Player<TicTacToeToken>[] { CurrentPlayer, NextPlayer };
        public int RoundNumber { get; private set; }

        public bool IsGameOver(out Player<TicTacToeToken> winningPlayer)
        {
            return DoesThreeInARowExist(out winningPlayer) || IsBoardFull();
        }

        public bool IsBoardFull()
        {
            for (int x = 1; x <= Width; x++)
            {
                for (int y = 1; y <= Height; y++)
                {
                    if (_grid[x, y] == TicTacToeToken.Blank)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool DoesThreeInARowExist(out Player<TicTacToeToken> winningPlayer)
        {
            foreach (TokenWindow<TicTacToeToken> window in GetAllTokenWindows())
            {
                if (IsWinningTokenWindow(window))
                {
                    winningPlayer = GetPlayerForToken(window.Tokens.First());
                    return true;
                }
            }
            winningPlayer = default;
            return false;
        }

        public Player<TicTacToeToken> GetPlayerForToken(TicTacToeToken token)
        {
            return Players
                .Single(p => p.Token.Equals(token));
        }

        public IEnumerable<TokenWindow<TicTacToeToken>> GetAllTokenWindows()
        {
            // Rows
            yield return new TokenWindow<TicTacToeToken>(_grid, new Coordinate(1, 1), new Coordinate(2, 1), new Coordinate(3, 1));
            yield return new TokenWindow<TicTacToeToken>(_grid, new Coordinate(1, 2), new Coordinate(2, 2), new Coordinate(3, 2));
            yield return new TokenWindow<TicTacToeToken>(_grid, new Coordinate(1, 3), new Coordinate(2, 3), new Coordinate(3, 3));

            // Columns
            yield return new TokenWindow<TicTacToeToken>(_grid, new Coordinate(1, 1), new Coordinate(1, 2), new Coordinate(1, 3));
            yield return new TokenWindow<TicTacToeToken>(_grid, new Coordinate(2, 1), new Coordinate(2, 2), new Coordinate(2, 3));
            yield return new TokenWindow<TicTacToeToken>(_grid, new Coordinate(3, 1), new Coordinate(3, 2), new Coordinate(3, 3));

            // Diagonals
            yield return new TokenWindow<TicTacToeToken>(_grid, new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3));
            yield return new TokenWindow<TicTacToeToken>(_grid, new Coordinate(1, 3), new Coordinate(2, 2), new Coordinate(3, 1));
        }

        public bool IsWinningTokenWindow(TokenWindow<TicTacToeToken> window)
        {
            return window.Tokens.Distinct().Count() == 1 && window.Tokens.All(t => t != TicTacToeToken.Blank);
        }

        public bool IsMoveAllowed(Move<TicTacToeToken> move)
        {
            return CurrentPlayer.Equals(move.Player) && _grid[move.Coordinate] == TicTacToeToken.Blank;
        }

        public bool TryMove(Move<TicTacToeToken> move)
        {
            if (!IsMoveAllowed(move))
            {
                return false;
            }

            _grid[move.Coordinate] = move.Player.Token;

            // Swap the current player and the next player
            Player<TicTacToeToken> temp = CurrentPlayer;
            CurrentPlayer = NextPlayer;
            NextPlayer = temp;

            RoundNumber++;

            return true;
        }

        public override string ToString()
        {
            return _grid.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Player<TicTacToeToken> GetOtherPlayer(Player<TicTacToeToken> player)
        {
            if (Equals(CurrentPlayer, player))
            {
                return NextPlayer;
            }
            
            if (Equals(NextPlayer, player))
            {
                return CurrentPlayer;
            }

            throw new ArgumentException(nameof(player));
        }

        public override int GetHashCode()
        {
            return Width 
                ^ Height 
                ^ _grid.GetHashCode() 
                ^ Players.Select(p => p.GetHashCode()).Aggregate((a, b) => a ^ b)
                ^ CurrentPlayer.GetHashCode()
                ^ NextPlayer.GetHashCode();
        }

        public IEnumerable<(Move<TicTacToeToken>, IMinimaxGame<TicTacToeToken>)> GetChildGameStates()
        {
            for (int x = 1; x <= Width; x++)
            {
                for (int y = 1; y <= Height; y++)
                {
                    Move<TicTacToeToken> move = new Move<TicTacToeToken>(CurrentPlayer, new Coordinate(x, y));
                    if (IsMoveAllowed(move))
                    {
                        IMinimaxGame<TicTacToeToken> clone = (IMinimaxGame<TicTacToeToken>)Clone();
                        if (!clone.TryMove(move))
                        {
                            throw new InvalidOperationException();
                        }

                        yield return (move, clone);
                    }
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TicTacToeGame))
            {
                return false;
            }

            TicTacToeGame other = (TicTacToeGame)obj;

            return Equals(Width, other.Width)
                && Equals(Height, other.Height)
                && Equals(_grid, other._grid)
                && Enumerable.Zip(Players, other.Players).All(players => Equals(players.First, players.Second))
                && Equals(CurrentPlayer, other.CurrentPlayer)
                && Equals(NextPlayer, other.NextPlayer);
        }


        public int Evaluation(Player<TicTacToeToken> player)
        {
            Player<TicTacToeToken> otherPlayer = this.GetOtherPlayer(player);

            int sum = 0;

            if (this.IsGameOver(out Player<TicTacToeToken> winningPlayer))
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

            foreach (TokenWindow<TicTacToeToken> window in GetAllTokenWindows())
            {
                int score = ScoreWindow(this, window, player);
                sum += score;
            }

            return sum;
        }

        private static int ScoreWindow(TicTacToeGame game, TokenWindow<TicTacToeToken> tokenWindow, Player<TicTacToeToken> player)
        {
            Player<TicTacToeToken> otherPlayer = game.GetOtherPlayer(player);

            TicTacToeToken playerToken = player.Token;
            TicTacToeToken otherPlayerToken = otherPlayer.Token;

            int score = 0;

            {
                Coordinate center = new Coordinate(game.Width / 2 + 1, game.Height / 2 + 1);
                if (tokenWindow.Pairs.Any(pair => pair.Item1 == center && pair.Item2 == playerToken))
                {
                    score += 10;
                }
            }

            return score;
        }
    }
}
