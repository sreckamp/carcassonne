using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Carcassonne.Model;
using System.Windows.Markup;

namespace Carcassonne.WPF
{
    /// <summary>
    /// Interaction logic for GameTile.xaml
    /// </summary>
    public partial class TileView : UserControl
    {
        //private static readonly Brush s_defaultBrush = new SolidColorBrush(Colors.DarkGray);
        //private static readonly Brush s_cityBrush = new SolidColorBrush(Colors.SaddleBrown);
        //private Meeple m_activeMeeple;
        //public Meeple ActiveMeeple
        //{
        //    get { return m_activeMeeple; }
        //    set
        //    {
        //        setActiveMeeple(value);
        //    }
        //}

        //public Brush CityBrush { get { return s_cityBrush; } }
        //private void setActiveMeeple(Meeple m)
        //{
        //    if (Dispatcher.CheckAccess())
        //    {
        //        m_activeMeeple = m;
        //        if (m_activeMeeple != null)
        //        {
        //            markActiveRegion();
        //        }
        //        else
        //        {
        //            clearActiveRegion(null, null);
        //        }
        //    }
        //    else
        //    {
        //        Dispatcher.Invoke(new Action<Meeple>((_m) => { setActiveMeeple(_m); }), m);
        //    }
        //}

        //private FrameworkElement m_activeElement;
        //public FrameworkElement ActiveElement
        //{
        //    get { return m_activeElement; }
        //    set
        //    {
        //        m_activeElement = value;
        //    }
        //}

        //public IClaimable ActiveClaim
        //{
        //    get
        //    {
        //        if (ActiveElement != null)
        //        {
        //            return ActiveElement.DataContext as IClaimable;
        //        }
        //        return null;
        //    }
        //}

        public TileView()
        {
            InitializeComponent();
        }

        private void SetActiveElement(object sender, MouseEventArgs e)
        {

        }

        private void ClearActiveElement(object sender, MouseEventArgs e)
        {

        }

        //private void GameTileView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{

        //}

        //public Tile Tile { get { return DataContext as Tile; } }

        //private Point? m_location;
        //public Point? Location
        //{
        //    get
        //    {
        //        if (Tile != null && Tile.Location != null) return Tile.Location;
        //        return m_location;
        //    }
        //    set
        //    {
        //        m_location = value;
        //    }
        //}

        //private void markActiveRegion()
        //{
        //    foreach (var c in canvas.Children)
        //    {
        //        var fe = c as FrameworkElement;
        //        if (Mouse.DirectlyOver == fe && fe.DataContext is IClaimable)
        //        {
        //            setActiveElement(fe, null);
        //        }
        //    }
        //}

        //private void setActiveElement(object sender, MouseEventArgs e)
        //{
        //    if (ActiveMeeple != null && Tile != null && Tile.Location != null)
        //    {
        //        var fe = sender as FrameworkElement;
        //        var c = fe.DataContext as IClaimable;
        //        if (Game.RuleSet.IsAvailable(c, ActiveMeeple.Type))
        //        {
        //            ActiveElement = sender as FrameworkElement;
        //        }
        //        else
        //        {
        //            ActiveElement = null;
        //        }
        //    }
        //}

        //void clearActiveRegion(object sender, MouseEventArgs e)
        //{
        //    if (ActiveMeeple != null && Tile != null && Tile.Location != null)
        //    {
        //        ActiveElement = null;
        //    }
        //}

        //public bool ContainsRegion(IClaimable cl)
        //{
        //    foreach (var c in canvas.Children)
        //    {
        //        var fe = c as FrameworkElement;
        //        if(fe != null && fe.DataContext == cl)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private void GameTileView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (Tile != null)
        //    {
        //        //Tile.PropertyChanged += new PropertyChangedEventHandler(Tile_PropertyChanged);
        //    }
        //}

        //void Tile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if ("Location".Equals(e.PropertyName))
        //    {
        //        notifyPropertyChanged("Location");
        //    }
        //}
    }

    //public class EdgeRegionTypeToColor : MarkupExtension, IValueConverter
    //{
    //    #region IValueConverter Members

    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        var t = (RegionType)value;
    //        if (targetType == typeof(Brush))
    //        {
    //            switch (t)
    //            {
    //                case RegionType.City:
    //                    return new SolidColorBrush(Colors.SaddleBrown);
    //                case RegionType.River:
    //                    return new SolidColorBrush(Colors.DarkBlue);
    //                case RegionType.Road:
    //                    return new SolidColorBrush(Colors.Gainsboro);
    //            }
    //        }
    //        return DependencyProperty.UnsetValue;
    //    }

    //    public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion

    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return this;
    //    }
    //}

    public class TileRegionFilter : MarkupExtension, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TileRegion tr && parameter is TileRegionType)
            {
                var type = (TileRegionType)parameter;
                if (tr.Type == type)
                {
                    return value;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
