namespace Carcassonne.Model.Rules
{
    public class CityRoadRegionClaimRule : IClaimRule
    {
        #region IClaimRule Members

        public bool Applies(IClaimable region, MeepleType type)
        {
            return type == MeepleType.Meeple && region is IEdgeRegion er
                && (er.Type == RegionType.City || er.Type == RegionType.Road);
        }

        public bool IsAvailable(IClaimable region, MeepleType type)
        {
            return Applies(region, type) && (((IEdgeRegion)region).Container?.Owners.Count ?? 0) == 0;
        }

        #endregion
    }
}
