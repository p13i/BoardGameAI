using System;

namespace TicTacToe
{
    public struct Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate))
            {
                return false;
            }
            else
            {
                Coordinate other = (Coordinate) obj;
                return X == other.X && Y == other.Y;
            }
        }

        public override string ToString()
        {
            return $"(x={X}, y={Y})";
        }
    }
}