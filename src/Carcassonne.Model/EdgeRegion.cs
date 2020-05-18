using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassonne.Model
{
    /// <summary>
    /// Area on a tile of a specific type
    /// </summary>
    public class EdgeRegion_ : IEdgeRegion
    {
        public EdgeRegion_(RegionType type, params EdgeDirection[] edges)
            :this(type, (IEnumerable<EdgeDirection>)edges)
        {
        }

        protected EdgeRegion_(RegionType type, IEnumerable<EdgeDirection> edges)
        {
            Type = type;
            Edges = new List<EdgeDirection>(edges).AsReadOnly();
        }

        #region IClaimable Members

        private Meeple m_claimer = Meeple.None;
        public Meeple Claimer
        {
            get => m_claimer;
            private set
            {
                m_claimer = value;
                Container.UpdateOwners();
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

        public bool IsClosed => Container.IsClosed;

        #endregion

        private PointRegion m_container = PointRegion.None;
        public PointRegion Container
        {
            get => m_container;
            set
            {
                m_container = value;
            }
        }

        //This never changes.
        public Tile Parent { get; set; } = Tile.None;

        //This never changes.
        public RegionType Type { get; }

        public IList<EdgeDirection> Edges { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var e in Edges)
            {
                sb.Append(e.ToString().Substring(0, 1));
            }
            return $"{Type} {sb}";
        }

        public virtual IEdgeRegion Duplicate(Tile parent)
        {
            return new EdgeRegion_(Type, Edges)
            {
                Parent = parent
            };
        }
    }

    public class CityEdgeRegion:EdgeRegion_
    {
        public CityEdgeRegion(params EdgeDirection[] edges)
            : base(RegionType.City, edges) { }
        private CityEdgeRegion(IEnumerable<EdgeDirection> edges)
            : base(RegionType.City, edges) { }

        public bool HasShield { get; set; }

        public override IEdgeRegion Duplicate(Tile parent)
        {
            var cer = new CityEdgeRegion(Edges)
            {
                Parent = parent,
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
