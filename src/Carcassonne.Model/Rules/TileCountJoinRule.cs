using System;

namespace Carcassonne.Model.Rules
{
    public abstract class TileCountJoinRule : IJoinRule
    {
        private readonly TileRegionType m_type;
        private readonly Func<ITile, bool> m_appliesFunction;

        protected TileCountJoinRule(TileRegionType type, Func<ITile, bool> appliesFunction)
        {
            m_type = type;
            m_appliesFunction = appliesFunction;
        }

        public bool Applies(ITile newTile, ITile neighbor, EdgeDirection direction) =>
            neighbor != NopTile.Instance && (m_appliesFunction(newTile) || m_appliesFunction(neighbor));

        public void Join(ITile newTile, ITile neighbor, EdgeDirection direction)
        {
            if (m_appliesFunction(newTile))
            {
                if (newTile.TileRegion.Type != m_type)
                {
                    newTile.TileRegion = new TileRegion(m_type);
                }
                newTile.TileRegion.Add(neighbor);
            }

            if (!m_appliesFunction(neighbor)) return;
            neighbor.TileRegion.Add(newTile);
        }
        
    }
}