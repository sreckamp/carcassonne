using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Carcassonne.Model;
using System.Windows.Threading;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Media;
using GameBase.Model;
using System.Drawing;
using GameBoard.WPF.ViewModel;
using GameBoard.Model;
using GameBase.WPF.ViewModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GameBase.WPF;

namespace Carcassonne.WPF.ViewModel
{
    public class GameBoardViewModel : IGridManager, INotifyPropertyChanged
    {
        private readonly CarcassonneGameBoard m_board;
        private readonly ObservableList<PlacementViewModel> m_grid = new ObservableList<PlacementViewModel>();
        private readonly ObservableList<PlacementViewModel> m_active = new ObservableList<PlacementViewModel>();
        private readonly MappingCollection<PlacementViewModel, Placement<Tile, CarcassonneMove>> m_placements;

        public GameBoardViewModel(CarcassonneGameBoard board)
        {
            m_board = board;
            m_placements = new MappingCollection<PlacementViewModel, Placement<Tile, CarcassonneMove>>(board.Placements, this);
            Grid = new OverlayDispatchableObservableList<PlacementViewModel>(m_grid, m_placements, m_active);
            board.MinXChanged += new EventHandler<ChangedValueArgs<int>>(board_MinXChanged);
            board.MaxXChanged += new EventHandler<ChangedValueArgs<int>>(board_MaxXChanged);
            board.MinYChanged += new EventHandler<ChangedValueArgs<int>>(board_MinYChanged);
            board.MaxYChanged += new EventHandler<ChangedValueArgs<int>>(board_MaxYChanged);
            initializeGrid();
        }

        public OverlayDispatchableObservableList<PlacementViewModel> Grid { get; private set; }
        public DispatchedObservableList<Point> AvailablePositions { get; private set; }

        public event EventHandler<ChangedValueArgs<int>> StartColumnChanged;
        private int m_startColumn = 0;
        public int StartColumn
        {
            get => m_startColumn;
            set
            {
                var old = m_startColumn;
                m_startColumn = value;
                ChangedValueArgs<int>.Trigger(StartColumnChanged, this, old, value);
            }
        }

        public event EventHandler<ChangedValueArgs<int>> StartRowChanged;
        private int m_startRow = 0;
        public int StartRow
        {
            get => m_startRow;
            set
            {
                var old = m_startRow;
                m_startRow = value;
                ChangedValueArgs<int>.Trigger(StartRowChanged, this, old, value);
            }
        }

        private int m_columns = 0;
        public int Columns
        {
            get => m_columns;
            set
            {
                m_columns = value;
                notifyPropertyChanged(nameof(Columns));
            }
        }

        private int m_rows = 0;
        public int Rows
        {
            get => m_rows;
            set
            {
                m_rows = value;
                notifyPropertyChanged(nameof(Rows));
            }
        }

        private void initializeGrid()
        {
            if (m_board != null)
            {
                Columns = m_board.MaxX - m_board.MinX + 3;
                Rows = m_board.MaxY - m_board.MinY + 3;
                StartColumn = m_board.MinX - 1;
                StartRow = m_board.MinY - 1;
                for (int x = StartColumn; x <= m_board.MaxX + 1; x++)
                {
                    addColumn(x);
                }
            }
        }

        private void board_MinXChanged(object sender, ChangedValueArgs<int> e)
        {
            Columns = m_board.MaxX - m_board.MinX + 3;
            StartColumn = m_board.MinX - 1;
            if (e.NewVal < e.OldVal)
            {
                addColumn(StartColumn);
            }
        }

        private void board_MaxXChanged(object sender, ChangedValueArgs<int> e)
        {
            Columns = m_board.MaxX - m_board.MinX + 3;
            if (e.NewVal > e.OldVal)
            {
                addColumn(m_board.MaxX + 1);
            }
        }

        private void board_MinYChanged(object sender, ChangedValueArgs<int> e)
        {
            Rows = m_board.MaxY - m_board.MinY + 3;
            StartRow = m_board.MinY - 1;
            if (e.NewVal < e.OldVal)
            {
                addRow(StartRow);
            }
        }

        private void board_MaxYChanged(object sender, ChangedValueArgs<int> e)
        {
            Rows = m_board.MaxY - m_board.MinY + 3;
            if (e.NewVal > e.OldVal)
            {
                addRow(m_board.MaxY + 1);
            }
        }

        private void addColumn(int x)
        {
            for (int y = m_board.MinY - 1; y <= m_board.MaxY + 1; y++)
            {
                addPoint(x, y);
            }
        }

        private void addRow(int y)
        {
            for (int x = m_board.MinX - 1; x <= m_board.MaxX + 1; x++)
            {
                addPoint(x, y);
            }
        }

        private void addPoint(int x, int y)
        {
            var p = new Point(x, y);
            m_grid.Add(new PointViewModel(p, this));
        }

        public PlacementViewModel ActivePlacementViewModel => m_active.Count > 0 ? m_active[0] : null;
        private List<CarcassonneMove> m_availableMoves = new List<CarcassonneMove>();
        public void SetActiveTile(Tile t)
        {
            if (t != null)
            {
                m_active.Add(new PlacementViewModel(t, this));
                ActivePlacementViewModel?.ChangedDepth();
                m_availableMoves = m_board.GetAvailableMoves(t);
            }
            else
            {
                m_active.Clear();
            }
        }

        public ICommand PlaceCommand => new RelayCommand<object>(place, canPlace);
        private bool canPlace(object obj)
        {
            return Fits(ActivePlacementViewModel);

        }

        public event EventHandler<MoveEventArgs> Placed;
        private void place(object obj)
        {
            Placed?.Invoke(this, new MoveEventArgs(ActivePlacementViewModel?.Move));
        }

        public ICommand MoveCommand => new RelayCommand<GridCellRoutedEventArgs>(move, canMove);
        private bool canMove(GridCellRoutedEventArgs args)
        {
            return ActivePlacementViewModel != null;
        }

        private void move(GridCellRoutedEventArgs args)
        {
            ActivePlacementViewModel?.SetCell(args.Cell);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            //if (m_dispatcher.CheckAccess())
            //{
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //}
            //else
            //{
            //    Debug.WriteLine("PropChanged Invoke");
            //    m_dispatcher.Invoke(new Action<string>((n) =>
            //    { notifyPropertyChanged(n); }), name);
            //}
        }

        #endregion

        internal bool Fits(PlacementViewModel pvm)
        {
            return !m_active.Contains(pvm) || m_availableMoves.Contains(pvm.Move);
        }
        internal bool IsBackground(PlacementViewModel pvm)
        {
            return m_grid.Contains(pvm);
        }
        internal bool IsForeground(PlacementViewModel pvm)
        {
            return m_active.Contains(pvm);
        }
    }
    public class MoveEventArgs:EventArgs
    {
        public MoveEventArgs(CarcassonneMove move)
        {
            Move = move;
        }

        public CarcassonneMove Move { get; private set; }
    }
}
