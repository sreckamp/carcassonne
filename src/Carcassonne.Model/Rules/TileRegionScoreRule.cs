using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model.Rules
{
    public class TileRegionScoreRule : IScoreRule
    {
        private readonly TileRegionType m_type;

        public TileRegionScoreRule(TileRegionType type)
        {
            m_type = type;
        }

        #region IScoreRule Members

        public bool Applies(IPointRegion region)
        {
            return applies(region, out TileRegion tr);
        }

        private bool applies(IPointRegion region, out TileRegion tr)
        {
            tr = region as TileRegion;
            return (tr?.Type == m_type);
        }

        public int GetScore(IPointRegion region)
        {
            return applies(region, out TileRegion tr) && tr.IsClosed ? tr.Score:0;
        }

        public int GetEndScore(IPointRegion region)
        {
            return applies(region, out TileRegion tr) ? tr.Score : 0;
        }

        #endregion
    }
}
