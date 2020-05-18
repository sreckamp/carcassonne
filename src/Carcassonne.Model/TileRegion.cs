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

        private Meeple m_claimer = Meeple.None;
        public Meeple Claimer
        {
            get => m_claimer;
            private set
            {
                m_claimer = value;
                Owners.Clear();
                if (m_claimer != Meeple.None)
                {
                    Owners.Add(m_claimer.Player);
                }
            }
        }

        public List<Player> Owners { get; } = new List<Player>();

        /// <inheritdoc />
        public void UpdateOwners()
        {
        }

        public void Claim(Meeple meeple)
        {
            if (Claimer != Meeple.None)
            {
                throw new InvalidOperationException("Cannot Claim a region already claimed.");
            }
            Claimer = meeple;
        }

        public void ResetClaim()
        {
            Claimer = Meeple.None;
        }

        public bool IsForcedOpened { get; set; } = false;

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
