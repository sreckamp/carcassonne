using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Carcassonne.Model.Rules;
using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model
{
    public class RuleSet : IPlaceRule<IGameBoard, ITile>, IClaimRule,IPlayerCreationRule,IScoreRule
    {
        private readonly List<IPlaceRule<IGameBoard, ITile>> m_placeRules = new List<IPlaceRule<IGameBoard, ITile>>();
        private readonly List<IClaimRule> m_claimRules = new List<IClaimRule>();
        private readonly List<IPlayerCreationRule> m_playerRules = new List<IPlayerCreationRule>();
        private readonly List<IScoreRule> m_scoreRules = new List<IScoreRule>();

        public RuleSet(params ExpansionPack[] expansions)
        {
            m_placeRules.Add(new EmptyBoardPlaceRule());
            m_placeRules.Add(new NoneTilePlaceRule());
            m_placeRules.Add(new OccupiedPlaceRule());
            m_claimRules.Add(new CityRoadRegionClaimRule());
            m_claimRules.Add(new MonasteryClaimRule());
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

        public bool Applies(IGameBoard board, ITile tile, Point location)
        {
            return true;
        }

        public bool Fits(IGameBoard board, ITile tile, Point location)
        {
            return (m_placeRules.Where(r => r.Applies(board, tile, location)).Select(r => r.Fits(board, tile, location))).FirstOrDefault();
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

        public void UpdatePlayer(IPlayer player)
        {
            foreach (var pcr in m_playerRules)
            {
                pcr.UpdatePlayer(player);
            }
        }

        #endregion

        #region IScoreRule Members

        public bool Applies(IPointContainer region)
        {
            return true;
        }

        public int GetScore(IPointContainer region)
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

        public int GetEndScore(IPointContainer region)
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
