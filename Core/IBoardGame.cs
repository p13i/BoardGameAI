namespace BoardGameAI.Core
{
    public interface IBoardGame<TToken>
    {
        int Width { get; }
        int Height { get; }
        Player<TToken> CurrentPlayer { get; }
        Player<TToken> NextPlayer { get; }
        Player<TToken>[] Players { get; }
        int RoundNumber { get; }
        IBoardGame<TToken> Clone();
        bool IsGameOver(out Player<TToken> winningPlayer);
        bool IsBoardFull();
        bool IsMoveAllowed(Move<TToken> move);
        bool TryMove(Move<TToken> move);
        string ToString();
        Player<TToken> GetOtherPlayer(Player<TToken> player);
        int GetHashCode();
        bool Equals(object obj);
    }
}