using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Carcassonne.Model;
using GameBase.Model;
using GameBase.WPF.ViewModel;

namespace Carcassonne.WPF.ViewModel
{
    public class GameViewModel: INotifyPropertyChanged
    {
        //private readonly Game m_game;
        private readonly Dispatcher m_dispatcher;

        public GameViewModel(params ExpansionPack[] expansions)
        {
            m_dispatcher = Application.Current.Dispatcher;
            PropertyChanged += (sender, args) => { };
            Game = new Game(expansions);
            BoardViewModel = new GameBoardViewModel(Game.Board, Game.RuleSet);
            BoardViewModel.Placed += boardViewModel_Placed;
            Game.ActivePlayerChanged += game_ActivePlayerChanged;
            Game.ActiveTileChanged += game_ActiveTileChanged;
            PlayerViewModels = new MappingCollection<PlayerViewModel, Player>(Game.Players);
            DeckViewModel = new DispatchedObservableList<PlacementViewModel>(/*m_dispatcher, */new ObservableList<PlacementViewModel>());
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

        public MappingCollection<PlayerViewModel, Player> PlayerViewModels { get; }

        void game_ActivePlayerChanged(object sender, ChangedValueArgs<Player> e)
        {
            NotifyPropertyChanged("ActivePlayer");
        }

        public DispatchedObservableList<PlacementViewModel> DeckViewModel { get; }
        public Game Game { get; }
        public GameBoardViewModel BoardViewModel { get; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (m_dispatcher.CheckAccess())
            {
                Debug.WriteLine("GameViewModel.PropChanged Act:" + name);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            else
            {
                Debug.WriteLine("GameViewModel.PropChanged Invoke:" + name);
                m_dispatcher.Invoke(new Action<string>(NotifyPropertyChanged), name);
            }
        }

        #endregion

        public PlayerViewModel ActivePlayerViewModel => PlayerViewModels[Game.ActivePlayer];

        internal void AddPlayer(string name, Color color)
        {
            if (Game.AddPlayer(name) != Player.None)
            {
                PlayerViewModel.ColorsForName[name] = new SolidColorBrush(color);
            }
        }
    }
}
