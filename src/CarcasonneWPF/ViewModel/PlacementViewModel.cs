using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Carcassonne.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GameBase.Model;
using GameBase.WPF.ViewModel;
using MBrush = System.Windows.Media.Brush;
using DPoint = System.Drawing.Point;
//using System.Drawing;

namespace Carcassonne.WPF.ViewModel
{
    public class PlacementViewModel : PlacementViewModel<ITile>
    {
        private const int BackgroundDepth = 10;
        private const int ForegroundDepth = 50;
        private const int DefaultDepth = 25;

        private static readonly TileFeatureViewModel SDefaultIClaimableModel = new TileFeatureViewModel(null, new EdgeRegion(EdgeRegionType.Any), NopTileRegion.Instance);

        private TileFeatureViewModel? m_overFeature = null;

        public PlacementViewModel(ITile tile, BoardViewModel boardViewModel) :
            this(new Placement<ITile>(tile, new DPoint(int.MinValue, int.MinValue)), boardViewModel)
        { }

        public PlacementViewModel(Placement<ITile> placement, BoardViewModel boardViewModel)
            : base(placement, boardViewModel)
        {
            ParseTile();
            ClearCommand = new RelayCommand(Clear, CanClear);
        }

        public ICommand ClearCommand { get; set; }

        private bool CanClear() => true;

        private void Clear()
        {
            if (m_overFeature != null)
            {
                m_overFeature.SetMeeple(null);
                m_overFeature = null;
                Debug.WriteLine("Over Grass");
            }
        }

        private void EnterFeature(TileFeatureViewModel feature)
        {
            if (m_overFeature != feature)
            {
                m_overFeature = feature;
                m_overFeature.SetMeeple(m_meeple);
                Debug.WriteLine($"Enter:{feature}");
            }
        }

        public override void SetCell(DPoint cell)
        {
            base.SetCell(cell);
            NotifyPropertyChanged(nameof(Opacity));
        }

        private bool m_fits;
        public bool Fits { get => m_fits;
            set
            {
                m_fits = value;
                NotifyPropertyChanged(nameof(Opacity));
            }
        }

        public double Opacity => !IsForeground || Fits ? 1 : 0.5;

        // public void ChangedDepth() => NotifyPropertyChanged(nameof(Depth));

        protected bool m_isBackground;
        public bool IsBackground => m_isBackground;

        private bool m_isForeground;
        public bool IsForeground
        {
            get => m_isForeground;
            set
            {
                m_isForeground = value;
                NotifyPropertyChanged(nameof(Depth));
            }
        }

        public int Depth => IsBackground ? BackgroundDepth : (m_isForeground ? ForegroundDepth : DefaultDepth);

        public Rotation TileRotation
        {
            get => Placement.Piece is RotatedTile rt ? rt.Rotation : Rotation.None;
            set
            {
                if (!(Placement.Piece is RotatedTile rt))
                {
                    rt = new RotatedTile(Placement.Piece, Rotation.None);
                    Placement.Piece = rt;
                }
                rt.Rotation = value;
                NotifyPropertyChanged(nameof(RotationAngle));
                NotifyPropertyChanged(nameof(NegRotationAngle));
                NotifyPropertyChanged(nameof(Opacity));
            }
        }

        public float RotationAngle => TileRotation.ToDegrees();
        public float NegRotationAngle => -RotationAngle;

        public string Name => Placement.Piece.ToString();

        private TileFeatureViewModel m_allDataContext = SDefaultIClaimableModel;
        public object AllDataContext => m_allDataContext;

        readonly TileFeatureViewModel m_tileDataContext = SDefaultIClaimableModel;
        public object TileDataContext => m_tileDataContext;

        TileFeatureViewModel m_northEastWestDataContext = SDefaultIClaimableModel;
        public object NorthEastWestDataContext => m_northEastWestDataContext;

        TileFeatureViewModel m_northDataContext = SDefaultIClaimableModel;
        public object NorthDataContext => m_northDataContext;

        TileFeatureViewModel m_southDataContext = SDefaultIClaimableModel;
        public object SouthDataContext => m_southDataContext;

