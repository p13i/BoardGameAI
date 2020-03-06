using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameAI.Core
{
    public sealed class Move<TToken>
    {
        public static readonly Move<TToken> Default = new Move<TToken>(default, default);

        public Move(Player<TToken> player, Coordinate coordinate)
        {
            Player = player;
            Coordinate = coordinate;
        }

        public Player<TToken> Player { get; }
        public Coordinate Coordinate { get; }

        public override int GetHashCode()
        {
            return Player.GetHashCode() ^ Coordinate.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Move<TToken>))
            {
                return false;
            }

            Move<TToken> other = (Move<TToken>) obj;

            return Equals(Player, other.Player) && Equals(Coordinate, other.Coordinate);
        }

        public override string ToString()
        {
            return $"{Player} chose {Coordinate}";
        }
    }
}
