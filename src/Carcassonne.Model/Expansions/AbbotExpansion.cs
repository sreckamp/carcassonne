using Carcassonne.Model.Rules;

namespace Carcassonne.Model.Expansions
{
    public class AbbotExpansion:AbstractExpansionPack
    {
        public AbbotExpansion()
        {
            _ClaimRules.Add(new AbbotClaimRule());
            _PlayerRules.Add(new CreateMeeplePlayerCreationRule(1, MeepleType.Abbot));
            _ScoreRules.Add(new TileRegionScoreRule(TileRegionType.Flower));
        }

        private class AbbotClaimRule : IClaimRule
        {
            #region IClaimRule Members

            public bool Applies(IClaimable region, MeepleType type)
            {
                return type == MeepleType.Abbot && region is TileRegion tr
                    && (tr.Type == TileRegionType.Flower || tr.Type == TileRegionType.Monestary);
            }

            public bool IsAvailable(IClaimable region, MeepleType type)
            {
                return Applies(region, type) && ((TileRegion) region).Owners.Count == 0;
            }

            #endregion
        }
    }
}
