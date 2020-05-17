namespace Carcassonne.Model
{
    public enum Rotation
    {
        None,
        CounterClockwise,
        UpsideDown,
        Clockwise,
    }

    public static class RotationExtensions
    {
        public static int ToInt(this Rotation rotation)
        {
            return (int)rotation;
        }

        public static float ToDegrees(this Rotation rot)
        {
            return rot switch
            {
                Rotation.Clockwise => 90f,
                Rotation.UpsideDown => 180f,
                Rotation.CounterClockwise => 270f,
                _ => 0f
            };
        }

        public static Rotation RotateCw(this Rotation rot)
        {
            var newRotation = rot.ToInt() - 1;
            if (newRotation < 0)
            {
                newRotation = (int)Rotation.Clockwise;
            }
            return (Rotation)newRotation;
        }

        public static Rotation RotateCcw(this Rotation rot)
        {
            var newRotation = (rot.ToInt() + 1) % 4;
            return (Rotation)newRotation;
        }
    }
}
