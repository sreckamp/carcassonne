using System.Collections.Generic;
using System.Linq;
using GameBase.Model;

namespace Carcassonne.Model
{
    public interface ITile
    {
        ITileRegion TileRegion { get; }
        IEnumerable<IEdgeRegion> Regions { get; }
        EdgeRegionType GetEdge(EdgeDirection direction);
        IEdgeRegion GetRegion(EdgeDirection direction);
        bool Contains(EdgeRegionType type);
    }

    public class NopTile : ITile
    {
        public static readonly ITile Instance = new NopTile();

        private NopTile()
        {
        }

        /// <inheritdoc />
        public ITileRegion TileRegion => NopTileRegion.Instance;

        /// <inheritdoc />
        public IEnumerable<IEdgeRegion> Regions => Enumerable.Empty<IEdgeRegion>();

        /// <inheritdoc />
        public EdgeRegionType GetEdge(EdgeDirection direction) => NopEdgeRegion.Instance.Type;

        /// <inheritdoc />
        public IEdgeRegion GetRegion(EdgeDirection direction) => NopEdgeRegion.Instance;

        /// <inheritdoc />
        public bool Contains(EdgeRegionType type) => true;
    }
}