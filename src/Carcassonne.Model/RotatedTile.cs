using System.Collections.Generic;
using System.Linq;

namespace Carcassonne.Model
{
    public class RotatedTile:ITile
    {
        public RotatedTile(ITile tile, Rotation rot)
        {
            m_tile = tile;
            Rotation = rot;
        }

        private readonly ITile m_tile;
        public Rotation Rotation { get; set; }

        public TileRegion TileRegion => m_tile.TileRegion;

        public IEnumerable<IEdgeRegion> Regions => m_tile.Regions.Select(r => new RotatedEdgeRegion(r, Rotation));

        public IEnumerable<IEdgeRegion> RegionsNotRotated => m_tile.Regions;

        public RegionType GetEdge(EdgeDirection direction)
        {
            direction = direction.UnRotate(Rotation);
            return m_tile.GetEdge(direction);
        }

        public IEdgeRegion GetRegion(EdgeDirection direction)
        {
            direction = direction.UnRotate(Rotation);
            return m_tile.GetRegion(direction);
        }

        public bool Contains(RegionType type)
        {
            return m_tile.Contains(type);
        }
    }
}