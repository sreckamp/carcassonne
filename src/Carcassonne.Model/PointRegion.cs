using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Carcassonne.Model
{
    public class PointRegion : IPointRegion
    {
        private readonly List<EdgeRegion> m_regions = new List<EdgeRegion>();
        private readonly List<Tile> m_tiles = new List<Tile>();
        private readonly RegionType m_type;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public PointRegion(RegionType type)
        {
            m_type = type;
        }

        public event EventHandler IsClosedChanged;
        public RegionType Type { get; private set; }

        private void OnIsClosedChange()
        {
            NotifyPropertyChanged("IsClosed");
            IsClosedChanged?.Invoke(this, new EventArgs());
        }

        private bool m_isForcedOpened = false;
        public bool IsForcedOpened
        {
            get => m_isForcedOpened;
            set
            {
                m_isForcedOpened = value;
                OnIsClosedChange();
            }
        }
        public bool IsClosed => !IsForcedOpened && (OpenEdges == 0);
        private int m_edges = 0;
        public int OpenEdges
        {
            get => m_edges;
            set
            {
                m_edges = value;
                NotifyPropertyChanged("OpenEdges");
                OnIsClosedChange();
            }
        }

        public int TileCount => m_tiles.Count;

        public void ReturnMeeple()
        {
            var markers = new List<Player>();
            foreach (var r in m_regions)
            {
                if (r.Claimer != null)
                {
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
        }

        private readonly List<Player> m_owners = new List<Player>();
        public List<Player> Owners => m_owners;

        public void Add(EdgeRegion r)
        {
            var updated = UpdateEdges(r);
            if (updated)
            {
                NotifyPropertyChanged("Score");
            }
        }

        protected virtual bool UpdateEdges(EdgeRegion r)
        {
            var updated = false;
            if (!m_regions.Contains(r))
            {
                var openEdges = OpenEdges + r.Edges.Length;
                if (m_regions.Count > 0)
                {
                    openEdges -= 2;
                }
                m_regions.Add(r);
                OpenEdges = openEdges;
                r.Container = this;
                UpdateOwners();
            }
            if (!m_tiles.Contains(r.Parent))
            {
                m_tiles.Add(r.Parent);
                updated = true;
            }
            return updated;
        }

        public void UpdateOwners()
        {
            var claims = new Dictionary<Player, int>();
            var high = 0;
            foreach (var r in m_regions)
            {
                if (r.Claimer != null)
                {
                    if (!claims.ContainsKey(r.Claimer.Player))
                    {
                        claims[r.Claimer.Player] = 0;
                    }
                    claims[r.Claimer.Player]++;
                    if (claims[r.Claimer.Player] >= high)
                    {
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
            }
            NotifyPropertyChanged("Owners");
        }

        public void Merge(PointRegion other)
        {
            foreach (var er in other.m_regions)
            {
                Add(er);
            }
        }

        public List<EdgeRegion> GetClaimedRegions()
        {
            var claimed = new List<EdgeRegion>();
            foreach (var r in m_regions)
            {
                if (r.Claimer != null)
                {
                    claimed.Add(r);
                }
            }
            return claimed;
        }

        public override string ToString()
        {
            return $"{GetType().Name} {m_tiles.Count}";
        }
    }
}
