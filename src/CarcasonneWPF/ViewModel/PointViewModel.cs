using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Media;
using Carcassonne.Model;
using GameBase.Model;
using GameBase.WPF.ViewModel;
using MBrush = System.Windows.Media.Brush;

namespace Carcassonne.WPF.ViewModel
{
    public class PointViewModel : PlacementViewModel
    {
        private static readonly MBrush SAvailableColor = new SolidColorBrush(Colors.LightGray);
        private static readonly MBrush SUnavailableColor = new SolidColorBrush(Colors.Transparent);

        public PointViewModel(Point p, IGridManager gridManager)
            : base(new Placement<ITile>(Tile.None, p), gridManager)
        {
            //Location = p;
            //board.AvailablePositions.CollectionChanged += new NotifyCollectionChangedEventHandler(AvailablePositions_CollectionChanged);
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        void availablePositions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if ((e.NewItems != null && e.NewItems.Contains(Location))
            //    || (e.OldItems != null && e.OldItems.Contains(Location)))
            //{
            //    NotifyPropertyChanged("Color");
            //}
        }

        public MBrush Color => SAvailableColor;
    }
}
