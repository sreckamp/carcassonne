using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model.Rules
{
    public class CityRegionScoreRule : IScoreRule
    {
        #region IScoreRule Members

        public bool Applies(IPointRegion region)
        {
            return region is CityPointRegion;
        }

        public int GetScore(IPointRegion region)
        {
            if (region is CityPointRegion cpr && cpr.IsClosed)
            {
                return 2 * (cpr.TileCount + cpr.ShieldCount);
            }
            return 0;
        }

        public int GetEndScore(IPointRegion region)
        {
            if (region is CityPointRegion cpr && !cpr.IsClosed)
            {
                return cpr.TileCount + cpr.ShieldCount;
            }
            return 0;
        }

        #endregion
    }
}
