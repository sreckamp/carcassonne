using System.Collections.Generic;
using System.Linq;

namespace Carcassonne.Model
{
    public interface ITile
    {
        ITileRegion TileRegion1 { get; set; }
        IEnumerable<IEdgeRegion> Regions { get; }
        EdgeRegionType GetEdge(EdgeDirection direction);
        IEdgeRegion GetRegion(EdgeDirection direction);
        bool Contains(EdgeRegionType type);
        bool HasMonastery { get; }
        bool HasFlowers { get; }
    }

    public class NopTile : ITile
    {
        public static readonly ITile Instance = new NopTile();

        private NopTile()
        {
        }

        /// <inheritdoc />
        public bool HasMonastery => false;

        /// <inheritdoc />
        public bool HasFlowers => false;

        /// <inheritdoc />
        public ITileRegion TileRegion1 { get; set; } = NopTileRegion.Instance;

        /// <inheritdoc />
        public IEnumerable<IEdgeRegion> Regions => Enumerable.Empty<IEdgeRegion>();

        /// <inheritdoc />
        public EdgeRegionType GetEdge(EdgeDirection direction) => NopEdgeRegion.Instance.Type;

        /// <inheritdoc />
        public IEdgeRegion GetRegion(EdgeDirection direction) => NopEdgeRegion.Instance;

        /// <inheritdoc />
        public bool Contains(EdgeRegionType type) => true;

        public void Join(ITile getNeighbor, EdgeDirection dir) { }
    }
}