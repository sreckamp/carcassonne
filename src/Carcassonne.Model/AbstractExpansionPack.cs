using System.Collections.Generic;
using System.Collections.ObjectModel;
using Carcassonne.Model.Rules;
using GameBase.Model.Rules;

namespace Carcassonne.Model
{
    public abstract class AbstractExpansionPack
    {
        protected readonly List<IPlaceRule<Tile, CarcassonneMove>> _PlaceRules = new List<IPlaceRule<Tile, CarcassonneMove>>();
        protected readonly List<IClaimRule> _ClaimRules = new List<IClaimRule>();
        protected readonly List<IPlayerCreationRule> _PlayerRules = new List<IPlayerCreationRule>();
        protected readonly List<IScoreRule> _ScoreRules = new List<IScoreRule>();

        protected AbstractExpansionPack()
        {
            PlaceRules = new ReadOnlyCollection<IPlaceRule<Tile, CarcassonneMove>>(_PlaceRules);
            ClaimRules = new ReadOnlyCollection<IClaimRule>(_ClaimRules);
            PlayerCreationRules = new ReadOnlyCollection<IPlayerCreationRule>(_PlayerRules);
            ScoreRules = new ReadOnlyCollection<IScoreRule>(_ScoreRules);
        }

        public virtual bool IgnoreDefaultStart => false;
        public virtual void BeforeDeckShuffle(Deck deck) { }
        public virtual void AfterDeckShuffle(Deck deck) { }
        public readonly ReadOnlyCollection<IPlaceRule<Tile, CarcassonneMove>> PlaceRules;
        public readonly ReadOnlyCollection<IClaimRule> ClaimRules;
        public readonly ReadOnlyCollection<IPlayerCreationRule> PlayerCreationRules;
        public readonly ReadOnlyCollection<IScoreRule> ScoreRules;
    }
}
