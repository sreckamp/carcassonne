using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Carcassonne.Model.Expansions;
using Carcassonne.Model.Rules;
using GameBase.Model.Rules;

namespace Carcassonne.Model
{
    public class RuleSet : IPlaceRule<IBoard, ITile>, IClaimRule, IPlayerCreationRule, IScoreRule, IJoinRule
    {
        private readonly List<IPlaceRule<IBoard, ITile>> m_placeRules = new List<IPlaceRule<IBoard, ITile>>();
        private readonly List<IClaimRule> m_claimRules = new List<IClaimRule>();
        private readonly List<IPlayerCreationRule> m_playerRules = new List<IPlayerCreationRule>();
        private readonly List<IScoreRule> m_scoreRules = new List<IScoreRule>();
        private readonly List<IJoinRule> m_joinRules = new List<IJoinRule>();
        private readonly List<Action<Deck>> m_beforeShuffle = new List<Action<Deck>>();
        private readonly List<Action<Deck>> m_afterShuffle = new List<Action<Deck>>();

        public RuleSet(IEnumerable<ExpansionPack> expansions)
        {
            AddExpansion(StandardRules.Instance);
            foreach (var exp in expansions)
            {
                AddExpansion(exp);
            }
            m_placeRules.AddRange(StandardRules.Instance.DefaultPlaceRules);
        }

        private void AddExpansion(ExpansionPack pack)
        {
            m_placeRules.AddRange(pack.PlaceRules);
            m_claimRules.AddRange(pack.ClaimRules);
            m_playerRules.AddRange(pack.PlayerCreationRules);
            m_scoreRules.AddRange(pack.ScoreRules);
            m_beforeShuffle.Add(pack.BeforeDeckShuffle);
            m_afterShuffle.Add(pack.AfterDeckShuffle);
            m_joinRules.AddRange(pack.JoinRules);
        }

        #region IPlayerCreationRule Members

        public void UpdatePlayer(IPlayer player)
        {
            foreach (var pcr in m_playerRules)
            {
                pcr.UpdatePlayer(player);
            }
        }

        #endregion

        #region IPlaceRule Members

        public bool Applies(IBoard board, ITile tile, Point location) => true;

        public bool Fits(IBoard board, ITile tile, Point location) =>
            m_placeRules.FirstOrDefault(r => r.Applies(board, tile, location))?.Fits(board, tile, location) ?? false;

        #endregion

        #region IClaimRule Members

        public bool Applies(IClaimable region, MeepleType type) => true;

        public bool IsAvailable(IClaimable region, MeepleType type)=>
            m_claimRules.FirstOrDefault(cr => cr.Applies(region, type))?.IsAvailable(region, type) ?? false;

        #endregion

        #region IScoreRule Members

        public bool Applies(IPointContainer region) => true;

        public int GetScore(IPointContainer region) =>
            m_scoreRules.FirstOrDefault(sr => sr.Applies(region))?.GetScore(region) ?? 0;

        public int GetEndScore(IPointContainer region) =>
            m_scoreRules.FirstOrDefault(sr => sr.Applies(region))?.GetEndScore(region) ?? 0;

        #endregion

        #region IJoinRule Members

        public bool Applies(ITile newTile, ITile neighbor, EdgeDirection direction) => true;

        public void Join(ITile newTile, ITile neighbor, EdgeDirection direction)
        {
            foreach(var rule in m_joinRules.Where(sr => sr.Applies(newTile, neighbor, direction)))
            {
                rule.Join(newTile, neighbor, direction);
            }
        }

        #endregion

        public void BeforeDeckShuffle(Deck deck)
        {
            foreach (var action in m_beforeShuffle)
            {
                action.Invoke(deck);
            }
        }

        public void AfterDeckShuffle(Deck deck)
        {
            foreach (var action in m_afterShuffle)
            {
                action.Invoke(deck);
            }
        }
    }
}
