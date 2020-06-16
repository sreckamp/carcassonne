using System;

namespace Carcassonne.Model
{
    public enum EdgeDirection
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public static class EdgeDirectionExtensions
    {
        private static readonly int SCount = Enum.GetNames(typeof(EdgeDirection)).Length;
        private static readonly int SDirectionPerRotation = SCount/Enum.GetNames(typeof(Rotation)).Length;

        public static int ToInt(this EdgeDirection dir) => (int) dir;

        public static bool IsCardinal(this EdgeDirection dir) => (dir.ToInt() % 2) == 0;

        public static EdgeDirection Rotate(this EdgeDirection dir, Rotation rot)
        {
            var newDir = dir.ToInt() - (rot.ToInt() * SDirectionPerRotation);
            return (EdgeDirection)(newDir < 0 ? newDir + SCount : newDir);
        }

        public static EdgeDirection Opposite(this EdgeDirection dir) => dir.Rotate(Rotation.UpsideDown);

        public static EdgeDirection UnRotate(this EdgeDirection dir, Rotation rot) => (EdgeDirection)(((rot.ToInt() * SDirectionPerRotation) + dir.ToInt()) % SCount);
    }
}
