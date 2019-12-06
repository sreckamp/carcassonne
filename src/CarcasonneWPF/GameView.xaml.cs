using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Carcassonne.Model;
using System.ComponentModel;
using System.Collections.Specialized;
using Carcassonne.Model.Expansions;
using Carcassonne.WPF.ViewModel;

namespace Carcassonne.WPF
{
    /// <summary>
    /// Interaction logic for CarcasonneWindow.xaml
    /// </summary>
    public partial class GameView : Window
    {
        public TileView ActiveTileView { get; private set; }
        //private readonly Game m_game = new Game();
        private readonly PropertyChangedEventHandler m_propChanged;
        private readonly BackgroundWorker m_worker = new BackgroundWorker();
        private readonly GameViewModel m_gameVm;

        public GameView()
        {
            var args = Environment.GetCommandLineArgs();
            var packs = new List<Type>();
            foreach (var a in args)
            {
                if ("abbot".Equals(a.ToLower().Trim()))
                {
                    packs.Add(typeof(AbbotExpansion));
                }
                if ("river".Equals(a.ToLower().Trim()))
                {
                    packs.Add(typeof(RiverExpansion));
                }
                if ("farmer".Equals(a.ToLower().Trim()))
                {
                    packs.Add(typeof(FarmerExpansion));
                }
            }
            var exp = new AbstractExpansionPack[packs.Count];
            for (var i = 0; i < exp.Length; i++)
            {
                exp[i] = (AbstractExpansionPack)packs[i].GetConstructor(new Type[0]).Invoke(new object[0]);
            }
            m_gameVm = new GameViewModel(exp);
            m_propChanged = activeTile_PropertyChanged;
//            m_gameVm.PropertyChanging += new PropertyChangingEventHandler(m_game_PropertyChanging);
            m_gameVm.PropertyChanged += m_game_PropertyChanged;
            InitializeComponent();
//            m_gameVm.Game.PointRegions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PointRegions_CollectionChanged);
            m_worker.DoWork += m_worker_DoWork;
            m_worker.RunWorkerCompleted += m_worker_RunWorkerCompleted;

            m_gameVm.AddPlayer("Luke", Colors.Blue);
            m_gameVm.AddPlayer("Vader", Colors.Red);

            DataContext = m_gameVm;
            m_worker.RunWorkerAsync();
            //while (m_gameVm.Game.m_deck.Count > 0)
            //{
            //    Deck.Children.Add(new TileViewModel(m_gameVm.Game.Board, m_gameVm.Game.m_deck.Pop()).View);
            //}
        }

        private void pointRegions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Dispatcher.CheckAccess())
            {
                Regions.Items.Clear();
                foreach (var r in m_gameVm.Game.PointRegions)
                {
                    Regions.Items.Add(r);
                }
            }
            else
            {
                Dispatcher.Invoke(new Action<object, NotifyCollectionChangedEventArgs>(pointRegions_CollectionChanged), sender, e);
            }
        }

        private void m_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            m_gameVm.Game.Play();
        }

        private void m_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Game Over!");
        }

        void m_game_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            //if (Dispatcher.CheckAccess())
            //{
            //    if (m_gameVm.Game.ActiveTile != null)
            //    {
            //        if ("ActiveTile".Equals(e.PropertyName))
            //        {
            //            if (m_gameVm.Game.ActiveTile != null)
            //            {
            //                //m_gameVm.Game.ActiveTile.PropertyChanged -= m_propChanged;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    Dispatcher.Invoke(new Action<object, PropertyChangingEventArgs>(
            //        (_sender, _e) => { m_game_PropertyChanging(_sender, _e); }), sender, e);
            //}
        }

        private void m_game_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (Dispatcher.CheckAccess())
            //{
            //    if (m_gameVm.Game.ActiveTile != null)
            //    {
            //        if ("ActiveTile".Equals(e.PropertyName))
            //        {
            //            if (m_gameVm.Game.ActiveTile != null)
            //            {
            //                var gsv = new TileView
            //                {
            //                    DataContext = m_gameVm.Game.ActiveTile
            //                };
            //                gsv.Width = gsv.Height = 75;
            //                ActiveTileView = gsv;
            //                //m_gameVm.Game.ActiveTile.PropertyChanged += m_propChanged;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    Dispatcher.Invoke(new Action<object, PropertyChangedEventArgs>(
            //        (_sender, _e) => { m_game_PropertyChanged(_sender, _e); }), sender, e);
            //}
        }

        //private void tileEnter(object sender, TileEnterArgs args)
        //{
        //    if (m_gameVm.Game.State == GameState.Place && ActiveTileView != null)
        //    {
        //        GameField.Hover(m_gameVm.Game.ActiveTile, args.Point);
        //        updateDimming(args.Point);
        //    }
        //}

        private bool UpdateDimming(Point location)
        {
            if (ActiveTileView != null)
            {
                //TODO: Fix this...
                //if (m_gameMv.Game.TryFit(new Move(location, ActiveTileView.Tile.Rotation)))
                //{
                //    ActiveTileView.Opacity = 1;
                //}
                //else
                //{
                //    if (ActiveTileView.Tile != null && ActiveTileView.Tile != m_gameMv.Game.ActiveTile)
                //    {
                //        return false;
                //    }
                //    ActiveTileView.Opacity = 0.5;
                //}
                //return true;
            }
            return false;
        }

        private void activeTile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("Rotation".Equals(e.PropertyName))
            {
                //updateDimming((Point)ActiveTileView.Location);
            }
        }
    }

    //public class PlayerToBrushConverter : MarkupExtension, IValueConverter
    //{
    //    public static readonly Dictionary<AbstractPlayer, Color> PlayerColors = new Dictionary<AbstractPlayer, Color>();
    //    private static readonly Dictionary<AbstractPlayer, Brush> m_playerBrush = new Dictionary<AbstractPlayer, Brush>();

    //    #region IValueConverter Members

    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        var p = value as AbstractPlayer;
    //        var b = GetBrushForPlayer(p);
    //        if (b != null) return b;
    //        return DependencyProperty.UnsetValue;
    //    }

    //    public static Brush GetBrushForPlayer(AbstractPlayer p)
    //    {
    //        if (p != null && PlayerColors.ContainsKey(p))
    //        {
    //            if (!m_playerBrush.ContainsKey(p))
    //            {
    //                m_playerBrush[p] = new SolidColorBrush(PlayerColors[p]);
    //            }
    //            return m_playerBrush[p];
    //        }
    //        return null;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion

    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return this;
    //    }
    //}
}
