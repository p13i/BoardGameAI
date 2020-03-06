using System;
using System.Text;

namespace BoardGameAI.Core
{
    public class Grid<T>
    {
        private readonly int width;
        private readonly int height;

        private readonly T[,] grid;

        public T Default { get; }

        public Grid(int width, int height, T @default)
        {
            if (width < 1 || height < 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(width)} and {nameof(height)} must both be positive");
            }

            this.width = width;
            this.height = height;
            Default = @default;

            grid = new T[height, width];

            for (int x = 1; x <= width; x++)
            {
                for (int y = 1; y <= height; y++)
                {
                    this[x, y] = Default;
                }
            }
        }

        public T this[int x, int y]
        {
            get
            {
                CheckBounds(x, y);
                return grid[height - y, x - 1];
            }
            set
            {
                CheckBounds(x, y);
                grid[height - y, x - 1] = value;
            }
        }

        public T this[Coordinate coordinate]
        {
            get 
            {
                return this[coordinate.X, coordinate.Y];
            }
            set
            {
                this[coordinate.X, coordinate.Y] = value;
            }
        }

        private void CheckBounds(int x, int y)
        {
            if (x < 1 || x > width)
            {
                throw new ArgumentOutOfRangeException($"{nameof(x)}={x} must be between {1} and {width}");
            }

            if (y < 1 || y > height)
            {
                throw new ArgumentOutOfRangeException($"{nameof(y)}={y} must be between {1} and {height}");
            }
        }

        public Grid<T> Clone()
        {
            Grid<T> clone = new Grid<T>(width, height, Default);

            for (int i = 1; i <= width; i++) {
                for (int j = 1; j <= height; j++) {
                    clone[i, j] = this[i, j];
                }
            }

            return clone;
        }


        public override string ToString()
        {
            StringBuilder rowBuilder = new StringBuilder();

            for (int row = height; row >= 1; row--)
            {
                StringBuilder colBuilder = new StringBuilder();

                colBuilder.Append('|');
                for (int col = 1; col <= width; col++)
                {
                    T item = this[col, row];

                    colBuilder.Append(item.ToString());
                    colBuilder.Append('|');
                }

                colBuilder.Append(Environment.NewLine);

                // Add this new row to the rowBuilder
                rowBuilder.Append(colBuilder);
            }

            return rowBuilder.ToString();
        }

        public override int GetHashCode()
        {
            int hash = 0;

            for (int x = 1; x <= width; x++)
            {
                for (int y = 1; y <= height; y++)
                {
                    hash ^= this[x, y].GetHashCode();
                }
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Grid<T>))
            {
                return false;
            }

            Grid<T> other = (Grid<T>)obj;

            if (width != other.width && height != other.height)
            {
                return false;
            }

            for (int x = 1; x <= width; x++)
            {
                for (int y = 1; y <= height; y++)
                {
                    if (!Equals(this[x, y], other[x, y]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
