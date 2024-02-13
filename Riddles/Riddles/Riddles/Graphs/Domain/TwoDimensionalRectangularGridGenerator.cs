using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Graphs.Domain
{
    public class TwoDimensionalRectangularGridGenerator
    {
        public List<Location> GenerateGrid(int length, int width)
        {
            return Enumerable.Range(0, length * width)
                .Select(x => new Location(id: x, length: length, width: width))
                .OrderBy(x => x.Id)
                .ToList();
        }

        public class Location : IGraphLocation
        {
            private List<Location> _adjacentLocations;
            private int _length;
            private int _width;
            public Location(int row, int col, int length, int width)
            {
                this.Row = row;
                this.Column = col;
                this._length = length;
                this._width = width;
                this.Id = row * width + col;
            }

            public Location(int id, int length, int width)
            {
                this.Row = id / width;
                this.Column = id % width;
                this._length = length;
                this._width = width;
                this.Id = id;
            }
            public int Row { get; }

            public int Column { get; }

            public int Id { get; }

            public override bool Equals(object obj)
            {
                if (!(obj is Location))
                {
                    return false;
                }
                Location other = (Location)obj;
                return other.Row == this.Row && other.Column == this.Column;
            }

            public override int GetHashCode()
            {
                return $"{this.Row}_{this.Column}".GetHashCode();
            }

            public IEnumerable<IGraphLocation> GetAdjacentLocations()
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
                        d => (this.Row + d.Item1, this.Column + d.Item2)
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
