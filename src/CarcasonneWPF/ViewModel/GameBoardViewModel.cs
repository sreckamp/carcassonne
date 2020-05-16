﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Carcassonne.Model;
using GameBase.Model;
using System.Drawing;
using System.Linq;
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

        public GameBoardViewModel(CarcassonneGameBoard board)
        {
            StartColumnChanged += (sender, args) => { };
            StartRowChanged += (sender, args) => { };
            ColumnsChanged += (sender, args) => { };
            RowsChanged += (sender, args) => { };
            PropertyChanged += (sender, args) => { };
            Placed += (sender, args) => { };
            m_board = board;
            var placements = new MappingCollection<PlacementViewModel, Placement<Tile, CarcassonneMove>>(board.Placements, this);
            Grid = new OverlayDispatchableObservableList<PlacementViewModel>(m_grid, placements, m_active);
            AvailablePositions = new DispatchedObservableList<Point>(board.AvailableLocations);
            board.MinXChanged += board_MinXChanged;
            board.MaxXChanged += board_MaxXChanged;
            board.MinYChanged += board_MinYChanged;
            board.MaxYChanged += board_MaxYChanged;
            InitializeGrid();
        }

        public OverlayDispatchableObservableList<PlacementViewModel> Grid { get; }
        public DispatchedObservableList<Point> AvailablePositions { get; }

        public event EventHandler<ChangedValueArgs<int>> StartColumnChanged;
        private int m_startColumn = 0;
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
        private int m_startRow = 0;
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
        private int m_columns = 0;
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
        private int m_rows = 0;
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
        private IEnumerable<CarcassonneMove> m_availableMoves = Enumerable.Empty<CarcassonneMove>();
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

        public ICommand PlaceCommand => new RelayCommand<object>(Place, CanPlace);
        private bool CanPlace(object obj)
        {
            return Fits(ActivePlacementViewModel);

        }

        public event EventHandler<MoveEventArgs> Placed;
        private void Place(object obj)
        {
            Placed?.Invoke(this, new MoveEventArgs(ActivePlacementViewModel?.Move));
        }

        public ICommand MoveCommand => new RelayCommand<GridCellRoutedEventArgs>(Move, CanMove);
        private bool CanMove(GridCellRoutedEventArgs args)
        {
            return ActivePlacementViewModel != null;
        }

        private void Move(GridCellRoutedEventArgs args)
        {
            ActivePlacementViewModel?.SetCell(args.Cell);
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

        public CarcassonneMove Move { get; }
    }
}
