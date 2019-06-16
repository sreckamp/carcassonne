using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carcassonne.Model;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Input;
using GameBase.Model;
using GameBoard.WPF.ViewModel;
using GameBoard.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GameBase.WPF;

namespace Carcassonne.WPF.ViewModel
{
    public class GameViewModel: INotifyPropertyChanged
    {
        //private readonly Game m_game;
        private readonly Dispatcher m_dispatcher;

        public GameViewModel(params AbstractExpansionPack[] expansions)
        {
            m_dispatcher = System.Windows.Application.Current.Dispatcher;
//            m_dispatcher = dispatcher;
            Game = new Game(expansions);
            BoardViewModel = new GameBoardViewModel(Game.Board);
            BoardViewModel.Placed += boardViewModel_Placed;
            Game.ActivePlayerChanged += new EventHandler<ChangedValueArgs<Player>>(game_ActivePlayerChanged);
            Game.ActiveTileChanged += new EventHandler<ChangedValueArgs<Tile>>(game_ActiveTileChanged);
            PlayerViewModels = new MappingCollection<PlayerViewModel, Player>(Game.Players);
            DeckViewModel = new DispatchedObservableList<PlacementViewModel>(/*m_dispatcher, */new ObservableList<PlacementViewModel>());
            Game.Shuffle();
            foreach(var t in Game.m_deck)
            {
                DeckViewModel.Add(new PlacementViewModel(t, null));
            }
        }

        private void boardViewModel_Placed(object sender, MoveEventArgs e)
        {
            ActivePlayerViewModel.Place(e.Move);
        }

        void game_ActiveTileChanged(object sender, ChangedValueArgs<Tile> e)
        {
            BoardViewModel.SetActiveTile(e.NewVal);
            //Board.SetActiveTile(e.NewVal);
            //if (Board.ActiveTile != null)
            //{
            //    ActivePlayer.SetActiveTile(Board.ActiveTile);
            //}
        }

        public MappingCollection<PlayerViewModel, Player> PlayerViewModels { get; private set; }

        void game_ActivePlayerChanged(object sender, ChangedValueArgs<Player> e)
        {
            notifyPropertyChanged("ActivePlayer");
        }

        public DispatchedObservableList<PlacementViewModel> DeckViewModel { get; private set; }
        public Game Game { get; private set; }
        public GameBoardViewModel BoardViewModel { get; private set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            if (m_dispatcher.CheckAccess())
            {
                Debug.WriteLine("GameViewModel.PropChanged Act:" + name);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            else
            {
                Debug.WriteLine("GameViewModel.PropChanged Invoke:" + name);
                m_dispatcher.Invoke(new Action<string>((n) =>
                { notifyPropertyChanged(n); }), name);
            }
        }

        #endregion

        public PlayerViewModel ActivePlayerViewModel
        {
            get
            {
                return PlayerViewModels[Game.ActivePlayer];
            }
        }

        internal void AddPlayer(string name, Color color)
        {
            if (Game.AddPlayer(name) != null)
            {
                PlayerViewModel.ColorsForName[name] = new SolidColorBrush(color);
            }
        }
    }
}
