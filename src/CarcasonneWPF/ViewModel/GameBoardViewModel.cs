using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Carcassonne.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GameBase.Model;
using GameBase.Model.Rules;
using GameBase.WPF;
using GameBase.WPF.ViewModel;
using Move = Carcassonne.Model.Move;
using Point = System.Drawing.Point;

namespace Carcassonne.WPF.ViewModel
{
    public class BoardViewModel : IGridManager, INotifyPropertyChanged
    {
        internal static readonly ICommand NopCommand = new RelayCommand<GridCellRoutedEventArgs>(
            (o) => { Debug.WriteLine("Nop Action"); },
            (o) =>
            {
                Debug.WriteLine("Nop Action");
                return false;
            }); 

        private readonly Board m_board;
        private readonly ObservableList<PlacementViewModel> m_grid = new ObservableList<PlacementViewModel>();
        private readonly ObservableList<PlacementViewModel> m_active = new ObservableList<PlacementViewModel>();
        private readonly IPlaceRule<IGameBoard, ITile> m_placeRule;

        public BoardViewModel(Board board, IPlaceRule<IGameBoard, ITile> placeRule)
        {
            StartColumnChanged += (sender, args) => Debug.WriteLine($"BoardViewModel.StartColumn {args.OldVal}=>{args.NewVal}");
            StartRowChanged += (sender, args) => Debug.WriteLine($"BoardViewModel.StartRow {args.OldVal}=>{args.NewVal}");
            ColumnsChanged += (sender, args) => Debug.WriteLine($"BoardViewModel.Columns {args.OldVal}=>{args.NewVal}");
            RowsChanged += (sender, args) => Debug.WriteLine($"BoardViewModel.Rows {args.OldVal}=>{args.NewVal}");
            PropertyChanged += (sender, args) => Debug.WriteLine($"BoardViewModel.Property {args.PropertyName}");

            var placements = new DispatchedMappingCollection<PlacementViewModel, Placement<ITile>>(board.Placements, this);
            Grid = new OverlayDispatchedObservableList<PlacementViewModel>(m_grid, placements, m_active);

            m_board = board;

            board.MinXChanged += board_MinXChanged;
            board.MaxXChanged += board_MaxXChanged;
            board.MinYChanged += board_MinYChanged;
            board.MaxYChanged += board_MaxYChanged;

            m_placeRule = placeRule;

            InitializeGrid();
        }

        public OverlayDispatchedObservableList<PlacementViewModel> Grid { get; }

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
            Debug.WriteLine($"BoardViewModel.MinXChanged {e.OldVal}=>{e.NewVal}");
            Columns = m_board.MaxX - m_board.MinX + 3;
            StartColumn = m_board.MinX - 1;
            if (e.NewVal < e.OldVal)
            {
                AddColumn(StartColumn);
            }
        }

        private void board_MaxXChanged(object sender, ChangedValueArgs<int> e)
        {
            Debug.WriteLine($"BoardViewModel.MaxXChanged {e.OldVal}=>{e.NewVal}");
            Columns = m_board.MaxX - m_board.MinX + 3;
            if (e.NewVal > e.OldVal)
            {
                AddColumn(m_board.MaxX + 1);
            }
        }

        private void board_MinYChanged(object sender, ChangedValueArgs<int> e)
        {
            Debug.WriteLine($"BoardViewModel.MinYChanged {e.OldVal}=>{e.NewVal}");
            Rows = m_board.MaxY - m_board.MinY + 3;
            StartRow = m_board.MinY - 1;
            if (e.NewVal < e.OldVal)
            {
                AddRow(StartRow);
            }
        }

        private void board_MaxYChanged(object sender, ChangedValueArgs<int> e)
        {
            Debug.WriteLine($"BoardViewModel.MaxYChanged {e.OldVal}=>{e.NewVal}");
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

        private IEnumerable<Move> m_availableMoves = Enumerable.Empty<Move>();

        public void SetActivePlacement(PlacementViewModel placement)
        {
            m_active.Clear();
            m_active.Add(placement);
            placement.IsForeground = true;
        }

        public void ClearActiveTile()
        {
            m_active.Clear();
        }

        private ICommand m_leftButtonCommand = NopCommand;

        public ICommand LeftButtonCommand
        {
            get => m_leftButtonCommand;
            set
            {
                m_leftButtonCommand = value;
                NotifyPropertyChanged(nameof(LeftButtonCommand));
            }
        }

        private ICommand m_rightButtonCommand = NopCommand;

        public ICommand RightButtonCommand
        {
            get => m_rightButtonCommand;
            set
            {
                m_rightButtonCommand = value;
                NotifyPropertyChanged(nameof(RightButtonCommand));
            }
        }

        public ICommand MoveCommand { get; set; } = NopCommand;

        private bool m_monitorMouse = true;

        public bool MonitorMouse
        {
            get => m_monitorMouse;
            set
            {
                m_monitorMouse = value;
                NotifyPropertyChanged(nameof(MonitorMouse));
            }
        }
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                Debug.WriteLine($"BoardViewModel.PropChanged {name}");
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
            }
            else
            {
                Debug.WriteLine($"Invoke BoardViewModel.PropChanged {name}");
                Application.Current.Dispatcher.Invoke(new Action<string>(NotifyPropertyChanged), name);
            }
        }

        #endregion
    }
}
