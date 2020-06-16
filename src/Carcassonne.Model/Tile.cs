using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model
{
    public class Tile : ITile
    {
        protected Tile()
        {
        }

        private ITileRegion m_tileRegion = NopTileRegion.Instance;

        public ITileRegion TileRegion1
        {
            get => m_tileRegion;
            set
            {
                m_tileRegion = value;
                m_tileRegion.Add(this);
            }
        }

        private readonly IList<IEdgeRegion> m_regions = new List<IEdgeRegion>
        {
            new EdgeRegion(EdgeRegionType.Any, EdgeDirection.North),
            new EdgeRegion(EdgeRegionType.Any, EdgeDirection.East),
            new EdgeRegion(EdgeRegionType.Any, EdgeDirection.South),
            new EdgeRegion(EdgeRegionType.Any, EdgeDirection.West)
        };

        public IEnumerable<IEdgeRegion> Regions => m_regions;

        public bool HasMonastery { get; private set; } // return TileRegion.Type == TileRegionType.Monastery; }

        public bool HasFlowers { get; private set; } //=> TileRegion.Type == TileRegionType.Flower;

        public IEnumerable<IClaimable> ClaimedRegions
        {
            get
            {
                var claimed = m_regions.Where(r => r is IClaimable c && c.Claimer.Type != MeepleType.None).Cast<IClaimable>().ToList();
                if (TileRegion1 is IClaimable claim && claim.Claimer.Type != MeepleType.None)
                {
                    claimed.Add(claim);
                }

                return claimed;
            }
        }

        public EdgeRegionType GetEdge(EdgeDirection direction)
        {
            return GetRegion(direction).Type;
        }

        private void AddRegion(IEdgeRegion region)
        {
            foreach (var dir in region.Edges)
            {
                var r = GetRegion(dir);
                if (r.Type == EdgeRegionType.Any)
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

        public IEdgeRegion GetRegion(EdgeDirection direction)
        {
            return Regions.First(er => er.Edges.Contains(direction));
        }

        public Tile TileClone()
        {
            var tile = new Tile
            {
                TileRegion1 = TileRegion1.Duplicate(),
                HasFlowers = HasFlowers,
                HasMonastery = HasMonastery
            };
            
            foreach (var r in Regions)
            {
                tile.AddRegion(r.Duplicate(tile));
            }

            return tile;
        }

        public override string ToString()
        {
            var done = new List<IEdgeRegion>();
            var sb = new StringBuilder();
            foreach (var r in Regions.Where(r => r.Type != EdgeRegionType.None && !done.Contains(r)))
            {
                if (sb.Length > 0)
                {
                    sb.Append(',');
                }

                sb.Append(r);
                done.Add(r);
            }

            if (HasFlowers)
            {
                if (sb.Length > 0)
                {
                    sb.Append(',');
                }

                sb.Append("Flowers");
            }

            if (!HasMonastery) return sb.ToString();
            if (sb.Length > 0)
            {
                sb.Append(',');
            }

            sb.Append("Monastery");

            return sb.ToString();
        }

        public bool Contains(EdgeRegionType type)
        {
            return Regions.Where(r => r.Type == type).Cast<IClaimable>().Any();
        }

        public List<IClaimable> GetAvailableRegions()
        {
            var claimableRegions = Regions.Where(r =>
                r.Type.IsValid() && !r.Container.Owners.Any()).Distinct().Cast<IClaimable>().ToList();
            if (TileRegion1 is IClaimable c && c.Claimer.Type != MeepleType.None)
            {
                claimableRegions.Add(c);
            }

            return claimableRegions;
        }

        // public List<IPointContainer> GetClosedRegions()
        // {
        //     var closed = new List<IPointContainer>();
        //     if (TileRegion.IsClosed)
        //     {
        //         closed.Add(TileRegion);
        //     }
        //
        //     closed.AddRange(Regions.Where(r => r.Container.IsClosed
        //                                        && !closed.Contains(r.Container)).Select(r=>r.Container));
        //
        //     return closed;
        // }

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
                Tile.HasFlowers = true;
                return this;
            }

            public TileBuilder AddMonastery()
            {
                Tile.HasMonastery = true;
                return this;
            }

            public TileBuilder AddRiverRegion(params EdgeDirection[] edges)
            {
                Tile.AddRegion(new EdgeRegion(EdgeRegionType.River, edges) {Parent = Tile});
                return this;
            }

            public TileBuilder AddRoadRegion(params EdgeDirection[] edges)
            {
                Tile.AddRegion(new EdgeRegion(EdgeRegionType.Road, edges) {Parent = Tile});
                return this;
            }

            public TileBuilder NewTile()
            {
                Tile = new Tile();
                return this;
            }

            public static implicit operator Tile(TileBuilder builder)
            {
                foreach (EdgeDirection dir in Enum.GetValues(typeof(EdgeDirection)))
                {
                    if (builder.Tile.GetEdge(dir) == EdgeRegionType.Any)
                    {
                        builder.Tile.AddRegion(new EdgeRegion(EdgeRegionType.None, dir));
                    }
                }
                return builder.Tile;
            }
        }
    }
}