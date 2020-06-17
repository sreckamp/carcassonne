using System;
using System.Diagnostics;

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
                    Debug.WriteLine($"{newTile.TileRegion} added to {newTile}");
                }
                Debug.Write($"Add {neighbor} to {newTile}:[{string.Join(";", ((TileRegion)newTile.TileRegion).Tiles)}]");
                newTile.TileRegion.Add(neighbor);
                Debug.WriteLine($" => {newTile}:[{string.Join(";", ((TileRegion)newTile.TileRegion).Tiles)}]");
            }

            if (!m_appliesFunction(neighbor)) return;
            Debug.Write($"Add {newTile} to {neighbor}:[{string.Join(";", ((TileRegion)neighbor.TileRegion).Tiles)}]");
            neighbor.TileRegion.Add(newTile);
            Debug.WriteLine($" => {neighbor}:[{string.Join(";", ((TileRegion)neighbor.TileRegion).Tiles)}]");
        }
        
    }
}