using System.Collections.Generic;
using System.Linq;

namespace Carcassonne.Model
{
    public class RotatedEdgeRegion : IEdgeRegion
    {
        public RotatedEdgeRegion(IEdgeRegion region, Rotation rot)
        {
            m_region = region;
            Rotation = rot;
        }

        private readonly IEdgeRegion m_region;
        public Rotation Rotation { get; set; }

        public void Claim(Meeple meeple) => m_region.Claim(meeple);

        public void ResetClaim() => m_region.ResetClaim();

        public Meeple Claimer => m_region.Claimer;
        public bool IsClosed => m_region.IsClosed;

        public PointRegion Container
        {
            get => m_region.Container;
            set => m_region.Container = value;
        }

        public RegionType Type => m_region.Type;
        public Tile Parent
        {
            get => m_region.Parent;
            set => m_region.Parent = value;
        }

        public IList<EdgeDirection> Edges => m_region.Edges.Select(d => d.UnRotate(Rotation)).ToList();

        public IEdgeRegion Duplicate(Tile parent) => new RotatedEdgeRegion(m_region.Duplicate(parent), Rotation);
    }
}