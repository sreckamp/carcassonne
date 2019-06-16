using GameBase.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Carcassonne.Model
{
    public class CarcassonneMove : Move, IEquatable<CarcassonneMove>
    {
        public CarcassonneMove(int x, int y, Rotation rotation)
            : this(new Point(x, y), rotation)
        {
        }

        public CarcassonneMove(Point location, Rotation rotation)
            : base(location)
        {
            Location = location;
            Rotation = rotation;
        }

        public Rotation Rotation;

        public override bool Equals(object other)
        {
            return Equals(other as CarcassonneMove);
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
    }
}
