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

        public bool Applies(IPointContainer region)
        {
            return AppliesAsTileRegion(region).applies;
        }

        private (bool applies, TileRegion tileRegion) AppliesAsTileRegion(IPointContainer region)
        {
            if(!(region is TileRegion tr)) return (false, TileRegion.None);
            return (tr.Type == m_type, tr);
        }

        public int GetScore(IPointContainer region)
        {
            var (applies, tr) = AppliesAsTileRegion(region);
            return applies && tr.IsClosed ? tr.Score : 0;
        }

        public int GetEndScore(IPointContainer region)
        {
            var (applies, tr) = AppliesAsTileRegion(region);
            return applies  ? tr.Score : 0;
        }

        #endregion
    }
}
