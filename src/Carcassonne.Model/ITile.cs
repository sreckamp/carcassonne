using System.Collections.Generic;
using GameBase.Model;

namespace Carcassonne.Model
{
    public interface ITile : IPiece
    {
        TileRegion TileRegion { get; }
        IEnumerable<EdgeRegion> Regions { get; }
        RegionType GetEdge(EdgeDirection direction);
        EdgeRegion GetRegion(EdgeDirection direction);
        bool Contains(RegionType type);
    }
}