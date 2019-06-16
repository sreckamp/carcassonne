using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model.Rules
{
    public class RoadRegionScoreRule : IScoreRule
    {
        #region IScoreRule Members

        public bool Applies(IPointRegion region)
        {
            return region is PointRegion;
        }

        public int GetScore(IPointRegion region)
        {
            if (region is PointRegion pr && pr.Type == RegionType.Road && pr.IsClosed)
            {
                return pr.TileCount;
            }
            return 0;
        }

        public int GetEndScore(IPointRegion region)
        {
            if (region is PointRegion pr && pr.Type == RegionType.Road && !pr.IsClosed)
            {
                return pr.TileCount;
            }
            return 0;
        }

        #endregion
    }
}
