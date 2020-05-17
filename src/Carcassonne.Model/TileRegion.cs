using System;
using System.Collections.Generic;

namespace Carcassonne.Model
{
    public class TileRegion : IClaimable, IPointRegion //, INotifyPropertyChanged
    {
        public static readonly TileRegion None = new TileRegion();

        private readonly List<Tile> m_tiles = new List<Tile>();

        //#region INotifyPropertyChanged Members

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void notifyPropertyChanged(string name)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(name));
        //    }
        //}

        //#endregion

        public TileRegion(TileRegionType type = TileRegionType.None)
        {
            // PropertyChanged += (sender, args) => { };
            Type = type;
        }


        public TileRegionType Type { get; }

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

        public void Add(Tile t)
        {
            if (m_tiles.Contains(t)) return;
            m_tiles.Add(t);
            //notifyPropertyChanged("Score");
            if (Score == 9)
            {
                //notifyPropertyChanged("IsClosed");
            }
        }

        public bool IsForcedOpened { get; set; } = false;

        public bool IsClosed => !IsForcedOpened && Score == 9;
        public int Score => m_tiles.Count;

        public void ReturnMeeple()
        {
            Claimer.Player.ReturnMeeple(Claimer);
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
