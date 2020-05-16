using System.Collections.Generic;
using System.Collections.ObjectModel;
using Carcassonne.Model.Rules;
using GameBase.Model.Rules;

namespace Carcassonne.Model
{
    public abstract class AbstractExpansionPack
    {
        protected readonly List<IPlaceRule<Tile, CarcassonneMove>> WritablePlaceRules = new List<IPlaceRule<Tile, CarcassonneMove>>();
        protected readonly List<IClaimRule> WritableClaimRules = new List<IClaimRule>();
        protected readonly List<IPlayerCreationRule> WritablePlayerRules = new List<IPlayerCreationRule>();
        protected readonly List<IScoreRule> WritableScoreRules = new List<IScoreRule>();

        protected AbstractExpansionPack()
        {
            PlaceRules = new ReadOnlyCollection<IPlaceRule<Tile, CarcassonneMove>>(WritablePlaceRules);
            ClaimRules = new ReadOnlyCollection<IClaimRule>(WritableClaimRules);
            PlayerCreationRules = new ReadOnlyCollection<IPlayerCreationRule>(WritablePlayerRules);
            ScoreRules = new ReadOnlyCollection<IScoreRule>(WritableScoreRules);
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
