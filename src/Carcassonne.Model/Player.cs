using System;
using System.Collections.Generic;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class Player
    {
        public static readonly Player None = new Player(string.Empty);

        public Player(string name)
        {
            Name = name;
            ScoreChanged += (sender, args) => { };
            Meeple = new MeepleCollection();
        }

        public string Name { get; }
        public IMoveProvider MoveChooser { get; set; } = EmptyMoveProvider.Instance;
        public IClaimProvider ClaimChooser { get; set; } = EmptyClaimProvider.Instance;
        public MeepleCollection Meeple { get; }

        public event EventHandler<ChangedValueArgs<int>> ScoreChanged;
        private int m_score;
        public int Score {
            get => m_score;
            internal set
            {
                var old = m_score;
                m_score = value;
                ScoreChanged.Invoke(this, new ChangedValueArgs<int>(old, value));
            }
        }

        public List<MeepleType> AvailableMeepleTypes => Meeple.AvailableTypes;

        public void CreateMeeple(int count, MeepleType type)
        {
            for (var i = 0; i < count; i++)
            {
                Meeple.Push(new Meeple(type, this));
            }
        }

        public Meeple PeekMeeple(MeepleType type)
        {
            return Meeple.Peek(type);
        }

        public Meeple GetMeeple(MeepleType type)
        {
            return Meeple.Pop(type);
        }

        public void ReturnMeeple(Meeple meeple)
        {
            Meeple.Push(meeple);
        }

        public virtual CarcassonneMove GetMove(Game game) {
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
