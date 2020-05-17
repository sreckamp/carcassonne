using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class Tile : IPiece
    {
        public static readonly Tile None = new Tile();

        private readonly List<EdgeRegion> m_grassRegions = new List<EdgeRegion>();

        private Tile()
        {
        }

        private TileRegion m_tileRegion = TileRegion.None;

        public TileRegion TileRegion
        {
            get => m_tileRegion;
            private set
            {
                m_tileRegion = value;
                m_tileRegion.Add(this);
            }
        }

        private readonly IList<EdgeRegion> m_regions = new List<EdgeRegion>
        {
            new EdgeRegion(RegionType.None, EdgeDirection.North),
            new EdgeRegion(RegionType.None, EdgeDirection.East),
            new EdgeRegion(RegionType.None, EdgeDirection.South),
            new EdgeRegion(RegionType.None, EdgeDirection.West),
        };

        public IEnumerable<EdgeRegion> Regions => m_regions;

        public bool HasMonastery => TileRegion.Type == TileRegionType.Monastery;

        public bool HasFlower => TileRegion.Type == TileRegionType.Flower;

        public Meeple ClaimingMeeple => ClaimedRegion.Claimer;

        public IClaimable ClaimedRegion
        {
            get
            {
                if (TileRegion.Claimer != Meeple.None)
                {
                    return TileRegion;
                }

                return Regions.FirstOrDefault(r => r.Claimer != Meeple.None);
            }
        }

        public RegionType GetEdge(EdgeDirection direction)
        {
            return GetRegion(direction).Type;
        }

        private void AddRegion(EdgeRegion region)
        {
            foreach (var dir in region.Edges)
            {
                var r = GetRegion(dir);
                if (r.Type == RegionType.None)
                {
                    m_regions.Remove(r);
                }
                else
                {
                    throw new ArgumentException($"Edge {dir} is already in use.");
                }
            }

            m_regions.Add(region);
            //region.PropertyChanged += new PropertyChangedEventHandler(region_PropertyChanged);
        }

        public EdgeRegion GetRegion(EdgeDirection direction)
        {
            return Regions.First(er => er.ContainsDirection(direction));
        }

        public Tile TileClone()
        {
            return Clone() as Tile ?? None;
        }

        public IPiece Clone()
        {
            var tile = new Tile
            {
                TileRegion = new TileRegion(TileRegion.Type)
            };

            foreach (var r in Regions)
            {
                tile.AddRegion(r.CopyTo(tile));
            }

            return tile;
        }

        public override string ToString()
        {
            var done = new List<EdgeRegion>();
            var sb = new StringBuilder();
            foreach (var r in Regions.Where(r => r.Type != RegionType.None && !done.Contains(r)))
            {
                if (sb.Length > 0)
                {
                    sb.Append(',');
                }

                sb.Append(r);
                done.Add(r);
            }

            if (TileRegion == TileRegion.None) return sb.ToString();
            if (sb.Length > 0)
            {
                sb.Append(',');
            }

            sb.Append(TileRegion.Type);

            return sb.ToString();
        }

        public List<IClaimable> GetAvailableRegions(RegionType type)
        {
            return Regions.Where(r => r.Type == type).Cast<IClaimable>().ToList();
        }

        public List<IClaimable> GetAvailableRegions()
        {
            var claimableRegions = new List<IClaimable>();
            if (TileRegion.Claimer == Meeple.None)
            {
                claimableRegions.Add(TileRegion);
            }

            claimableRegions.AddRange(Regions.Where(r =>
                r.Type != RegionType.None && r.Container.Owners.Count == 0 && !claimableRegions.Contains(r)));

            return claimableRegions;
        }

        public List<IPointRegion> GetClosedRegions()
        {
            var closed = new List<IPointRegion>();
            if (TileRegion.IsClosed)
            {
                closed.Add(TileRegion);
            }

            closed.AddRange(Regions.Where(r => r.Container.IsClosed
                                               && !closed.Contains(r.Container)).Select(r=>r.Container));

            return closed;
        }

        public class TileBuilder
        {
            public TileBuilder()
            {
                Tile = new Tile();
            }

            public Tile Tile { get; private set; }

            public TileBuilder AddCityRegion(params EdgeDirection[] edges)
            {
                Tile.AddRegion(new CityEdgeRegion(edges) {Parent = Tile});
                return this;
            }

            public TileBuilder AddShield(EdgeDirection edge)
            {
                var region = Tile.GetRegion(edge) as CityEdgeRegion ??
                             throw new InvalidOperationException($"{edge} edge not part of a City region!");
                region.HasShield = true;
                return this;
            }

            public TileBuilder AddFlowers()
            {
                Tile.TileRegion = new TileRegion(TileRegionType.Flower);
                return this;
            }

            public TileBuilder AddMonastery()
            {
                Tile.TileRegion = new TileRegion(TileRegionType.Monastery);
                return this;
            }

            public TileBuilder AddRiverRegion(params EdgeDirection[] edges)
            {
                Tile.AddRegion(new EdgeRegion(RegionType.River, edges) {Parent = Tile});
                return this;
            }

            public TileBuilder AddRoadRegion(params EdgeDirection[] edges)
            {
                Tile.AddRegion(new EdgeRegion(RegionType.Road, edges) {Parent = Tile});
                return this;
            }

            public TileBuilder NewTile()
            {
                Tile = new Tile();
                return this;
            }

            public static implicit operator Tile(TileBuilder builder)
            {
                return builder.Tile;
            }
        }
    }
}