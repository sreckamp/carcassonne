﻿using System.Collections.Generic;
using System.Linq;
using Carcassonne.Model.Rules;
using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model
{
    public class RuleSet : IPlaceRule<Tile, CarcassonneMove>, IClaimRule,IPlayerCreationRule,IScoreRule
    {
        private readonly List<IPlaceRule<Tile, CarcassonneMove>> m_placeRules = new List<IPlaceRule<Tile, CarcassonneMove>>();
        private readonly List<IClaimRule> m_claimRules = new List<IClaimRule>();
        private readonly List<IPlayerCreationRule> m_playerRules = new List<IPlayerCreationRule>();
        private readonly List<IScoreRule> m_scoreRules = new List<IScoreRule>();

        public RuleSet(params AbstractExpansionPack[] expansions)
        {
            m_placeRules.Add(new EmptyBoardPlaceRule());
            m_placeRules.Add(new NoneTilePlaceRule());
            m_placeRules.Add(new OccupiedPlaceRule());
            m_claimRules.Add(new CityRoadRegionClaimRule());
            m_claimRules.Add(new MonestaryClaimRule());
            m_playerRules.Add(new CreateMeeplePlayerCreationRule(7, MeepleType.Meeple));
            foreach (var exp in expansions)
            {
                m_placeRules.AddRange(exp.PlaceRules);
                m_claimRules.AddRange(exp.ClaimRules);
                m_playerRules.AddRange(exp.PlayerCreationRules);
                m_scoreRules.AddRange(exp.ScoreRules);
            }
            m_scoreRules.Add(new TileRegionScoreRule(TileRegionType.Monastery));
            m_scoreRules.Add(new CityRegionScoreRule());
            m_scoreRules.Add(new RoadRegionScoreRule());
            m_placeRules.Add(new DefaultPlaceRule());
        }

        #region IPlaceRule Members

        public bool Applies(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return true;
        }

        public bool Fits(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return (m_placeRules.Where(r => r.Applies(board, tile, move)).Select(r => r.Fits(board, tile, move))).FirstOrDefault();
        }

        #endregion

        #region IClaimRule Members

        public bool Applies(IClaimable region, MeepleType type)
        {
            return true;
        }

        public bool IsAvailable(IClaimable region, MeepleType type)
        {
            var claimable = false;
            foreach (var cr in m_claimRules)
            {
                if (cr.Applies(region, type))
                {
                    if (cr.IsAvailable(region, type))
                    {
                        claimable = true;
                        break;
                    }
                }
            }
            return claimable;
        }

        #endregion

        #region IPlayerCreationRule Members

        public void UpdatePlayer(Player player)
        {
            foreach (var pcr in m_playerRules)
            {
                pcr.UpdatePlayer(player);
            }
        }

        #endregion

        #region IScoreRule Members

        public bool Applies(IPointRegion region)
        {
            return true;
        }

        public int GetScore(IPointRegion region)
        {
            foreach (var sr in m_scoreRules)
            {
                if (sr.Applies(region))
                {
                    return sr.GetScore(region);
                }
            }
            return 0;
        }

        public int GetEndScore(IPointRegion region)
        {
            foreach (var sr in m_scoreRules)
            {
                if (sr.Applies(region))
                {
                    return sr.GetEndScore(region);
                }
            }
            return 0;
        }

        #endregion
    }
}
