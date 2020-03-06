using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    public sealed class Move
    {
        public static readonly Move Default = new Move(default, default);

        public Move(Player player, Coordinate coordinate)
        {
            Player = player;
            Coordinate = coordinate;
        }

        public Player Player { get; }
        public Coordinate Coordinate { get; }

        public override int GetHashCode()
        {
            return Player.GetHashCode() ^ Coordinate.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Move))
            {
                return false;
            }

            Move other = (Move) obj;

            return Equals(Player, other.Player) && Equals(Coordinate, other.Coordinate);
        }

        public override string ToString()
        {
            return $"{Player} chose {Coordinate}";
        }
    }
}
