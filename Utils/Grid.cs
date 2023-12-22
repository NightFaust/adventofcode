using System.Text.RegularExpressions;
using System.Linq;

namespace adventofcode.Utils
{
    public class Grid
    {
        int[,] cells;

        public delegate int EdgeHandler();

        public Grid(string input, bool asChar, char separator = (char)0, int offx = 0, int offy = 0)
        {
            var lines = input.Split('\n');
            Width = lines.Max(x => x.Length);
            Height = lines.Length;
            MinX = offx;
            MinY = offy;

            cells = new int[Width, Height];
            var y = 0;
            foreach (var lin in lines)
            {
                var x = 0;
                string[] ca;

                if (separator == (char)0)
                    ca = Regex.Split(lin, string.Empty);
                else
                    ca = lin.Split(separator);

                foreach (var c in ca)
                {
                    if (c == string.Empty) continue;
                    cells[x, y] = (!asChar) ? int.Parse(c) : c[0];
                    x++;
                }

                y++;
            }
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int MinX { get; private set; }

        public int MinY { get; private set; }

        public int MaxX => Width + MinX;
        public int MaxY => Height + MinY;

        public int this[int x, int y, bool useOffset = true]
        {
            get => cells[x - (useOffset ? MinX : 0), y - (useOffset ? MinY : 0)];
            set => cells[x - (useOffset ? MinX : 0), y - (useOffset ? MinY : 0)] = value;
        }

        public void Rotate(Orientation northBecomes)
        {
            int[,] nCells = null;
            switch (northBecomes)
            {
                case Orientation.North:
                    nCells = new int[Width, Height];
                    for (var ny = 0; ny < Height; ny++)
                    {
                        for (var nx = 0; nx < Width; nx++)
                        {
                            nCells[nx, ny] = cells[nx, ny];
                        }
                    }

                    break;
                case Orientation.South:
                    nCells = new int[Width, Height];
                    for (var ny = 0; ny < Height; ny++)
                    {
                        for (var nx = 0; nx < Width; nx++)
                        {
                            nCells[nx, ny] = cells[Width - nx - 1, Height - ny - 1];
                        }
                    }

                    MinX = Width - MinX;
                    break;
                case Orientation.East:
                    nCells = new int[Height, Width];
                    for (var ny = 0; ny < Width; ny++)
                    {
                        for (var nx = 0; nx < Height; nx++)
                        {
                            nCells[nx, ny] = cells[ny, Width - nx - 1];
                        }
                    }

                    var t = MinX;
                    MinX = -MinY;
                    MinY = t;
                    break;
                case Orientation.West:
                    nCells = new int[Height, Width];
                    for (var ny = 0; ny < Width; ny++)
                    {
                        for (var nx = 0; nx < Height; nx++)
                        {
                            nCells[nx, ny] = cells[Width - ny - 1, nx];
                        }
                    }

                    var tm = MinX;
                    MinX = MinY;
                    MinY = -tm;
                    break;
            }

            cells = nCells;
            Width = cells.GetLength(0);
            Height = cells.GetLength(1);
        }
    }
}