using System.Collections.Generic;
using System.Linq;

namespace Carcassonne.Model
{
    public class RotatedEdgeRegion : IEdgeRegion, IClaimable
    {
        public RotatedEdgeRegion(IEdgeRegion region, Rotation rot)
        {
            m_region = region;
            m_claimable = region is IClaimable c ? c : DefaultClaimable.Instance;
            Rotation = rot;
        }

        private readonly IEdgeRegion m_region;
        private readonly IClaimable m_claimable;
        public Rotation Rotation { get; }

        public void Claim(Meeple meeple) => m_claimable.Claim(meeple);

        public void ResetClaim() => m_claimable.ResetClaim();

        public Meeple Claimer => m_claimable.Claimer;
        public bool IsClosed => m_claimable.IsClosed;

        public IPointContainer Container
        {
            get => m_region.Container;
            set => m_region.Container = value;
        }

        public EdgeRegionType Type => m_region.Type;
        public ITile Parent
        {
            get => m_region.Parent;
            set => m_region.Parent = value;
        }

        public IList<EdgeDirection> Edges => m_region.Edges.Select(d => d.UnRotate(Rotation)).ToList();

        public IEdgeRegion Duplicate(ITile parent) => new RotatedEdgeRegion(m_region.Duplicate(parent), Rotation);
    }
}