using System.Collections.Generic;

namespace Carcassonne.Model
{
    public class CityPointRegion : PointRegion
    {
        private readonly List<Tile> m_shieldTiles = new List<Tile>();

        public CityPointRegion():base(RegionType.City){ }

        public int ShieldCount => m_shieldTiles.Count;

        protected override bool UpdateEdges(IEdgeRegion r)
        {
            var updated = base.UpdateEdges(r);
            if (!(r is CityEdgeRegion cer) || !cer.HasShield || m_shieldTiles.Contains(cer.Parent)) return updated;
            m_shieldTiles.Add(cer.Parent);
            return true;
        }
    }
}
