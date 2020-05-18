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

    public class NopEdgeRegion : IEdgeRegion
    {
        public static readonly IEdgeRegion Instance = new NopEdgeRegion();

        private NopEdgeRegion()
        {
        }

        /// <inheritdoc />
        public IPointContainer Container { get; set; }

        /// <inheritdoc />
        public EdgeRegionType Type => EdgeRegionType.Any;

        /// <inheritdoc />
        public ITile Parent { get; set; } = NopTile.Instance;

        /// <inheritdoc />
        public IList<EdgeDirection> Edges { get; } = new List<EdgeDirection>();

        /// <inheritdoc />
        public IEdgeRegion Duplicate(ITile parent) => this;
    }
}