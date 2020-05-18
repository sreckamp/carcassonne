using System.Collections.Generic;
using GameBase.Model;

namespace Carcassonne.Model
{
    public interface ITile : IPiece
    {
        TileRegion TileRegion { get; }
        IEnumerable<IEdgeRegion> Regions { get; }
        RegionType GetEdge(EdgeDirection direction);
        IEdgeRegion GetRegion(EdgeDirection direction);
        bool Contains(RegionType type);
    }
}