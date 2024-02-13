using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Graphs.Model
{
    public interface IGraphLocation
    {
        public int Id { get; }
        public IEnumerable<IGraphLocation> GetAdjacentLocations();
    }
}
