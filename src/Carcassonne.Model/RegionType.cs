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
        public static bool IsValid(this EdgeRegionType type) =>
            type != EdgeRegionType.Any && type != EdgeRegionType.None;

        public static bool FitsWith(this EdgeRegionType mine, EdgeRegionType theirs) =>
            mine == EdgeRegionType.Any || theirs == EdgeRegionType.Any || mine == theirs;

        public static bool IsPath(this EdgeRegionType type) =>
            type == EdgeRegionType.Road || type == EdgeRegionType.River;
    }
}
