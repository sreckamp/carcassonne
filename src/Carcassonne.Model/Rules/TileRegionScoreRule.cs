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

        private (bool applies, ITileRegion container) AppliesAsTileRegion(IPointContainer container)
        {
            if(!(container is ITileRegion region)) return (false, NopTileRegion.Instance);
            return (region.Type == m_type, region);
        }

        public int GetScore(IPointContainer container)
        {
            var (applies, region) = AppliesAsTileRegion(container);
            return applies && container.IsClosed ? region.TileCount : 0;
        }

        public int GetEndScore(IPointContainer container)
        {
            var (applies, region) = AppliesAsTileRegion(container);
            return applies  ? region.TileCount : 0;
        }

        #endregion
    }
}
