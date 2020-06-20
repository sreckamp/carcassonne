using System;
using System.Collections.Generic;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class Player : IPlayer
    {
        public Player(string name, Color color)
        {
            Name = name;
            Color = color;
            ScoreChanged += (sender, args) => { };
            Meeple = new MeepleCollection();
        }

        public string Name { get; }
        public Color Color { get; }
        public IMoveProvider MoveChooser { get; set; } = EmptyMoveProvider.Instance;
        public IClaimProvider ClaimChooser { get; set; } = EmptyClaimProvider.Instance;
        public MeepleCollection Meeple { get; }

        public void Reset()
        {
            Meeple.Clear();
            Score = 0;
        }

        public event EventHandler<ChangedValueArgs<int>> ScoreChanged;
        private int m_score;
        public int Score {
            get => m_score;
            set
            {
                var old = m_score;
                m_score = value;
                ScoreChanged.Invoke(this, new ChangedValueArgs<int>(old, value));
            }
        }

        public List<MeepleType> AvailableMeepleTypes => Meeple.AvailableTypes;

        public IMeeple PeekMeeple(MeepleType type)
        {
            return Meeple.Peek(type);
        }

        public IMeeple GetMeeple(MeepleType type)
        {
            return Meeple.Pop(type);
        }

        public void ReturnMeeple(IMeeple meeple)
        {
            if(meeple.Player != this) throw new InvalidOperationException("Meeple must be owned by this player.");
            Meeple.Push(meeple);
        }

        public virtual Move GetMove(Game game) {
            return MoveChooser.GetMove(game);
        }
        public virtual (IClaimable, MeepleType) GetClaim(Game game)
        {
            return ClaimChooser.GetClaim(game);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
