﻿using Carcassonne.Model.Rules;

namespace Carcassonne.Model.Expansions
{
    public class AbbotExpansion:ExpansionPack
    {
        public AbbotExpansion()
        {
            WritableClaimRules.Add(new AbbotClaimRule());
            WritablePlayerRules.Add(new CreateMeeplePlayerCreationRule(1, MeepleType.Abbot));
            WritableScoreRules.Add(new TileRegionScoreRule(TileRegionType.Flower));
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
                return Applies(region, type) && ((IPointContainer)region).Owners.Count == 0;
            }

            #endregion
        }
    }
}
