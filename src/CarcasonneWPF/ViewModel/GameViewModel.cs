using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Carcassonne.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GameBase.Model;
using GameBase.WPF;

namespace Carcassonne.WPF.ViewModel
{
    public class GameViewModel: INotifyPropertyChanged
    {
        private static readonly PlayerViewModel SNopPlayerViewModel = new PlayerViewModel(NopPlayer.Instance);

        private readonly ICommand m_placeCommand;
        private readonly ICommand m_rotateCommand;
        private readonly ICommand m_claimCommand;
        private readonly ICommand m_chooseMeepleCommand;

        private readonly PlacementViewModel m_defaultPlacement;
        private PlacementViewModel m_active;

        public GameViewModel(IEnumerable<ExpansionPack> expansions)
        {
            m_placeCommand = new RelayCommand<object>(Place, CanPlace);
            m_rotateCommand = new RelayCommand<object>(Rotate, CanRotate);
            m_claimCommand = new RelayCommand<object>(Claim, CanClaim);
            m_chooseMeepleCommand = new RelayCommand<object>(ChooseMeeple, CanChooseMeeple);

            PropertyChanged += (sender, args) => { };

            Game = new Game(expansions);
            BoardViewModel = new BoardViewModel(Game.Board/*, Game.RuleSet*/)
            {
                MoveCommand = new RelayCommand<GridCellRoutedEventArgs>(Move, CanMove)
            };
            m_defaultPlacement = new PlacementViewModel(NopTile.Instance, BoardViewModel);
            m_active = m_defaultPlacement;
            Game.ActivePlayerChanged += Game_ActivePlayerChanged;
            Game.ActiveTileChanged += Game_ActiveTileChanged;
            Game.GameStateChanged += Game_GameStateChanged;
            PlayerViewModels = new MappingCollection<PlayerViewModel, IPlayer>(Game.Players);
            // var cards = new ObservableList<ITile>();
            // Game.Shuffle();
            // Game.DumpDeck(cards);
            // DeckViewModel = new MappingCollection<PlacementViewModel, ITile>(cards, BoardViewModel);
        }
        // public MappingCollection<PlacementViewModel, ITile> DeckViewModel { get; }

        /// <summary>
        /// TODO:Eliminate
        /// </summary>
        public IEnumerable<IPointContainer> PointContainers => Game.Board.Placements.SelectMany(
            p =>
            {
                var pcs = p.Piece.Regions.Select(r => r.Container).Where(c=> !(c is NopPointContainer));
                return (p.Piece.TileRegion1 is IPointContainer pc) ? pcs.Append(pc) : pcs;
            }).Distinct();

        private void Game_GameStateChanged(object sender, ChangedValueArgs<GameState> e)
        {
            switch (e.NewVal)
            {
                case GameState.Claim:
                    NotifyPropertyChanged(nameof(PointContainers));
                    BoardViewModel.MonitorMouse = false;
                    m_active = m_defaultPlacement;
                    BoardViewModel.ClearActiveTile();
                    BoardViewModel.LeftButtonCommand = m_claimCommand;
                    BoardViewModel.RightButtonCommand = m_chooseMeepleCommand;
                    break;
                case GameState.Place:
                    BoardViewModel.MonitorMouse = true;
                    BoardViewModel.LeftButtonCommand = m_placeCommand;
                    BoardViewModel.RightButtonCommand = m_rotateCommand;
                    break;
                case GameState.Score:
                    BoardViewModel.LeftButtonCommand = BoardViewModel.NopCommand;
                    BoardViewModel.RightButtonCommand = BoardViewModel.NopCommand;
                    Game.Score();
                    break;
                case GameState.Next:
                    Game.NextTurn();
                    break;
                case GameState.End:
                case GameState.NotStarted:
                    BoardViewModel.LeftButtonCommand = BoardViewModel.NopCommand;
                    BoardViewModel.RightButtonCommand = BoardViewModel.NopCommand;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // TODO: Highlight meeple & allow claiming of closed regions on active tile. 
        private void Game_ActiveTileChanged(object sender, ChangedValueArgs<ITile> e)
        {
            m_active = new PlacementViewModel(e.NewVal, BoardViewModel);
            var meeple = ActivePlayerViewModel.MeepleViewModels.ToList();
            if (meeple.Count > 0)
            {
                m_active.SetMeeple(meeple[0]);
            }
            BoardViewModel.SetActivePlacement(m_active);
        }

        public MappingCollection<PlayerViewModel, IPlayer> PlayerViewModels { get; }

        private void Game_ActivePlayerChanged(object sender, ChangedValueArgs<IPlayer> e)
        {
            NotifyPropertyChanged(nameof(ActivePlayerViewModel));
        }

        public Game Game { get; }
        public BoardViewModel BoardViewModel { get; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public PlayerViewModel ActivePlayerViewModel => Game.Players.Contains(Game.ActivePlayer)
            ? PlayerViewModels[Game.ActivePlayer]
            : SNopPlayerViewModel;

        internal void AddPlayer(string name, Color color)
        {
            if (Game.AddPlayer(name) == NopPlayer.Instance) return;
            PlayerViewModel.ColorsForName[name] = new SolidColorBrush(color);
            PlayerViewModel.ColorsForName[name].Freeze();
        }
        
        private void Place(object args)
        {
            Game.ApplyMove(m_active.Location, m_active.TileRotation);
        }

        private bool CanPlace(object obj)
        {
            return Game.State == GameState.Place && Game.RuleSet.Applies(Game.Board, m_active.Piece, m_active.Location) && Game.RuleSet.Fits(Game.Board, m_active.Piece, m_active.Location);
        }

        private void Rotate(object obj)
        {
            m_active.TileRotation = m_active.TileRotation.RotateCw();
            UpdateActiveFits();
        }

        private bool CanRotate(object obj)
        {
            return Game.State == GameState.Place && Game.ActiveTile != NopTile.Instance;
        }

        private void Claim(object obj)
        {
            Debug.WriteLine($"GameViewModel.{nameof(Claim)}");
            Game.ApplyClaim(NopClaimable.Instance, MeepleType.None);
        }

        private bool CanClaim(object obj)
        {
            Debug.WriteLine($"GameViewModel.{nameof(CanClaim)}");
            return true;
        }

        private void ChooseMeeple(object obj)
        {
            Debug.WriteLine($"GameViewModel.{nameof(ChooseMeeple)}");
        }

        private bool CanChooseMeeple(object obj)
        {
            Debug.WriteLine($"GameViewModel.{nameof(CanChooseMeeple)}");
            return true;
        }
        
        private bool CanMove(GridCellRoutedEventArgs args) => Game.State == GameState.Place && m_active.Piece != NopTile.Instance;

        private void Move(GridCellRoutedEventArgs args)
        {
            m_active.SetCell(args.Cell);
            UpdateActiveFits();
            Debug.WriteLine($"GameViewModel.{nameof(Move)}");
        }

        private void UpdateActiveFits()
        {
            m_active.Fits = Game.RuleSet.Applies(Game.Board, m_active.Piece, m_active.Location)
                            && Game.RuleSet.Fits(Game.Board, m_active.Piece, m_active.Location);
        }
    }
}
