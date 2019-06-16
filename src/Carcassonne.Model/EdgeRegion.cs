using System;
using System.Collections.Generic;
using System.Linq;
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
            m_rotatedEdges = new Dictionary<Rotation, List<EdgeDirection>>();
        private readonly EventHandler m_container_IsClosedChangedHandler;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public EdgeRegion(Tile tile, RegionType type, params EdgeDirection[] edges)
        {
            Type = type;
            Parent = tile;
            m_rotatedEdges[Rotation.None] = new List<EdgeDirection>();
            m_rotatedEdges[Rotation.CounterClockwise] = new List<EdgeDirection>();
            m_rotatedEdges[Rotation.UpsideDown] = new List<EdgeDirection>();
            m_rotatedEdges[Rotation.Clockwise] = new List<EdgeDirection>();
            foreach (var e in edges)
            {
                addEdge(e);
            }
            m_container_IsClosedChangedHandler = new EventHandler(container_IsClosedChangedHandler);
        }

        private void container_IsClosedChangedHandler(object sender, EventArgs e)
        {
            notifyPropertyChanged("IsClosed");
        }

        private Meeple m_claimer;
        public Meeple Claimer
        {
            get { return m_claimer; }
            private set
            {
                m_claimer = value;
                Container?.UpdateOwners();
                notifyPropertyChanged("Claimer");
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

        public bool IsClosed
        {
            get
            {
                return (Container?.IsClosed ?? false);
            }
        }

        private PointRegion m_container;
        public PointRegion Container
        {
            get { return m_container; }
            set
            {
                if (m_container != null)
                {
                    m_container.IsClosedChanged -= m_container_IsClosedChangedHandler;
                }
                m_container = value;
                if (m_container != null)
                {
                    m_container.IsClosedChanged += m_container_IsClosedChangedHandler;
                }
                notifyPropertyChanged("Container");
                container_IsClosedChangedHandler(value, null);
            }
        }

        //This never changes.
        public Tile Parent { get; private set; }

        //This never changes.
        public RegionType Type { get; private set; }

        private List<EdgeDirection> edges
        {
            get
            {
                //if (Parent.Placement != null)
                //{
                //    return m_rotatedEdges[(Parent.Placement as CarcassonneMove).Rotation];
                //}
                return m_rotatedEdges[Rotation.None];
            }
        }

        public EdgeDirection[] Edges
        {
            get
            {
                return edges.ToArray();
            }
        }

        public EdgeDirection[] RawEdges
        {
            get
            {
                return m_rotatedEdges[Rotation.None].ToArray();
            }
        }

        private void addEdge(EdgeDirection edge)
        {
            m_rotatedEdges[Rotation.None].Add(edge);
            m_rotatedEdges[Rotation.Clockwise].Add(edge.Rotate(Rotation.Clockwise));
            m_rotatedEdges[Rotation.UpsideDown].Add(edge.Rotate(Rotation.UpsideDown));
            m_rotatedEdges[Rotation.CounterClockwise].Add(edge.Rotate(Rotation.CounterClockwise));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var e in m_rotatedEdges[Rotation.None])
            {
                sb.Append(e.ToString().Substring(0, 1));
            }
            return string.Format("{0} {1}", Type, sb.ToString());
        }

        public bool ContainsDirection(EdgeDirection dir)
        {
            return edges.Contains(dir);
        }

        internal virtual EdgeRegion CopyTo(Tile tile)
        {
            return new EdgeRegion(tile, Type, m_rotatedEdges[Rotation.None].ToArray());
        }
    }

    public class CityEdgeRegion:EdgeRegion
    {
        public CityEdgeRegion(Tile tile, params EdgeDirection[] edges)
            : base(tile, RegionType.City, edges) { }
        public bool HasShield { get; set; }

        internal override EdgeRegion CopyTo(Tile tile)
        {
            var cer = new CityEdgeRegion(tile, m_rotatedEdges[Rotation.None].ToArray())
            {
                HasShield = HasShield
            };
            return cer;
        }

        public override string ToString()
        {
            string temp = string.Empty;
            if (HasShield)
            {
                temp += "Shielded ";
            }
            return temp + base.ToString();
        }
    }
}
