using System.Drawing;
using System.Windows.Media;
using Carcassonne.Model;
using GameBase.Model;
using MBrush = System.Windows.Media.Brush;

namespace Carcassonne.WPF.ViewModel
{
    public class PointViewModel : PlacementViewModel
    {
        private static readonly MBrush SAvailableColor = new SolidColorBrush(Colors.LightGray);
        private static readonly MBrush SUnavailableColor = new SolidColorBrush(Colors.Transparent);

        static PointViewModel()
        {
            SAvailableColor.Freeze();
            SUnavailableColor.Freeze();
        }

        public PointViewModel(Point p, BoardViewModel boardViewModel)
            : base(new Placement<ITile>(NopTile.Instance, p), boardViewModel)
        {
            IsBackground = true;
        }

        public MBrush Color => SAvailableColor;
    }
}
