using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Carcassonne.Model.Rules;
using GameBase.Model.Rules;

namespace Carcassonne.Model
{
    public abstract class AbstractExpansionPack
    {
        protected readonly List<IPlaceRule<Tile, CarcassonneMove>> m_placeRules = new List<IPlaceRule<Tile, CarcassonneMove>>();
        protected readonly List<IClaimRule> m_claimRules = new List<IClaimRule>();
        protected readonly List<IPlayerCreationRule> m_playerRules = new List<IPlayerCreationRule>();
        protected readonly List<IScoreRule> m_scoreRules = new List<IScoreRule>();

        protected AbstractExpansionPack()
        {
            PlaceRules = new ReadOnlyCollection<IPlaceRule<Tile, CarcassonneMove>>(m_placeRules);
            ClaimRules = new ReadOnlyCollection<IClaimRule>(m_claimRules);
            PlayerCreationRules = new ReadOnlyCollection<IPlayerCreationRule>(m_playerRules);
            ScoreRules = new ReadOnlyCollection<IScoreRule>(m_scoreRules);
        }

        public virtual bool IgnoreDefaultStart { get { return false; } }
        public virtual void BeforeDeckShuffle(Deck deck) { }
        public virtual void AfterDeckShuffle(Deck deck) { }
        public readonly ReadOnlyCollection<IPlaceRule<Tile, CarcassonneMove>> PlaceRules;
        public readonly ReadOnlyCollection<IClaimRule> ClaimRules;
        public readonly ReadOnlyCollection<IPlayerCreationRule> PlayerCreationRules;
        public readonly ReadOnlyCollection<IScoreRule> ScoreRules;
    }
}
