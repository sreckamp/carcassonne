using System.Collections.Generic;
using System.Linq;

namespace Carcassonne.Model
{
    public class RotatedTile:ITile
    {
        public RotatedTile(ITile tile, Rotation rot)
        {
            RawTile = tile;
            Rotation = rot;
        }

        public ITile RawTile { get; }
        public Rotation Rotation { get; set; }

        public ITileRegion TileRegion
        {
            get { return RawTile.TileRegion; }
            set { RawTile.TileRegion = value; }
        }

        public bool HasFlowers => RawTile.HasFlowers;
        
        public bool HasMonastery => RawTile.HasMonastery;

        public IEnumerable<IEdgeRegion> Regions => RawTile.Regions.Select(r => new RotatedEdgeRegion(r, Rotation));

        public IEnumerable<IEdgeRegion> RegionsNotRotated => RawTile.Regions;

        public EdgeRegionType GetEdge(EdgeDirection direction)
        {
            direction = direction.UnRotate(Rotation);
            return RawTile.GetEdge(direction);
        }

        public IEdgeRegion GetRegion(EdgeDirection direction)
        {
            direction = direction.UnRotate(Rotation);
            return new RotatedEdgeRegion(RawTile.GetRegion(direction), Rotation);
        }

        public bool Contains(EdgeRegionType type)
        {
            return RawTile.Contains(type);
        }

        public override string ToString() => Rotation == Rotation.None ? RawTile.ToString() : $"{RawTile} @ {Rotation}";
    }
}