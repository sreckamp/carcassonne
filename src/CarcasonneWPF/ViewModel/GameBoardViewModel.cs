using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using Carcassonne.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GameBase.Model;
using GameBase.Model.Rules;
using GameBase.WPF;
using GameBase.WPF.ViewModel;
using Move = Carcassonne.Model.Move;

namespace Carcassonne.WPF.ViewModel
{
    public class GameBoardViewModel : IGridManager, INotifyPropertyChanged
    {
        private readonly GameBoard m_board;
        private readonly ObservableList<PlacementViewModel> m_grid = new ObservableList<PlacementViewModel>();
        private readonly ObservableList<PlacementViewModel> m_active = new ObservableList<PlacementViewModel>();
        private readonly IPlaceRule<IGameBoard, ITile> m_placeRule;

        public GameBoardViewModel(GameBoard board, IPlaceRule<IGameBoard, ITile> placeRule)
        {
            StartColumnChanged += (sender, args) => { };
            StartRowChanged += (sender, args) => { };
            ColumnsChanged += (sender, args) => { };
            RowsChanged += (sender, args) => { };
            PropertyChanged += (sender, args) => { };
            Placed += (sender, args) => { };
            m_board = board;
            var placements = new MappingCollection<PlacementViewModel, Placement<ITile>>(board.Placements, this);
            Grid = new OverlayDispatchedObservableList<PlacementViewModel>(m_grid, placements, m_active);
            // AvailablePositions = new DispatchedObservableList<Point>(board.AvailableLocations);
            board.MinXChanged += board_MinXChanged;
            board.MaxXChanged += board_MaxXChanged;
            board.MinYChanged += board_MinYChanged;
            board.MaxYChanged += board_MaxYChanged;
            m_placeRule = placeRule;
            InitializeGrid();
        }

        public OverlayDispatchedObservableList<PlacementViewModel> Grid { get; }
        // public DispatchedObservableList<Point> AvailablePositions { get; }

        public event EventHandler<ChangedValueArgs<int>> StartColumnChanged;
        private int m_startColumn;
        public int StartColumn
        {
            get => m_startColumn;
            private set
            {
                var old = m_startColumn;
                m_startColumn = value;
                StartColumnChanged.Invoke(this, new ChangedValueArgs<int>(old, value));
            }
        }

        public event EventHandler<ChangedValueArgs<int>> StartRowChanged;
        private int m_startRow;
        public int StartRow
        {
            get => m_startRow;
            private set
            {
                var old = m_startRow;
                m_startRow = value;
                StartRowChanged.Invoke(this, new ChangedValueArgs<int>(old, value));
            }
        }

        public event EventHandler<ChangedValueArgs<int>> ColumnsChanged;
        private int m_columns;
        public int Columns
        {
            get => m_columns;
            set
            {
                var old = m_columns;
                m_columns = value;
                ColumnsChanged.Invoke(this, new ChangedValueArgs<int>(old, value));
                NotifyPropertyChanged(nameof(Columns));
            }
        }

        public event EventHandler<ChangedValueArgs<int>> RowsChanged;
        private int m_rows;
        public int Rows
        {
            get => m_rows;
            set
            {
                var old = m_columns;
                m_rows = value;
                RowsChanged.Invoke(this, new ChangedValueArgs<int>(old, value));
                NotifyPropertyChanged(nameof(Rows));
            }
        }

        private void InitializeGrid()
        {
            Columns = m_board.MaxX - m_board.MinX + 3;
            Rows = m_board.MaxY - m_board.MinY + 3;
            StartColumn = m_board.MinX - 1;
            StartRow = m_board.MinY - 1;
            for (var x = StartColumn; x <= m_board.MaxX + 1; x++)
            {
                AddColumn(x);
            }
        }

        private void board_MinXChanged(object sender, ChangedValueArgs<int> e)
        {
            Columns = m_board.MaxX - m_board.MinX + 3;
            StartColumn = m_board.MinX - 1;
            if (e.NewVal < e.OldVal)
            {
                AddColumn(StartColumn);
            }
        }

        private void board_MaxXChanged(object sender, ChangedValueArgs<int> e)
        {
            Columns = m_board.MaxX - m_board.MinX + 3;
            if (e.NewVal > e.OldVal)
            {
                AddColumn(m_board.MaxX + 1);
            }
        }

        private void board_MinYChanged(object sender, ChangedValueArgs<int> e)
        {
            Rows = m_board.MaxY - m_board.MinY + 3;
            StartRow = m_board.MinY - 1;
            if (e.NewVal < e.OldVal)
            {
                AddRow(StartRow);
            }
        }

        private void board_MaxYChanged(object sender, ChangedValueArgs<int> e)
        {
            Rows = m_board.MaxY - m_board.MinY + 3;
            if (e.NewVal > e.OldVal)
            {
                AddRow(m_board.MaxY + 1);
            }
        }

        private void AddColumn(int x)
        {
            for (var y = m_board.MinY - 1; y <= m_board.MaxY + 1; y++)
            {
                AddPoint(x, y);
            }
        }

        private void AddRow(int y)
        {
            for (var x = m_board.MinX - 1; x <= m_board.MaxX + 1; x++)
            {
                AddPoint(x, y);
            }
        }

        private void AddPoint(int x, int y)
        {
            var p = new Point(x, y);
            m_grid.Add(new PointViewModel(p, this));
        }

        public PlacementViewModel ActivePlacementViewModel => m_active.Count > 0 ? m_active[0] : null;
        private IEnumerable<Move> m_availableMoves = Enumerable.Empty<Move>();
        public void SetActiveTile(Tile t)
        {
            if (t != null)
            {
                m_active.Add(new PlacementViewModel(t, this));
                ActivePlacementViewModel?.ChangedDepth();
                // m_availableMoves = m_board.GetAvailableMoves(t);
            }
            else
            {
                m_active.Clear();
            }
        }

        public ICommand PlaceCommand => new RelayCommand<object>(Place, CanPlace);
        public event EventHandler<PlaceEventArgs> Placed;
        private void Place(object obj)
        {
            Placed.Invoke(this, new PlaceEventArgs(ActivePlacementViewModel.Piece, ActivePlacementViewModel.Location));
        }

        private bool CanPlace(object obj)
        {
            return Fits(ActivePlacementViewModel);

        }

        public ICommand RotateCommand => new RelayCommand<object>(Rotate, CanRotate);
        private void Rotate(object obj)
        {
            ActivePlacementViewModel.TileRotation = ActivePlacementViewModel.TileRotation.RotateCw();
        }

        private bool CanRotate(object obj)
        {
            return ActivePlacementViewModel.Piece != Tile.None;
        }

        public ICommand MoveCommand => new RelayCommand<GridCellRoutedEventArgs>(Move, CanMove);
        private bool CanMove(GridCellRoutedEventArgs args)
        {
            return ActivePlacementViewModel.Piece != Tile.None;
        }

        private void Move(GridCellRoutedEventArgs args)
        {
            ActivePlacementViewModel.SetCell(args.Cell);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
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
            return !m_active.Contains(pvm) || (m_placeRule.Applies(m_board, pvm.Piece, pvm.Location)
                && m_placeRule.Fits(m_board, pvm.Piece, pvm.Location));
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
    public class PlaceEventArgs : EventArgs
    {
        public PlaceEventArgs(ITile tile, Point location)
        {
            Move = new Move(location, (tile is RotatedTile rt) ? rt.Rotation : Rotation.None);
        }

        public Move Move { get; }
    }
}
