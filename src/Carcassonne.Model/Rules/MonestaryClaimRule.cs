using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model.Rules
{
    public class MonestaryClaimRule : IClaimRule
    {
        #region IClaimRule Members

        public bool Applies(IClaimable region, MeepleType type)
        {
            return type == MeepleType.Meeple && region is TileRegion tr && tr?.Type == TileRegionType.Monestary;
        }

        public bool IsAvailable(IClaimable region, MeepleType type)
        {
            return Applies(region, type) && ((TileRegion)region).Owners.Count == 0;
        }

        #endregion
    }
}
