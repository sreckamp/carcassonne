namespace Carcassonne.Model.Rules
{
    public class MonasteryClaimRule : IClaimRule
    {
        #region IClaimRule Members

        public bool Applies(IClaimable region, MeepleType type)
        {
            return type == MeepleType.Meeple && region is TileRegion tr && tr?.Type == TileRegionType.Monastery;
        }

        public bool IsAvailable(IClaimable region, MeepleType type)
        {
            return Applies(region, type) && ((TileRegion)region).Owners.Count == 0;
        }

        #endregion
    }
}