        TileFeatureViewModel m_westDataContext = SDefaultIClaimableModel;
        public object WestDataContext => m_westDataContext;

        TileFeatureViewModel m_eastDataContext = SDefaultIClaimableModel;
        public object EastDataContext => m_eastDataContext;

        TileFeatureViewModel m_southWestDataContext = SDefaultIClaimableModel;
        public object SouthWestDataContext => m_southWestDataContext;

        TileFeatureViewModel m_eastWestDataContext = SDefaultIClaimableModel;
        public object EastWestDataContext => m_eastWestDataContext;

        TileFeatureViewModel m_northSouthDataContext = SDefaultIClaimableModel;
        public object NorthSouthDataContext => m_northSouthDataContext;

        readonly TileFeatureViewModel m_northSouthFlowerDataContext = SDefaultIClaimableModel;
        public object NorthSouthFlowerDataContext => m_northSouthFlowerDataContext;

        TileFeatureViewModel m_northEastDataContext = SDefaultIClaimableModel;
        public object NorthEastDataContext => m_northEastDataContext;

        TileFeatureViewModel m_eastSouthDataContext = SDefaultIClaimableModel;
        public object EastSouthDataContext => m_eastSouthDataContext;

        private void ParseTile()
        {
            var regions = (Placement.Piece is RotatedTile rt) ? rt.RegionsNotRotated : Placement.Piece.Regions;
            foreach (var r in regions)
            {
                var cvm = new TileFeatureViewModel(this, r, Placement.Piece.TileRegion);
                switch (r.Edges.Count)
                {
                    case 1:
                        switch (r.Edges[0])
                        {
                            case EdgeDirection.North:
                                m_northDataContext = cvm;
                                break;
                            case EdgeDirection.South:
                                m_southDataContext = cvm;
                                break;
                            case EdgeDirection.West:
                                m_westDataContext = cvm;
                                break;
                            case EdgeDirection.East:
                                m_eastDataContext = cvm;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case 2 when r.Edges.Contains(EdgeDirection.South)
                                && r.Edges.Contains(EdgeDirection.West):
                        m_southWestDataContext = cvm;
                        break;
                    case 2 when r.Edges.Contains(EdgeDirection.East)
                                && r.Edges.Contains(EdgeDirection.West):
                        m_eastWestDataContext = cvm;
                        break;
                    case 2 when r.Edges.Contains(EdgeDirection.North)
                                && r.Edges.Contains(EdgeDirection.South):
                        m_northSouthDataContext = cvm;
                        break;
                    case 2 when r.Edges.Contains(EdgeDirection.North)
                                && r.Edges.Contains(EdgeDirection.East):
                        m_northEastDataContext = cvm;
                        break;
                    case 2:
                    {
                        if (r.Edges.Contains(EdgeDirection.East)
                            && r.Edges.Contains(EdgeDirection.South))
                        {
                            m_eastSouthDataContext = cvm;
                        }

                        break;
                    }
                    case 3:
                        m_northEastWestDataContext = cvm;
                        break;
                    case 4:
                        m_allDataContext = cvm;
                        //m_northEastWestDataContext = cvm;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (m_allDataContext == SDefaultIClaimableModel)
            {
                m_allDataContext = new TileFeatureViewModel(this, NopEdgeRegion.Instance, Placement.Piece.TileRegion);
            }
        }

        private MeepleViewModel? m_meeple;
        public void SetMeeple(MeepleViewModel meeple)
        {
            m_meeple = meeple;
        }

        public override string ToString()
        {
            return Placement.ToString();
        }

        private class TileFeatureViewModel : INotifyPropertyChanged
        {
            private static readonly MBrush SRiverBrush = new SolidColorBrush(Colors.DarkBlue);
            private static readonly MBrush SRoadBrush = new SolidColorBrush(Colors.Khaki);
            private static readonly MBrush SCityBrush = new SolidColorBrush(Colors.SaddleBrown);

            static TileFeatureViewModel()
            {
                SRiverBrush.Freeze();
                SRoadBrush.Freeze();
                SCityBrush.Freeze();
            }

            private readonly PlacementViewModel m_parent;
            private readonly IEdgeRegion m_edge;
            private readonly ITileRegion m_tile;
            public TileFeatureViewModel(PlacementViewModel parent, IEdgeRegion edge, ITileRegion tile)
            {
                m_parent = parent;
                m_edge = edge;
                m_tile = tile;
                // Color.Freeze();
                MoveCommand = new RelayCommand(RegionMove, CanMove);
            }

            public ICommand MoveCommand { get; set; }

            private bool CanMove() => m_parent != null;

            private void RegionMove()
            {
                m_parent.EnterFeature(this);
            }

            private MeepleViewModel? m_meeple = null;

            public void SetMeeple(MeepleViewModel meeple)
            {
                m_meeple = meeple;
                NotifyPropertyChanged(nameof(CityMeeple));
                NotifyPropertyChanged(nameof(CityMeepleVisibility));
                NotifyPropertyChanged(nameof(RoadMeeple));
                NotifyPropertyChanged(nameof(RoadMeepleVisibility));
                NotifyPropertyChanged(nameof(FlowerMeeple));
                NotifyPropertyChanged(nameof(FlowerMeepleVisibility));
                NotifyPropertyChanged(nameof(MonasteryMeeple));
                NotifyPropertyChanged(nameof(MonasteryMeepleVisibility));
            }

            public Visibility FullVisibility =>
                m_edge.Type != EdgeRegionType.Any || m_tile.Type != TileRegionType.None ? Visibility.Visible : Visibility.Hidden;

            private bool IsCity => m_edge.Type == EdgeRegionType.City;

            public Visibility CityVisibility => IsCity.ToVisibility();

            public Visibility ShieldVisibility => (m_edge is CityEdgeRegion cer && cer.HasShield).ToVisibility();

            public MeepleViewModel CityMeeple => IsCity ? m_meeple : null;

            public Visibility CityMeepleVisibility => (IsCity && m_meeple != null).ToVisibility();

            public Visibility PathVisibility => m_edge.Type.IsPath().ToVisibility();

            private bool IsRoad => m_edge.Type == EdgeRegionType.Road;

            public Visibility RoadVisibility => IsRoad.ToVisibility();

            // public Visibility RoadEndVisibility => IsRoad.ToVisibility();

            public MeepleViewModel RoadMeeple => IsCity ? m_meeple : null;

            public Visibility RoadMeepleVisibility => (IsCity && m_meeple != null).ToVisibility();

            public Visibility RiverVisibility => (m_edge.Type == EdgeRegionType.River).ToVisibility();

            private bool IsFlower => m_tile.Type == TileRegionType.Flower;

            public Visibility FlowerVisibility => IsFlower.ToVisibility();

            public MeepleViewModel FlowerMeeple => IsFlower ? m_meeple : null;

            public Visibility FlowerMeepleVisibility => (IsFlower && m_meeple != null).ToVisibility();

            private bool IsMonastary => m_tile.Type == TileRegionType.Monastery;

            public Visibility MonasteryVisibility => IsMonastary.ToVisibility();

            public MeepleViewModel MonasteryMeeple => IsMonastary ? m_meeple : null;

            public Visibility MonasteryMeepleVisibility => (IsMonastary && m_meeple != null).ToVisibility();

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            #endregion

            public MBrush Fill
            {
                get
                {
                    return m_edge.Type switch
                    {
                        EdgeRegionType.City => SCityBrush,
                        EdgeRegionType.River => SRiverBrush,
                        EdgeRegionType.Road => SRoadBrush,
                        _ => null
                    };
                }
            }

            public override string ToString()
            {
                var text = new StringBuilder();

                if (m_edge.Type != EdgeRegionType.Any && m_edge.Type != EdgeRegionType.None)
                {
                    text.Append(m_edge);
                }

                if (m_tile.Type == TileRegionType.None) return text.ToString();
                if (text.Length > 0)
                {
                    text.Append('/');
                }

                text.Append(m_tile.Type);
                return text.ToString();
            }
        }
    }
}
