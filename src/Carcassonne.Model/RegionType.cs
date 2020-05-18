namespace Carcassonne.Model
{
    public enum EdgeRegionType
    {
        Any,
        None,
        Road,
        City,
        River,
    }

    public static class EdgeRegionTypeExtensions
    {
        public static bool IsPath(this EdgeRegionType type)
        {
            return type == EdgeRegionType.Road || type == EdgeRegionType.River;
        }
    }
}
