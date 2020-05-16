using System;
using System.Drawing;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class CarcassonneMove : Move, IEquatable<CarcassonneMove>
    {
        public new static readonly CarcassonneMove None = new CarcassonneMove(-1, -1, Rotation.None) {IsEmpty = true};

        public CarcassonneMove(int x, int y, Rotation rotation)
            : this(new Point(x, y), rotation)
        {
        }

        public CarcassonneMove(Point location, Rotation rotation)
            : base(location)
        {
            Rotation = rotation;
        }

        public readonly Rotation Rotation;

        public override bool Equals(object other)
        {
            return Equals(other as CarcassonneMove ?? None);
        }

        public override int GetHashCode()
        {
            return Rotation.GetHashCode() ^
                   base.GetHashCode();
        }

        #region IEquatable<Move> Members

        public bool Equals(CarcassonneMove other)
        {
            return base.Equals(other)
                   && Rotation.Equals(other?.Rotation);
        }

        #endregion
        
        public override string ToString()
        {
            return base.ToString() + $"-{Rotation}";
        }
    }
}
