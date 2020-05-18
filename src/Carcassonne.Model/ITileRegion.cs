namespace Carcassonne.Model
{
    public interface ITileRegion
    {
        TileRegionType Type { get; }

        void Add(ITile tile);
        int TileCount { get; }

        ITileRegion Duplicate();
    }

    public class NopTileRegion : ITileRegion
    {
        public static readonly ITileRegion Instance = new NopTileRegion();

        private NopTileRegion()
        {
        }

        /// <inheritdoc />
        public TileRegionType Type => TileRegionType.None;

        /// <inheritdoc />
        public void Add(ITile tile) { }

        /// <inheritdoc />
        public int TileCount => 0;

        public ITileRegion Duplicate() => this;
    }
}
