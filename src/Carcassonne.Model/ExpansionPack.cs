using System.Collections.Generic;
using System.Collections.ObjectModel;
using Carcassonne.Model.Rules;
using GameBase.Model.Rules;

namespace Carcassonne.Model
{
    public abstract class ExpansionPack
    {
        protected readonly List<IPlaceRule<IBoard, ITile>> WritablePlaceRules = new List<IPlaceRule<IBoard, ITile>>();
        protected readonly List<IClaimRule> WritableClaimRules = new List<IClaimRule>();
        protected readonly List<IPlayerCreationRule> WritablePlayerRules = new List<IPlayerCreationRule>();
        protected readonly List<IScoreRule> WritableScoreRules = new List<IScoreRule>();
        protected readonly List<IJoinRule> WritableJoinRules = new List<IJoinRule>();

        protected ExpansionPack()
        {
            PlaceRules = new ReadOnlyCollection<IPlaceRule<IBoard, ITile>>(WritablePlaceRules);
            ClaimRules = new ReadOnlyCollection<IClaimRule>(WritableClaimRules);
            PlayerCreationRules = new ReadOnlyCollection<IPlayerCreationRule>(WritablePlayerRules);
            ScoreRules = new ReadOnlyCollection<IScoreRule>(WritableScoreRules);
            JoinRules = new ReadOnlyCollection<IJoinRule>(WritableJoinRules);
        }

        public virtual void BeforeDeckShuffle(Deck deck) { }
        public virtual void AfterDeckShuffle(Deck deck) { }
        public ReadOnlyCollection<IPlaceRule<IBoard, ITile>> PlaceRules { get; }
        public ReadOnlyCollection<IClaimRule> ClaimRules { get; }
        public ReadOnlyCollection<IPlayerCreationRule> PlayerCreationRules { get; }
        public ReadOnlyCollection<IScoreRule> ScoreRules { get; }
        public ReadOnlyCollection<IJoinRule> JoinRules { get; }
    }
}
