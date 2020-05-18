namespace Carcassonne.Model.Rules
{
    public class MonasteryClaimRule : IClaimRule
    {
        #region IClaimRule Members

        public bool Applies(IClaimable region, MeepleType type)
        {
            return type == MeepleType.Meeple && region is ITileRegion tr && tr.Type == TileRegionType.Monastery;
        }

        public bool IsAvailable(IClaimable region, MeepleType type)
        {
            return Applies(region, type) && ((IPointContainer)region).Owners.Count == 0;
        }

        #endregion
    }
}
