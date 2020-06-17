using System.Drawing;
using System.Windows.Media;
using Carcassonne.Model;
using GameBase.Model;
using GameBase.WPF.ViewModel;
using MBrush = System.Windows.Media.Brush;

namespace Carcassonne.WPF.ViewModel
{
    public class LocationViewModel : PlacementViewModel
    {
        private static readonly MBrush SAvailableColor = new SolidColorBrush(Colors.LightGray);
        private static readonly MBrush SUnavailableColor = new SolidColorBrush(Colors.Transparent);

        static LocationViewModel()
        {
            SAvailableColor.Freeze();
            SUnavailableColor.Freeze();
        }

        public LocationViewModel(Point location, IGridManager gridManager)
            : base(new Placement<ITile>(NopTile.Instance, location), gridManager)
        {
            IsBackground = true;
        }

        public MBrush Color => SAvailableColor;
    }
}
