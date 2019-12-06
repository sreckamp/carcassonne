namespace Carcassonne.Model
{
    public enum RegionType
    {
        Grass,
        Road,
        City,
        River,
    }

    public static class RegionTypeExtensions
    {
        public static bool IsPath(this RegionType type)
        {
            return type == RegionType.Road || type == RegionType.River;
        }

        public static bool IsPath(this RegionType? type)
        {
            return type == RegionType.Road || type == RegionType.River;
        }
    }
}
