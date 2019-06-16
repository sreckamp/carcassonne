using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Carcassonne.Model
{
    public class TileRegion : IClaimable, IPointRegion //, INotifyPropertyChanged
    {
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

        public TileRegion(Tile parent, TileRegionType type)
        {
            m_tiles.Add(parent);
            Type = type;
        }

        private TileRegionType m_type;
        public TileRegionType Type
        {
            get { return m_type; }
            private set
            {
                m_type = value;
                //notifyPropertyChanged("Type");
            }
        }

        private Meeple m_claimer;
        public Meeple Claimer
        {
            get { return m_claimer; }
            private set
            {
                m_claimer = value;
                //notifyPropertyChanged("Claimer");
                m_owners.Clear();
                if (m_claimer != null)
                {
                    m_owners.Add(m_claimer.Player);
                }
                //notifyPropertyChanged("Owner");
            }
        }

        private readonly List<Player> m_owners = new List<Player>();
        public List<Player> Owners
        {
            get { return m_owners; }
        }

        public void Claim(Meeple meeple)
        {
            if (Claimer != null)
            {
                throw new InvalidOperationException("Cannot Claim a region already claimed.");
            }
            Claimer = meeple;
        }

        public void ResetClaim()
        {
            Claimer = null;
        }

        public void Add(Tile t)
        {
            if (!m_tiles.Contains(t))
            {
                m_tiles.Add(t);
                //notifyPropertyChanged("Score");
                if (Score == 9)
                {
                    //notifyPropertyChanged("IsClosed");
                }
            }
        }

        private bool m_isForcedOpened = false;
        public bool IsForcedOpened {
            get { return m_isForcedOpened; }
            set
            {
                m_isForcedOpened = value;
                //notifyPropertyChanged("IsClosed");
            }
        }

        public bool IsClosed { get { return !IsForcedOpened && (Score == 9); } }
        public int Score { get { return m_tiles.Count; } }

        public void ReturnMeeple()
        {
            Claimer?.Player.ReturnMeeple(Claimer);
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
