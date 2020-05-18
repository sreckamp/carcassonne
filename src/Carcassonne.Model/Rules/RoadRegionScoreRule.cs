namespace Carcassonne.Model.Rules
{
    public class RoadRegionScoreRule : IScoreRule
    {
        #region IScoreRule Members

        public bool Applies(IPointContainer region)
        {
            return region is PointRegion;
        }

        public int GetScore(IPointContainer region)
        {
            if (region is PointRegion pr && pr.Type == EdgeRegionType.Road && pr.IsClosed)
            {
                return pr.TileCount;
            }
            return 0;
        }

        public int GetEndScore(IPointContainer region)
        {
            if (region is PointRegion pr && pr.Type == EdgeRegionType.Road && !pr.IsClosed)
            {
                return pr.TileCount;
            }
            return 0;
        }

        #endregion
    }
}
