using System;
using System.Collections.Generic;

namespace Carcassonne.Model
{
    public class TileRegion : ITileRegion, IClaimable, IPointContainer
    {
        private readonly List<ITile> m_tiles = new List<ITile>();

        public TileRegion(TileRegionType type = TileRegionType.None)
        {
            Type = type;
        }

        public void Add(ITile t)
        {
            if (m_tiles.Contains(t)) return;
            m_tiles.Add(t);
            if (TileCount == 9)
            {
                //TODO: Score Changed
            }
        }

        public TileRegionType Type { get; }

        public int TileCount => m_tiles.Count;

        private IMeeple m_claimer = NopMeeple.Instance;

        public IMeeple Claimer
        {
            get => m_claimer;
            private set
            {
                m_claimer = value;
                m_owners.Clear();
                m_owners.Add(m_claimer.Player);
            }
        }

        private readonly HashSet<IPlayer> m_owners = new HashSet<IPlayer>();
        public IEnumerable<IPlayer> Owners => m_owners;

        /// <inheritdoc />
        public void UpdateOwners()
        {
        }

        public void Claim(IMeeple meeple)
        {
            if (Claimer.Type != MeepleType.None)
            {
                throw new InvalidOperationException("Cannot Claim a region already claimed.");
            }
            Claimer = meeple;
        }

        public void ResetClaim()
        {
            Claimer = NopMeeple.Instance;
        }

        public bool IsForcedOpened { get; set; } = false;

        public bool IsAvailable => !IsClosed;

        public bool IsClosed => !IsForcedOpened && TileCount == 9;

        public void ReturnMeeple()
        {
            Claimer.Player.ReturnMeeple(Claimer);
        }
        /// <inheritdoc />
        public ITileRegion Duplicate() => new TileRegion(Type);

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
