using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model
{
    public enum EdgeDirection
    {
        North,
        East,
        South,
        West,
    }

    public static class EdgeDirectionExtensions
    {
        public static int ToInt(this EdgeDirection dir)
        {
            return (int)dir;
        }

        public static EdgeDirection Rotate(this EdgeDirection dir, Rotation rot)
        {
            int newDir = dir.ToInt() - rot.ToInt();
            if (newDir < 0) newDir += 4;
            return (EdgeDirection)newDir;
        }

        public static EdgeDirection Opposite(this EdgeDirection dir)
        {
            int newDir = (dir.ToInt() + 2) % 4;
            return (EdgeDirection)newDir;
        }

        public static EdgeDirection UnRotate(this EdgeDirection dir, Rotation rot)
        {
            return (EdgeDirection)((rot.ToInt() + dir.ToInt()) % 4);
        }
    }
}
