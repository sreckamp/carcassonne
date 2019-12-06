using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Carcassonne.Model
{
    /// <summary>
    /// Area on a tile of a specific type
    /// </summary>
    public class EdgeRegion : IClaimable //, INotifyPropertyChanged
    {
        protected readonly Dictionary<Rotation, List<EdgeDirection>>
            _RotatedEdges = new Dictionary<Rotation, List<EdgeDirection>>();
        private readonly EventHandler m_containerIsClosedChangedHandler;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public EdgeRegion(Tile tile, RegionType type, params EdgeDirection[] edges)
        {
            Type = type;
            Parent = tile;
            _RotatedEdges[Rotation.None] = new List<EdgeDirection>();
            _RotatedEdges[Rotation.CounterClockwise] = new List<EdgeDirection>();
            _RotatedEdges[Rotation.UpsideDown] = new List<EdgeDirection>();
            _RotatedEdges[Rotation.Clockwise] = new List<EdgeDirection>();
            foreach (var e in edges)
            {
                AddEdge(e);
            }
            m_containerIsClosedChangedHandler = container_IsClosedChangedHandler;
        }

        private void container_IsClosedChangedHandler(object sender, EventArgs e)
        {
            NotifyPropertyChanged("IsClosed");
        }

        private Meeple m_claimer;
        public Meeple Claimer
        {
            get => m_claimer;
            private set
            {
                m_claimer = value;
                Container?.UpdateOwners();
                NotifyPropertyChanged("Claimer");
            }
        }

        public void ResetClaim()
        {
            Claimer = null;
        }

        public void Claim(Meeple meeple)
        {
            if (Claimer != null)
            {
                throw new InvalidOperationException("Cannot Claim a region already claimed.");
            }
            Claimer = meeple;
        }

        public bool IsClosed => (Container?.IsClosed ?? false);

        private PointRegion m_container;
        public PointRegion Container
        {
            get => m_container;
            set
            {
                if (m_container != null)
                {
                    m_container.IsClosedChanged -= m_containerIsClosedChangedHandler;
                }
                m_container = value;
                if (m_container != null)
                {
                    m_container.IsClosedChanged += m_containerIsClosedChangedHandler;
                }
                NotifyPropertyChanged("Container");
                container_IsClosedChangedHandler(value, null);
            }
        }

        //This never changes.
        public Tile Parent { get; private set; }

        //This never changes.
        public RegionType Type { get; private set; }

        private List<EdgeDirection> EdgesList => _RotatedEdges[Rotation.None];

        public EdgeDirection[] Edges => EdgesList.ToArray();

        public EdgeDirection[] RawEdges => _RotatedEdges[Rotation.None].ToArray();

        private void AddEdge(EdgeDirection edge)
        {
            _RotatedEdges[Rotation.None].Add(edge);
            _RotatedEdges[Rotation.Clockwise].Add(edge.Rotate(Rotation.Clockwise));
            _RotatedEdges[Rotation.UpsideDown].Add(edge.Rotate(Rotation.UpsideDown));
            _RotatedEdges[Rotation.CounterClockwise].Add(edge.Rotate(Rotation.CounterClockwise));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var e in _RotatedEdges[Rotation.None])
            {
                sb.Append(e.ToString().Substring(0, 1));
            }
            return $"{Type} {sb}";
        }

        public bool ContainsDirection(EdgeDirection dir)
        {
            return EdgesList.Contains(dir);
        }

        internal virtual EdgeRegion CopyTo(Tile tile)
        {
            return new EdgeRegion(tile, Type, _RotatedEdges[Rotation.None].ToArray());
        }
    }

    public class CityEdgeRegion:EdgeRegion
    {
        public CityEdgeRegion(Tile tile, params EdgeDirection[] edges)
            : base(tile, RegionType.City, edges) { }
        public bool HasShield { get; set; }

        internal override EdgeRegion CopyTo(Tile tile)
        {
            var cer = new CityEdgeRegion(tile, _RotatedEdges[Rotation.None].ToArray())
            {
                HasShield = HasShield
            };
            return cer;
        }

        public override string ToString()
        {
            var temp = string.Empty;
            if (HasShield)
            {
                temp += "Shielded ";
            }
            return temp + base.ToString();
        }
    }
}
