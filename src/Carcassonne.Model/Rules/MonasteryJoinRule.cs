namespace Carcassonne.Model.Rules
{
    public class MonasteryJoinRule : IJoinRule
    {
        public bool Applies(ITile newTile, ITile neighbor, EdgeDirection direction) =>
            neighbor != NopTile.Instance && (newTile.HasMonastery || neighbor.HasMonastery);

        public void Join(ITile newTile, ITile neighbor, EdgeDirection direction)
        {
            //TODO: All 8 directions, only 4 now.
            if (newTile.HasMonastery)
            {
                if (newTile.TileRegion1.Type != TileRegionType.Monastery)
                {
                    newTile.TileRegion1 = new TileRegion(TileRegionType.Monastery);
                }
                newTile.TileRegion1.Add(neighbor);
            }
            if (neighbor.HasMonastery)
            {
                newTile.TileRegion1.Add(newTile);
            }
        }
    }
}