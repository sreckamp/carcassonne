using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassonne.Model
{
    /// <summary>
    /// Area on a tile of a specific type
    /// </summary>
    public class EdgeRegion : IEdgeRegion, IClaimable
    {
        public EdgeRegion(EdgeRegionType type, params EdgeDirection[] edges)
            :this(type, (IEnumerable<EdgeDirection>)edges)
        {
        }

        protected EdgeRegion(EdgeRegionType type, IEnumerable<EdgeDirection> edges)
        {
            Type = type;
            Edges = new List<EdgeDirection>(edges).AsReadOnly();
        }

        #region IClaimable Members

        private IMeeple m_claimer = NopMeeple.Instance;
        public IMeeple Claimer
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
            Claimer = NopMeeple.Instance;
        }

        public void Claim(IMeeple meeple)
        {
            if (Claimer.Type != MeepleType.None && meeple.Type != MeepleType.None)
            {
                throw new InvalidOperationException("Cannot Claim a region already claimed.");
            }
            Claimer = meeple;
        }

        /// <inheritdoc />
        public bool IsAvailable => !Container.IsClosed;

        #endregion

        public IPointContainer Container { get; set; } = NopPointContainer.Instance;

        //This never changes.
        public ITile Parent { get; set; } = NopTile.Instance;

        //This never changes.
        public EdgeRegionType Type { get; }

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

        public virtual IEdgeRegion Duplicate(ITile parent)
        {
            return new EdgeRegion(Type, Edges)
            {
                Parent = parent
            };
        }
    }

    public class CityEdgeRegion:EdgeRegion, ICityEdgeRegion
    {
        public CityEdgeRegion(params EdgeDirection[] edges)
            : base(EdgeRegionType.City, edges) { }
        private CityEdgeRegion(IEnumerable<EdgeDirection> edges)
            : base(EdgeRegionType.City, edges) { }

        public bool HasShield { get; internal set; }

        public override IEdgeRegion Duplicate(ITile parent)
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
