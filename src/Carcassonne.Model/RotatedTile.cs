﻿using System.Collections.Generic;
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

        public ITileRegion TileRegion => m_tile.TileRegion;

        public IEnumerable<IEdgeRegion> Regions => m_tile.Regions.Select(r => new RotatedEdgeRegion(r, Rotation));

        public IEnumerable<IEdgeRegion> RegionsNotRotated => m_tile.Regions;

        public EdgeRegionType GetEdge(EdgeDirection direction)
        {
            direction = direction.UnRotate(Rotation);
            return m_tile.GetEdge(direction);
        }

        public IEdgeRegion GetRegion(EdgeDirection direction)
        {
            direction = direction.UnRotate(Rotation);
            return new RotatedEdgeRegion(m_tile.GetRegion(direction), Rotation);
        }

        public bool Contains(EdgeRegionType type)
        {
            return m_tile.Contains(type);
        }

        public override string ToString() => Rotation == Rotation.None ? m_tile.ToString() : $"{m_tile} @ {Rotation}";

        public void Join(ITile neighbor, EdgeDirection dir)
        {
            m_tile.Join(neighbor, dir);
        }
    }
}