using System;
using System.Drawing;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class CarcassonneMove // : IEquatable<CarcassonneMove>
    {
        public static readonly CarcassonneMove None = new CarcassonneMove(int.MinValue, int.MinValue, Rotation.None, true);

        public CarcassonneMove(int x, int y, Rotation rotation)
            : this(x, y, rotation, false)
        {
        }

        public CarcassonneMove(Point location, Rotation rotation) : this(location,rotation,false)
        {
        }

        private CarcassonneMove(int x, int y, Rotation rotation, bool isEmpty)
            : this(new Point(x, y), rotation, isEmpty)
        {
        }

        private CarcassonneMove(Point location, Rotation rotation, bool isEmpty)
        {
            Location = location;
            Rotation = rotation;
            IsEmpty = isEmpty;
        }

        public Point Location { get; }
        public Rotation Rotation { get; }

        public bool IsEmpty { get; }
        //
        // public override bool Equals(object other)
        // {
        //     return Equals(other as CarcassonneMove ?? None);
        // }
        
        // public override int GetHashCode()
        // {
        //     return Rotation.GetHashCode() ^
        //            base.GetHashCode();
        // }
        
        #region IEquatable<Move> Members
        
        public bool Equals(CarcassonneMove other)
        {
            return base.Equals(other);
            // && Rotation.Equals(other?.Rotation);
        }
        
        #endregion
        
        // public override string ToString()
        // {
        //     return base.ToString() + $"-{Rotation}";
        // }
    }
}
