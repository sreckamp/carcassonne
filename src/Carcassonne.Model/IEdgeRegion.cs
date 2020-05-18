using System.Collections.Generic;

namespace Carcassonne.Model
{
    public interface IEdgeRegion : IClaimable
    {
        PointRegion Container { get; set; }
        RegionType Type { get; }
        Tile Parent { get; set; }
        IList<EdgeDirection> Edges { get; }
        IEdgeRegion Duplicate(Tile parent);
    }
}