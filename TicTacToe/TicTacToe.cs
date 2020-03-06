using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TicTacToe
{
    public sealed class TicTacToe
    {
        public int Width { get; }
        public int Height { get; }

        private readonly Grid<Token> Grid;

        public TicTacToe(Player playerOne, Player playerTwo) : this(3, 3, playerOne, playerTwo) {}

        private TicTacToe(int width, int height, Player currentPlayer, Player nextPlayer) : this(width, height, new Grid<Token>(width, height, Token.BLANK), currentPlayer, nextPlayer)
        {
        }

        private TicTacToe(int width, int height, Grid<Token> grid, Player currentPlayer, Player nextPlayer)
        {
            RoundNumber = 0;
            Width = width;
            Height = height;
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
            CurrentPlayer = currentPlayer ?? throw new ArgumentNullException(nameof(currentPlayer));
            NextPlayer = nextPlayer ?? throw new ArgumentNullException(nameof(nextPlayer));

            if (Players.Length != 2 || Players.Select(p => p.Token).Distinct().Count() != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(Player.Token));
            };
        }

        public Player CurrentPlayer { get; private set; }

        public TicTacToe Clone()
        {
            return new TicTacToe(
                Width, Height, 
                Grid.Clone(), 
                CurrentPlayer.Clone(), 
                NextPlayer.Clone());
        }

        public Player NextPlayer { get; private set; }

        public Player[] Players => new Player[] { CurrentPlayer, NextPlayer };
        public int RoundNumber { get; private set; }

        public bool IsGameOver(out Player winningPlayer)
        {
            return DoesThreeInARowExist(out winningPlayer) || IsBoardFull();
        }

        public bool IsBoardFull()
        {
            for (int x = 1; x <= Width; x++)
            {
                for (int y = 1; y <= Height; y++)
                {
                    if (Grid[x, y] == Token.BLANK)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool DoesThreeInARowExist(out Player winningPlayer)
        {
            foreach (TokenWindow window in GetAllTokenWindows())
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

        public Player GetPlayerForToken(Token token)
        {
            return Players
                .Where(p => p.Token.Equals(token))
                .Single();
        }

        public IEnumerable<TokenWindow> GetAllTokenWindows()
        {
            // Rows
            yield return new TokenWindow(Grid, new Coordinate(1, 1), new Coordinate(2, 1), new Coordinate(3, 1));
            yield return new TokenWindow(Grid, new Coordinate(1, 2), new Coordinate(2, 2), new Coordinate(3, 2));
            yield return new TokenWindow(Grid, new Coordinate(1, 3), new Coordinate(2, 3), new Coordinate(3, 3));

            // Columns
            yield return new TokenWindow(Grid, new Coordinate(1, 1), new Coordinate(1, 2), new Coordinate(1, 3));
            yield return new TokenWindow(Grid, new Coordinate(2, 1), new Coordinate(2, 2), new Coordinate(2, 3));
            yield return new TokenWindow(Grid, new Coordinate(3, 1), new Coordinate(3, 2), new Coordinate(3, 3));

            // Diagonals
            yield return new TokenWindow(Grid, new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3));
            yield return new TokenWindow(Grid, new Coordinate(1, 3), new Coordinate(2, 2), new Coordinate(3, 1));
        }

        public bool IsWinningTokenWindow(TokenWindow window)
        {
            return window.Tokens.Distinct().Count() == 1 && window.Tokens.All(t => t != Token.BLANK);
        }

        public bool IsMoveAllowed(Move move)
        {
            return CurrentPlayer.Equals(move.Player) && Grid[move.Coordinate] == Token.BLANK;
        }

        public bool TryMove(Move move)
        {
            if (!IsMoveAllowed(move))
            {
                return false;
            }

            Grid[move.Coordinate] = move.Player.Token;

            // Swap the current player and the next player
            Player temp = CurrentPlayer;
            CurrentPlayer = NextPlayer;
            NextPlayer = temp;

            RoundNumber++;

            return true;
        }

        public override string ToString()
        {
            return Grid.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Player GetOtherPlayer(Player player)
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
                ^ Grid.GetHashCode() 
                ^ Players.Select(p => p.GetHashCode()).Aggregate((a, b) => a ^ b)
                ^ CurrentPlayer.GetHashCode()
                ^ NextPlayer.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TicTacToe))
            {
                return false;
            }

            TicTacToe other = (TicTacToe)obj;

            return Equals(Width, other.Width)
                && Equals(Height, other.Height)
                && Equals(Grid, other.Grid)
                && Enumerable.Zip(Players, other.Players).All(players => Equals(players.First, players.Second))
                && Equals(CurrentPlayer, other.CurrentPlayer)
                && Equals(NextPlayer, other.NextPlayer);
        }
    }
}
