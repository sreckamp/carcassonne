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
            RotatedEdges = new Dictionary<Rotation, List<EdgeDirection>>();
        private readonly EventHandler m_containerIsClosedChangedHandler;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public EdgeRegion(RegionType type = RegionType.None, params EdgeDirection[] edges)
        {
            Type = type;
            PropertyChanged += (sender, args) => { };
            RotatedEdges[Rotation.None] = new List<EdgeDirection>();
            RotatedEdges[Rotation.CounterClockwise] = new List<EdgeDirection>();
            RotatedEdges[Rotation.UpsideDown] = new List<EdgeDirection>();
            RotatedEdges[Rotation.Clockwise] = new List<EdgeDirection>();
            foreach (var e in edges)
            {
                AddEdge(e);
            }
            m_containerIsClosedChangedHandler = container_IsClosedChangedHandler;
        }

        private void container_IsClosedChangedHandler(object sender, EventArgs e)
        {
            NotifyPropertyChanged(nameof(IsClosed));
        }

        private Meeple m_claimer = Meeple.None;
        public Meeple Claimer
        {
            get => m_claimer;
            private set
            {
                m_claimer = value;
                Container.UpdateOwners();
                NotifyPropertyChanged(nameof(Claimer));
            }
        }

        public void ResetClaim()
        {
            Claimer = Meeple.None;
        }

        public void Claim(Meeple meeple)
        {
            if (Claimer != Meeple.None && meeple != Meeple.None)
            {
                throw new InvalidOperationException("Cannot Claim a region already claimed.");
            }
            Claimer = meeple;
        }

        public bool IsClosed => (Container?.IsClosed ?? false);

        private PointRegion m_container = PointRegion.None;
        public PointRegion Container
        {
            get => m_container;
            set
            {
                m_container.IsClosedChanged -= m_containerIsClosedChangedHandler;
                m_container = value;
                m_container.IsClosedChanged += m_containerIsClosedChangedHandler;
                NotifyPropertyChanged(nameof(Container));
                container_IsClosedChangedHandler(value, new EventArgs());
            }
        }

        //This never changes.
        public Tile Parent { get; internal set; } = Tile.None;

        //This never changes.
        public RegionType Type { get; }

        private List<EdgeDirection> EdgesList => RotatedEdges[Rotation.None];

        public EdgeDirection[] Edges => EdgesList.ToArray();

        public EdgeDirection[] RawEdges => RotatedEdges[Rotation.None].ToArray();

        private void AddEdge(EdgeDirection edge)
        {
            RotatedEdges[Rotation.None].Add(edge);
            RotatedEdges[Rotation.Clockwise].Add(edge.Rotate(Rotation.Clockwise));
            RotatedEdges[Rotation.UpsideDown].Add(edge.Rotate(Rotation.UpsideDown));
            RotatedEdges[Rotation.CounterClockwise].Add(edge.Rotate(Rotation.CounterClockwise));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var e in RotatedEdges[Rotation.None])
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
            return new EdgeRegion(Type, RotatedEdges[Rotation.None].ToArray())
            {
                Parent = tile
            };
        }
    }

    public class CityEdgeRegion:EdgeRegion
    {
        public CityEdgeRegion(params EdgeDirection[] edges)
            : base(RegionType.City, edges) { }
        public bool HasShield { get; set; }

        internal override EdgeRegion CopyTo(Tile tile)
        {
            var cer = new CityEdgeRegion(RotatedEdges[Rotation.None].ToArray())
            {
                Parent = tile,
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
