using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carcassonne.Model.Rules;

namespace Carcassonne.Model.Expansions
{
    public class AbbotExpansion:AbstractExpansionPack
    {
        public AbbotExpansion()
        {
            m_claimRules.Add(new AbbotClaimRule());
            m_playerRules.Add(new CreateMeeplePlayerCreationRule(1, MeepleType.Abbot));
            m_scoreRules.Add(new TileRegionScoreRule(TileRegionType.Flower));
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
                return Applies(region, type) && (region as TileRegion).Owners.Count == 0;
            }

            #endregion
        }
    }
}
