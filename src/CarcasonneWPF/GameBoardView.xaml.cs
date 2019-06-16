using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Carcassonne.WPF.ViewModel;
using DPoint = System.Drawing.Point;
using GameBase.WPF;

namespace Carcassonne.WPF
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class GameBoardView : ItemsControl
    {
        public GameBoardView()
        {
            InitializeComponent();
        }

        //private void mouseEnter(object sender, MouseEventArgs e)
        //{
        //    if (sender is FrameworkElement fe 
        //        //&& fe.DataContext is AbstractGameSquareViewModel gs 
        //        && DataContext is GameBoardViewModel gbvm)
        //    {
        //        //gbvm.MoveActiveTile(gs.Location);
        //    }
        //}

        //private void tileView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (DataContext is GameBoardViewModel gbvm)
        //    {
        //        gbvm.PlaceActiveTile(m_lastGrid);
        //    }
        //}

        //private DPoint m_lastGrid = new DPoint(int.MinValue, int.MinValue);
        //private void mouseMove(object sender, MouseEventArgs e)
        //{
        //    var p = e.GetPosition(this);
        //    if (m_grid != null)
        //    {
        //        var gbvm = DataContext as GameBoardViewModel;
        //        int row = 0, column = 0;
        //        while ((column + 1) * m_grid.ColumnWidth < p.X) column++;
        //        while ((row + 1) * m_grid.RowHeight < p.Y) row++;
        //        var gc = new DPoint(column + gbvm.StartColumn, row + gbvm.StartColumn);
        //        if (gc != m_lastGrid)
        //        {
        //            //gbvm.MoveActiveTile(gc);
        //            m_lastGrid = gc;
        //        }
        //    }
        //}

        //private DynamicGrid m_grid = null;
        //private void dynamicGrid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    m_grid = sender as DynamicGrid;
        //}
    }
}
