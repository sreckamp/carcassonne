using System.Windows.Media;
using MBrush = System.Windows.Media.Brush;
using System.Collections.Specialized;
using System.Drawing;
using GameBase.Model;
using Carcassonne.Model;
using GameBase.WPF.ViewModel;

namespace Carcassonne.WPF.ViewModel
{
    public class PointViewModel : PlacementViewModel
    {
        private static readonly MBrush SAvailableColor = new SolidColorBrush(Colors.LightGray);
        private static readonly MBrush SUnavailableColor = new SolidColorBrush(Colors.Transparent);

        public PointViewModel(Point p, IGridManager gridManager)
            : base(new Placement<Tile, CarcassonneMove>(null, new CarcassonneMove(p, Model.Rotation.None)), gridManager)
        {
            //Location = p;
            //board.AvailablePositions.CollectionChanged += new NotifyCollectionChangedEventHandler(AvailablePositions_CollectionChanged);
        }

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
