using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;
using Carcassonne.Model;
using GameBase.Model;
using Move = Carcassonne.Model.Move;

namespace Carcassonne.WPF.ViewModel
{
    public class PlayerViewModel : IMoveProvider, IClaimProvider
    {
        public readonly static Dictionary<string, Brush> ColorsForName = new Dictionary<string, Brush>();
        private readonly object m_lock = new object();
        //private readonly Dispatcher m_dispatcher;
        //private readonly MouseButtonEventHandler m_placeLeftButtonUpHandler;
        //private readonly MouseButtonEventHandler m_placeRightButtonUpHandler;
        //private readonly MouseButtonEventHandler m_rightButtonDownHandler;
        //private readonly MouseButtonEventHandler m_claimLeftButtonUpHandler;
        //private readonly MouseButtonEventHandler m_claimRightButtonUpHandler;
        //private readonly TileEnterHandler m_tileEnterHandler;
        //private PlacementViewModel m_activeTile;
        private Move m_move;
        //private Point? m_overPoint = null;
        private readonly IClaimable m_activeClaim = null;
        //private bool m_rightPressed = false;
        private readonly Player m_player;

        public PlayerViewModel(Player player)
        {
            //m_dispatcher = Application.Current.Dispatcher;
            m_player = player;
            player.ClaimChooser = this;
            player.MoveChooser = this;
            //m_placeLeftButtonUpHandler = new MouseButtonEventHandler(GameField_LeftMouseButtonUp);
            //m_placeRightButtonUpHandler = new MouseButtonEventHandler(GameField_RightMouseButtonUp);
            //m_claimLeftButtonUpHandler = new MouseButtonEventHandler(Tile_LeftMouseButtonUp);
            //m_claimRightButtonUpHandler = new MouseButtonEventHandler(Tile_RightMouseButtonUp);
            //m_rightButtonDownHandler = new MouseButtonEventHandler(RightMouseButtonDown);
            //m_tileEnterHandler = new TileEnterHandler(GameField_TileEnter);
            MeepleViewModels = new MappingCollection<MeepleViewModel, Meeple>(player.Meeple);
        }

        public MappingCollection<MeepleViewModel, Meeple> MeepleViewModels
        {
            get;
        }

        public Brush Color => ColorsForName[m_player.Name];
        public string Name => m_player.Name;
        public int Score => m_player.Score;

        public Move GetMove(Game game)
        {
            //m_activeTile.ClickAction +=
            //m_view.GameField.MouseRightButtonUp += m_placeRightButtonUpHandler;
            //m_view.GameField.MouseLeftButtonUp += m_placeLeftButtonUpHandler;
            //m_view.GameField.MouseRightButtonDown += m_rightButtonDownHandler;
            //m_view.GameField.TileEnter += m_tileEnterHandler;
            Move mv = null;
            lock (m_lock)
            {
                Monitor.Wait(m_lock);
                mv = m_move;
//                if (m_clickPoint != null)
//                {
////                    mv = new Move((Point)m_clickPoint, game.ActiveTile.Rotation);
//                }
            }
            //m_view.GameField.TileEnter -= m_tileEnterHandler;
            //m_view.GameField.MouseLeftButtonUp -= m_placeLeftButtonUpHandler;
            //m_view.GameField.MouseRightButtonUp -= m_placeRightButtonUpHandler;
            //m_view.GameField.MouseRightButtonDown -= m_rightButtonDownHandler;
            //m_rightPressed = false;
            return mv;
        }

        internal void Place(Move move)
        {
            m_move = move;
            lock (m_lock)
            {
                Monitor.PulseAll(m_lock);
            }
        }
        public (IClaimable, MeepleType) GetClaim(Game game)
        {
            //m_view.ActiveTileView.MouseLeftButtonUp += m_claimLeftButtonUpHandler;
            //m_view.ActiveTileView.MouseRightButtonUp += m_claimRightButtonUpHandler;
            //m_view.ActiveTileView.MouseRightButtonDown += m_rightButtonDownHandler;
            //m_view.ActiveTileView.ActiveMeeple = PeekMeeple(type);
            //if (m_view.ActiveTileView.ActiveMeeple != null)
            //{
            lock (m_lock)
            {
                Monitor.Wait(m_lock);
            }
            //    type = m_view.ActiveTileView.ActiveMeeple.Type;
            //}
            //m_view.ActiveTileView.ActiveMeeple = null;
            //m_view.ActiveTileView.MouseRightButtonUp -= m_claimRightButtonUpHandler;
            //m_view.ActiveTileView.MouseLeftButtonUp -= m_claimLeftButtonUpHandler;
            //m_view.ActiveTileView.MouseRightButtonDown -= m_rightButtonDownHandler;
            //m_rightPressed = false;
            return (m_activeClaim, MeepleType.Meeple);
        }

        //private void GameField_LeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.RightButton == MouseButtonState.Pressed)
        //    {
        //        m_clickPoint = null;
        //    }
        //    else
        //    {
        //        m_clickPoint = m_overPoint;
        //    }
        //    lock (m_lock)
        //    {
        //        Monitor.PulseAll(m_lock);
        //    }
        //}

        //private void GameField_RightMouseButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (m_rightPressed && m_view.ActiveTileView != null)
        //    {
        //        var t = m_view.ActiveTileView.DataContext as Tile;
        //        if (t != null)
        //        {
        //            //TODO rotate
        //        }
        //    }
        //}

        //private void Tile_LeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.RightButton == MouseButtonState.Pressed)
        //    {
        //        m_activeClaim = null;
        //    }
        //    else
        //    {
        //        //m_activeClaim = ((TileView)sender).ActiveClaim;
        //    }
        //    lock (m_lock)
        //    {
        //        Monitor.PulseAll(m_lock);
        //    }
        //}

        //private void Tile_RightMouseButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    //if (m_rightPressed && m_view.ActiveTileView.ActiveMeeple != null)
        //    //{
        //    //    var types = AvailableMeepleType;
        //    //    int idx = types.IndexOf(m_view.ActiveTileView.ActiveMeeple.Type);
        //    //    idx++;
        //    //    if (idx >= types.Count) idx = 0;
        //    //    m_view.ActiveTileView.ActiveMeeple = PeekMeeple(types[idx]);
        //    //}
        //}

        //private void RightMouseButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    m_rightPressed = true;
        //}

        //private void GameField_TileEnter(object sender, TileEnterArgs e)
        //{
        //    m_overPoint = e.Point;
        //}

        //internal void SetActiveTile(PlacementViewModel tileViewModel)
        //{
        //    m_activeTile = tileViewModel;
        //}
    }
}
