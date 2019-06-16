using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Carcassonne.Model;
using System.Windows.Markup;
using System.Diagnostics;
using System.ComponentModel;

namespace Carcassonne.WPF
{
    /// <summary>
    /// Interaction logic for GameTile.xaml
    /// </summary>
    public partial class TileViewOld : UserControl
    {
        private static readonly Brush s_defaultBrush = new SolidColorBrush(Colors.DarkGray);
        private static readonly Brush s_cityBrush = new SolidColorBrush(Colors.SaddleBrown);
        private Meeple m_activeMeeple;
        public Meeple ActiveMeeple
        {
            get { return m_activeMeeple; }
            set
            {
                setActiveMeeple(value);
            }
        }

        public Brush CityBrush { get { return s_cityBrush; } }
        private void setActiveMeeple(Meeple m)
        {
            if (Dispatcher.CheckAccess())
            {
                m_activeMeeple = m;
                if (m_activeMeeple != null)
                {
                    markActiveRegion();
                }
                else
                {
                    clearActiveRegion(null, null);
                }
            }
            else
            {
                Dispatcher.Invoke(new Action<Meeple>((_m) => { setActiveMeeple(_m); }), m);
            }
        }

        private FrameworkElement m_activeElement;
        public FrameworkElement ActiveElement
        {
            get { return m_activeElement; }
            set
            {
                m_activeElement = value;
            }
        }

        public IClaimable ActiveClaim
        {
            get
            {
                return ActiveElement?.DataContext as IClaimable;
            }
        }

        public TileViewOld()
        {
            InitializeComponent();
        }

        public Tile Tile { get { return DataContext as Tile; } }

        private Point? m_location;
        public Point? Location
        {
            get
            {
                return Tile?.Location ?? m_location;
            }
            set
            {
                m_location = value;
            }
        }

        private void markActiveRegion()
        {
            foreach (var c in canvas.Children)
            {
                if (c is FrameworkElement fe
                    && fe.DataContext is IClaimable
                    && Mouse.DirectlyOver == c)
                {
                    setActiveElement(fe, null);
                }
            }
        }

        private void setActiveElement(object sender, MouseEventArgs e)
        {
            if (ActiveMeeple != null && Tile != null && Tile.Location != null)
            {
                if (sender is FrameworkElement fe
                    && fe.DataContext is IClaimable c
                    && Game.RuleSet.IsAvailable(c, ActiveMeeple.Type))
                {
                    ActiveElement = fe;
                }
                else
                {
                    ActiveElement = null;
                }
            }
        }

        void clearActiveRegion(object sender, MouseEventArgs e)
        {
            if (ActiveMeeple != null && Tile != null && Tile.Location != null)
            {
                ActiveElement = null;
            }
        }

        public bool ContainsRegion(IClaimable cl)
        {
            foreach (var c in canvas.Children)
            {
                if(c is FrameworkElement fe && fe.DataContext == cl)
                {
                    return true;
                }
            }
            return false;
        }

        private void GameTileView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Tile != null)
            {
                //Tile.PropertyChanged += new PropertyChangedEventHandler(Tile_PropertyChanged);
            }
        }

        //void Tile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if ("Location".Equals(e.PropertyName))
        //    {
        //        notifyPropertyChanged("Location");
        //    }
        //}
    }
}
