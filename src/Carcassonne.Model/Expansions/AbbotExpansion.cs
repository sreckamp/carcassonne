using System.Linq;
using Carcassonne.Model.Rules;

namespace Carcassonne.Model.Expansions
{
    public class AbbotExpansion:ExpansionPack
    {
        public static readonly ExpansionPack Instance = new AbbotExpansion();

        private AbbotExpansion()
        {
            WritableClaimRules.Add(new AbbotClaimRule());
            WritablePlayerRules.Add(new CreateMeeplePlayerCreationRule(1, MeepleType.Abbot));
            WritableScoreRules.Add(new TileRegionScoreRule(TileRegionType.Flower));
            WritableJoinRules.Add(new FlowerJoinRule());
        }

        private class AbbotClaimRule : IClaimRule
        {
            #region IClaimRule Members

            public bool Applies(IClaimable region, MeepleType type)
            {
                return type == MeepleType.Abbot && region is ITileRegion tr
                    && (tr.Type == TileRegionType.Flower || tr.Type == TileRegionType.Monastery);
            }

            public bool IsAvailable(IClaimable region, MeepleType type)
            {
                return Applies(region, type) && !((IPointContainer)region).Owners.Any();
            }

            #endregion
        }
        
        private class FlowerJoinRule : IJoinRule
        {
            public bool Applies(ITile newTile, ITile neighbor, EdgeDirection direction) =>
                neighbor != NopTile.Instance && (newTile.HasFlowers || neighbor.HasFlowers);

            public void Join(ITile newTile, ITile neighbor, EdgeDirection direction)
            {
                //TODO: All 8 directions, only 4 now.
                if (newTile.HasFlowers)
                {
                    if (newTile.TileRegion1.Type != TileRegionType.Flower)
                    {
                        newTile.TileRegion1 = new TileRegion(TileRegionType.Flower);
                    }
                    newTile.TileRegion1.Add(neighbor);
                }
                if (neighbor.HasFlowers)
                {
                    newTile.TileRegion1.Add(newTile);
                }
            }
        }
    }
}
