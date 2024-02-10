using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Graphs.Domain
{
    public class TwoDimensionalRectangularGrid
    {
        public List<Location> GenerateGrid(int length, int width)
        {
            return Enumerable.Range(0, length * width)
                .Select(x => new Location(id: x, length: length, width: width))
                .OrderBy(x => x.Id)
                .ToList();
        }

        public class Location
        {
            private List<Location> _adjacentLocations;
            private int _length;
            private int _width;
            public Location(int x, int y, int length, int width)
            {
                this.X = x;
                this.Y = y;
                this._length = length;
                this._width = width;
                this.Id = x * width + y;
            }

            public Location(int id, int length, int width)
            {
                this.X = id / width;
                this.Y = id % width;
                this._length = length;
                this._width = width;
                this.Id = id;
            }
            public int X { get; }

            public int Y { get; }

            public int Id { get; }

            public override bool Equals(object obj)
            {
                if (!(obj is Location))
                {
                    return false;
                }
                Location other = (Location)obj;
                return other.X == this.X && other.Y == this.Y;
            }

            public override int GetHashCode()
            {
                return $"{this.X}_{this.Y}".GetHashCode();
            }

            public List<Location> GetAdjacentLocations()
            {
                if (this._adjacentLocations == null)
                {
                    var directions = new List<(int, int)> {
                        (-1, 0),
                        (0, -1),
                        (1, 0),
                        (0, 1),
                    };
                    this._adjacentLocations = directions.Select(
                        d => (this.X + d.Item1, this.Y + d.Item2)
                    ).Where(l =>
                        l.Item1 >= 0
                        && l.Item2 >= 0
                        && l.Item1 < this._length
                        && l.Item2 < this._width
                    ).Select(l =>
                        new Location(l.Item1, l.Item2, this._length, this._width)
                    ).ToList();
                }
                return this._adjacentLocations;
            }
        }
    }
}
