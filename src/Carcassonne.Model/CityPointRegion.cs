using System.Collections.Generic;

namespace Carcassonne.Model
{
    public class CityPointContainer : PointContainer
    {
        private readonly List<ITile> m_shieldTiles = new List<ITile>();

        public CityPointContainer():base(EdgeRegionType.City){ }

        public int ShieldCount => m_shieldTiles.Count;

        protected override bool UpdateEdges(IEdgeRegion r)
        {
            var updated = base.UpdateEdges(r);
            if (!(r is ICityEdgeRegion cer) || !cer.HasShield || m_shieldTiles.Contains(r.Parent)) return updated;
            m_shieldTiles.Add(r.Parent);
            return true;
        }
    }
}
