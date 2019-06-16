using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Carcassonne.Model
{
    public class CityPointRegion : PointRegion, INotifyPropertyChanged
    {
        private readonly List<Tile> m_shieldTiles = new List<Tile>();

        public CityPointRegion():base(RegionType.City){ }

        public int ShieldCount { get { return m_shieldTiles.Count; } }

        protected override bool UpdateEdges(EdgeRegion r)
        {
            var updated = base.UpdateEdges(r);
            var cer = r as CityEdgeRegion;
            if (cer.HasShield && !m_shieldTiles.Contains(cer.Parent))
            {
                m_shieldTiles.Add(cer.Parent);
                updated = true;
            }
            return updated;
        }
    }
}
