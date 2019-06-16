using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
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
            get { return m_score; }
            internal set
            {
                var old = m_score;
                m_score = value;
                ChangedValueArgs<int>.Trigger(ScoreChanged, this, old, value);
            }
        }

        public List<MeepleType> AvailableMeepleTypes
        {
            get
            {
                return Meeple.AvailableTypes;
            }
        }

        public void CreateMeeple(int count, MeepleType type)
        {
            for (int i = 0; i < count; i++)
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
