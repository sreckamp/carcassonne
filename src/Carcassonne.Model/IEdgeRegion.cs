using System.Collections.Generic;

namespace Carcassonne.Model
{
    public interface IEdgeRegion
    {
        IPointContainer Container { get; set; }
        EdgeRegionType Type { get; }
        ITile Parent { get; set; }
        IList<EdgeDirection> Edges { get; }
        IEdgeRegion Duplicate(ITile parent);
    }
}