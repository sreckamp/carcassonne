using System.Collections.Generic;
using GameBase.Model;

namespace Carcassonne.Model
{
    public interface ITile : IPiece
    {
        ITileRegion TileRegion { get; }
        IEnumerable<IEdgeRegion> Regions { get; }
        EdgeRegionType GetEdge(EdgeDirection direction);
        IEdgeRegion GetRegion(EdgeDirection direction);
        bool Contains(EdgeRegionType type);
    }
}