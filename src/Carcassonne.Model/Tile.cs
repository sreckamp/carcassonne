using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class Tile : Piece
    {
        private readonly List<EdgeRegion> m_grassRegions = new List<EdgeRegion>();

        public Tile()
        {
            Regions = new List<EdgeRegion>();
        }

        private TileRegion m_tileRegion;
        public TileRegion TileRegion
        {
            get { return m_tileRegion; }
            internal set
            {
                m_tileRegion = value;
                //m_tileRegion.PropertyChanged += new PropertyChangedEventHandler(region_PropertyChanged);
                //OnPropertyChanged("TileRegion");
            }
        }

        public List<EdgeRegion> Regions { get; private set; }

        public bool HasMonestary { get { return TileRegion?.Type == TileRegionType.Monestary; } }

        public bool HasFlower { get { return TileRegion?.Type == TileRegionType.Flower; } }

        public Meeple ClaimingMeeple { get { return ClaimedRegion?.Claimer; } }

        public IClaimable ClaimedRegion
        {
            get
            {
                if (TileRegion?.Claimer != null)
                {
                    return TileRegion;
                }
                foreach (var r in Regions)
                {
                    if (r.Claimer != null)
                    {
                        return r;
                    }
                }
                return null;
            }
        }

        public RegionType GetEdge(EdgeDirection direction)
        {
            return GetRegion(direction)?.Type ?? RegionType.Grass;
        }

        private void addRegion(EdgeRegion region)
        {
            foreach (var dir in region.Edges)
            {
                if (GetRegion(dir) != null)
                {
                    throw new ArgumentException("Edge " + dir.ToString() + " is already in use.");
                }
            }
            Regions.Add(region);
            //region.PropertyChanged += new PropertyChangedEventHandler(region_PropertyChanged);
        }

        public EdgeRegion GetRegion(EdgeDirection direction)
        {
            foreach (var er in Regions)
            {
                if (er.ContainsDirection(direction))
                {
                    return er;
                }
            }
            return null;
        }

        public Tile TileClone()
        {
            return Clone() as Tile;
        }

        public override Piece Clone()
        {
            var tile = new Tile();
            foreach (var r in Regions)
            {
                if (r != null)
                {
                    tile.addRegion(r.CopyTo(tile));
                }
            }
            if (TileRegion != null)
            {
                tile.TileRegion = new TileRegion(tile, TileRegion.Type);
            }
            return tile;
        }

        public override string ToString()
        {
            var done = new List<EdgeRegion>();
            var sb = new StringBuilder();
            foreach (var r in Regions)
            {
                if (r != null && !done.Contains(r))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(',');
                    }
                    sb.Append(r.ToString());
                    done.Add(r);
                }
            }
            if (TileRegion != null)
            {
                if (sb.Length > 0)
                {
                    sb.Append(',');
                }
                sb.Append(TileRegion.Type.ToString());
            }
            return sb.ToString();
        }

        public List<IClaimable> GetAvailableRegions(RegionType type)
        {
            var claimables = new List<IClaimable>();
            foreach (var r in Regions)
            {
                if (r?.Type == type)
                {
                    claimables.Add(r);
                }
            }
            return claimables;
        }

        public List<IClaimable> GetAvailableRegions()
        {
            var claimables = new List<IClaimable>();
            if (TileRegion?.Claimer == null)
            {
                claimables.Add(TileRegion);
            }
            foreach (var r in Regions)
            {
                if (r?.Container?.Owners.Count == 0 && !claimables.Contains(r))
                {
                    claimables.Add(r);
                }
            }
            return claimables;
        }

        public List<IPointRegion> GetClosedRegions()
        {
            var closed = new List<IPointRegion>();
            if (TileRegion?.IsClosed ?? false)
            {
                closed.Add(TileRegion);
            }
            foreach (var r in Regions)
            {
                if (r?.Container?.IsClosed ?? false
                    && !closed.Contains(r.Container))
                {
                    closed.Add(r.Container);
                }
            }
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
                Tile.addRegion(new CityEdgeRegion(Tile, edges));
                return this;
            }

            public TileBuilder AddShield(EdgeDirection edge)
            {
                var region = Tile.GetRegion(edge) as CityEdgeRegion ??
                    throw new InvalidOperationException(string.Format("{0} edge not part of a City region!", edge));
                region.HasShield = true;
                return this;
            }

            public TileBuilder AddFlowers()
            {
                Tile.TileRegion = new TileRegion(Tile, TileRegionType.Flower);
                return this;
            }

            public TileBuilder AddMonestary()
            {
                Tile.TileRegion = new TileRegion(Tile, TileRegionType.Monestary);
                return this;
            }

            public TileBuilder AddRiverRegion(params EdgeDirection[] edges)
            {
                Tile.addRegion(new EdgeRegion(Tile, RegionType.River, edges));
                return this;
            }

            public TileBuilder AddRoadRegion(params EdgeDirection[] edges)
            {
                Tile.addRegion(new EdgeRegion(Tile, RegionType.Road, edges));
                return this;
            }

            public TileBuilder NewTile()
            {
                Tile = new Tile();
                return this;
            }

            public static implicit operator Tile(TileBuilder builder)
            {
                return builder?.Tile;
            }
        }
    }
}