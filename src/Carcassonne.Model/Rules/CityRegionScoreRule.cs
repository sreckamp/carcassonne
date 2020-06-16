namespace Carcassonne.Model.Rules
{
    public class CityRegionScoreRule : IScoreRule
    {
        #region IScoreRule Members

        public bool Applies(IPointContainer region)
        {
            return region is CityPointContainer;
        }

        public int GetScore(IPointContainer region)
        {
            if (region is CityPointContainer cpr && cpr.IsClosed)
            {
                return 2 * (cpr.TileCount + cpr.ShieldCount);
            }
            return 0;
        }

        public int GetEndScore(IPointContainer region)
        {
            if (region is CityPointContainer cpr && !cpr.IsClosed)
            {
                return cpr.TileCount + cpr.ShieldCount;
            }
            return 0;
        }

        #endregion
    }
}
