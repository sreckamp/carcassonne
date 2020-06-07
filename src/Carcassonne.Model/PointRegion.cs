using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassonne.Model
{
    public class PointRegion : IPointContainer
    {
        private readonly List<IEdgeRegion> m_regions = new List<IEdgeRegion>();
        private readonly List<ITile> m_tiles = new List<ITile>();

        public PointRegion(EdgeRegionType type)
        {
            Type = type;
            IsClosedChanged += (sender, args) => { };
        }

        public event EventHandler IsClosedChanged;
        public EdgeRegionType Type { get; }

        private void OnIsClosedChange()
        {
            IsClosedChanged.Invoke(this, new EventArgs());
        }

        public bool IsClosed => OpenEdges == 0;

        private int m_edges;
        public int OpenEdges
        {
            get => m_edges;
            set
            {
                m_edges = value;
                OnIsClosedChange();
            }
        }

        public int TileCount => m_tiles.Count;

        public void ReturnMeeple()
        {
            var markers = new List<IPlayer>();
            foreach (var r in m_regions)
            {
                if (!(r is IClaimable c) || c.Claimer.Type == MeepleType.None) continue;
                var p = c.Claimer.Player;
                p.ReturnMeeple(c.Claimer);
                if (Owners.Contains(p) && !markers.Contains(p))
                {
                    markers.Add(p);
                }
                else
                {
                    c.ResetClaim();
                }
            }
        }

        private readonly HashSet<IPlayer> m_owners = new HashSet<IPlayer>();
        public IEnumerable<IPlayer> Owners => m_owners;

        public void Add(IEdgeRegion r)
        {
            UpdateEdges(r);
        }

        protected virtual bool UpdateEdges(IEdgeRegion r)
        {
            if (!m_regions.Contains(r))
            {
                var openEdges = OpenEdges + r.Edges.Count;
                if (m_regions.Count > 0)
                {
                    openEdges -= 2;
                }
                m_regions.Add(r);
                OpenEdges = openEdges;
                r.Container = this;
                UpdateOwners();
            }

            if (m_tiles.Contains(r.Parent)) return false;
            m_tiles.Add(r.Parent);
            return true;
        }

        public void UpdateOwners()
        {
            var claims = new Dictionary<IPlayer, int>();
            var high = 0;
            foreach (var claim in m_regions.Where(r => r is IClaimable c && c.Claimer.Type != MeepleType.None)
                .Cast<IClaimable>())
            {
                if (!claims.ContainsKey(claim.Claimer.Player))
                {
                    claims[claim.Claimer.Player] = 0;
                }
                claims[claim.Claimer.Player]++;
                if (claims[claim.Claimer.Player] < high) continue;
                if (claims[claim.Claimer.Player] > high)
                {
                    m_owners.Clear();
                }
                m_owners.Add(claim.Claimer.Player);

                high = claims[claim.Claimer.Player];
            }
        }

        public void Merge(PointRegion other)
        {
            foreach (var er in other.m_regions)
            {
                Add(er);
            }
        }

        public List<IEdgeRegion> GetClaimedRegions() => m_regions.Where(r => r is IClaimable c && c.Claimer.Type != MeepleType.None).ToList();

        public override string ToString() => $"{GetType().Name} {m_tiles.Count}";
    }
}
