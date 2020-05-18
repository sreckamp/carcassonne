using System.Linq;

namespace Carcassonne.Model.Rules
{
    public class CityRoadRegionClaimRule : IClaimRule
    {
        #region IClaimRule Members

        public bool Applies(IClaimable region, MeepleType type)
        {
            return type == MeepleType.Meeple && region is IEdgeRegion er
                && (er.Type == EdgeRegionType.City || er.Type == EdgeRegionType.Road);
        }

        public bool IsAvailable(IClaimable region, MeepleType type)
        {
            return Applies(region, type) && !((IEdgeRegion)region).Container.Owners.Any();
        }

        #endregion
    }
}
