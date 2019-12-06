using System;
using System.Collections.Generic;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
            Meeple = new MeepleCollection();
        }

        public string Name { get; private set; }
        public IMoveProvider MoveChooser { get; set; }
        public IClaimProvider ClaimChooser { get; set; }
        public MeepleCollection Meeple { get; private set; }

        public event EventHandler<ChangedValueArgs<int>> ScoreChanged;
        private int m_score;
        public int Score {
            get => m_score;
            internal set
            {
                var old = m_score;
                m_score = value;
                ScoreChanged?.Invoke(this, new ChangedValueArgs<int>(old, value));
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
            return MoveChooser?.GetMove(game);
        }
        public virtual IClaimable GetClaim(Game game, out MeepleType type)
        {
            type = MeepleType.Meeple;
            return ClaimChooser?.GetClaim(game, out type);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
