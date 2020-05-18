using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassonne.Model
{
    public class PointRegion : IPointContainer
    {
        public static readonly PointRegion None = new PointRegion(RegionType.Any);

        private readonly List<IEdgeRegion> m_regions = new List<IEdgeRegion>();
        private readonly List<Tile> m_tiles = new List<Tile>();

        public PointRegion(RegionType type)
        {
            Type = type;
            IsClosedChanged += (sender, args) => { };
        }

        public event EventHandler IsClosedChanged;
        public RegionType Type { get; }

        private void OnIsClosedChange()
        {
            IsClosedChanged.Invoke(this, new EventArgs());
        }

        private bool m_isForcedOpened;
        public bool IsForcedOpened
        {
            get => m_isForcedOpened;
            set
            {
                m_isForcedOpened = value;
                OnIsClosedChange();
            }
        }
        public bool IsClosed => !IsForcedOpened && OpenEdges == 0;

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
            var markers = new List<Player>();
            foreach (var r in m_regions)
            {
                if (r.Claimer == Meeple.None) continue;
                var p = r.Claimer.Player;
                p.ReturnMeeple(r.Claimer);
                if (Owners.Contains(p) && !markers.Contains(p))
                {
                    markers.Add(p);
                }
                else
                {
                    r.ResetClaim();
                }
            }
        }

        public List<Player> Owners { get; } = new List<Player>();

        public void Add(IEdgeRegion r)
        {
            UpdateEdges(r);
        }

        protected virtual bool UpdateEdges(IEdgeRegion r)
        {
            var updated = false;
            if (!m_regions.Contains(r))
            {
                var openEdges = OpenEdges + r.Edges.Count();
                if (m_regions.Count > 0)
                {
                    openEdges -= 2;
                }
                m_regions.Add(r);
                OpenEdges = openEdges;
                r.Container = this;
                UpdateOwners();
            }

            if (m_tiles.Contains(r.Parent)) return updated;
            m_tiles.Add(r.Parent);
            updated = true;
            return updated;
        }

        public void UpdateOwners()
        {
            var claims = new Dictionary<Player, int>();
            var high = 0;
            foreach (var r in m_regions.Where(r => r.Claimer != Meeple.None))
            {
                if (!claims.ContainsKey(r.Claimer.Player))
                {
                    claims[r.Claimer.Player] = 0;
                }
                claims[r.Claimer.Player]++;
                if (claims[r.Claimer.Player] < high) continue;
                if (claims[r.Claimer.Player] > high)
                {
                    Owners.Clear();
                }
                if (!Owners.Contains(r.Claimer.Player))
                {
                    Owners.Add(r.Claimer.Player);
                }
                high = claims[r.Claimer.Player];
            }
        }

        public void Merge(PointRegion other)
        {
            foreach (var er in other.m_regions)
            {
                Add(er);
            }
        }

        public List<IEdgeRegion> GetClaimedRegions() => m_regions.Where(r => r.Claimer != Meeple.None).ToList();

        public override string ToString() => $"{GetType().Name} {m_tiles.Count}";
    }
}
