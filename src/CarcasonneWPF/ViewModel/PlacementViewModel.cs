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

        private static readonly TileFeatureViewModel SDefaultIClaimableModel =
            new TileFeatureViewModel(null, new EdgeRegion(EdgeRegionType.Any), NopTileRegion.Instance);

        private TileFeatureViewModel? m_edgeFeature = null;
        private TileFeatureViewModel? m_tileFeature = null;

        public PlacementViewModel(ITile tile, BoardViewModel boardViewModel) :
            this(new Placement<ITile>(tile, new DPoint(int.MinValue, int.MinValue)), boardViewModel)
        {
        }

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
            if (m_edgeFeature != null || m_tileFeature != null)
            {
                Debug.WriteLine("Over Grass");
            }

            ClearMeeple();
        }

        private void EnterEdgeFeature(TileFeatureViewModel feature)
        {
            if (m_edgeFeature == feature) return;
            ClearMeeple();
            m_edgeFeature = feature;
            if (m_edgeFeature == null) return;
            m_edgeFeature.SetEdgeMeeple(m_meeple);
            Debug.WriteLine($"Enter:{feature}");
        }

        private void EnterTileFeature(TileFeatureViewModel feature)
        {
            if (m_tileFeature == feature) return;
            ClearMeeple();
            m_tileFeature = feature;
            if (m_tileFeature == null) return;
            m_tileFeature.SetTileMeeple(m_abbot);
            Debug.WriteLine($"Enter:{feature}");
        }

        private void ClearMeeple()
        {
            m_tileFeature?.SetTileMeeple(null);
            m_tileFeature = null;
            m_edgeFeature?.SetEdgeMeeple(null);
            m_edgeFeature = null;
        }

        public override void SetCell(DPoint cell)
        {
            base.SetCell(cell);
            NotifyPropertyChanged(nameof(Opacity));
        }

        private bool m_fits;

        public bool Fits
        {
            get => m_fits;
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

        public object AllDataContext { get; private set; } = SDefaultIClaimableModel;

        public object NorthEastWestDataContext { get; private set; } = SDefaultIClaimableModel;

        public object NorthDataContext { get; private set; } = SDefaultIClaimableModel;

        public object SouthDataContext { get; private set; } = SDefaultIClaimableModel;

        public object WestDataContext { get; private set; } = SDefaultIClaimableModel;

        public object EastDataContext { get; private set; } = SDefaultIClaimableModel;

        public object SouthWestDataContext { get; private set; } = SDefaultIClaimableModel;

        public object EastWestDataContext { get; private set; } = SDefaultIClaimableModel;

        public object NorthSouthDataContext { get; private set; } = SDefaultIClaimableModel;

        public object NorthEastDataContext { get; private set; } = SDefaultIClaimableModel;

        public object EastSouthDataContext { get; private set; } = SDefaultIClaimableModel;

        private void ParseTile()
        {
            var regions = (Placement.Piece is RotatedTile rt) ? rt.RegionsNotRotated : Placement.Piece.Regions;
            foreach (var r in regions)
            {
                var cvm = new TileFeatureViewModel(this, r, Placement.Piece.TileRegion);

                switch (r.Edges.Count)
                {
                    case 1 when r.Edges[0] == EdgeDirection.North:
                        NorthDataContext = cvm;
                        break;
                    case 1 when r.Edges[0] == EdgeDirection.South:
                        SouthDataContext = cvm;
                        break;
                    case 1 when r.Edges[0] == EdgeDirection.West:
                        WestDataContext = cvm;
                        break;
                    case 1 when r.Edges[0] == EdgeDirection.East:
                        EastDataContext = cvm;
                        break;
                    case 2 when r.Edges.Contains(EdgeDirection.South)
                                && r.Edges.Contains(EdgeDirection.West):
                        SouthWestDataContext = cvm;
                        break;
                    case 2 when r.Edges.Contains(EdgeDirection.East)
                                && r.Edges.Contains(EdgeDirection.West):
                        EastWestDataContext = cvm;
                        break;
                    case 2 when r.Edges.Contains(EdgeDirection.North)
                                && r.Edges.Contains(EdgeDirection.South):
                        NorthSouthDataContext = cvm;
                        break;
                    case 2 when r.Edges.Contains(EdgeDirection.North)
                                && r.Edges.Contains(EdgeDirection.East):
                        NorthEastDataContext = cvm;
                        break;
                    case 2 when (r.Edges.Contains(EdgeDirection.East)
                                 && r.Edges.Contains(EdgeDirection.South)):
                        EastSouthDataContext = cvm;
                        break;
                    case 3:
                        NorthEastWestDataContext = cvm;
                        break;
                    case 4:
                        AllDataContext = cvm;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (AllDataContext == SDefaultIClaimableModel)
            {
                AllDataContext = new TileFeatureViewModel(this, NopEdgeRegion.Instance, Placement.Piece.TileRegion);
            }
        }

        private MeepleViewModel m_meeple = new MeepleViewModel(new Meeple(MeepleType.Meeple, NopPlayer.Instance));
        private MeepleViewModel m_abbot = new MeepleViewModel(new Meeple(MeepleType.Abbot, NopPlayer.Instance));

        public void SetMeeple(MeepleViewModel meeple)
        {
            // m_meeple = meeple;
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
                EdgeMoveCommand = new RelayCommand(EdgeRegionMove, CanMove);
                TileMoveCommand = new RelayCommand(TileRegionMove, CanMove);
            }

            public ICommand EdgeMoveCommand { get; set; }

            public ICommand TileMoveCommand { get; set; }

            private bool CanMove() => m_parent != null;

            private void EdgeRegionMove()
            {
                m_parent.EnterEdgeFeature(this);
            }

            private void TileRegionMove()
            {
                m_parent.EnterTileFeature(this);
            }

            public void SetEdgeMeeple(MeepleViewModel meeple)
            {
                if (!IsCity && !IsRoad) return;
                EdgeMeeple = meeple;
                NotifyPropertyChanged(nameof(EdgeMeeple));
            }

            public void SetTileMeeple(MeepleViewModel meeple)
            {
                TileMeeple = meeple;
                NotifyPropertyChanged(nameof(TileMeeple));
            }

            public MeepleViewModel EdgeMeeple { get; private set; }

            public MeepleViewModel TileMeeple { get; private set; }

            public Visibility FullVisibility =>
                (m_edge.Type != EdgeRegionType.Any || m_tile.Type != TileRegionType.None).ToVisibility();

            private bool IsCity => m_edge.Type == EdgeRegionType.City;

            public Visibility CityVisibility => IsCity.ToVisibility();

            public Visibility ShieldVisibility => (m_edge is CityEdgeRegion cer && cer.HasShield).ToVisibility();

            public Visibility PathVisibility => m_edge.Type.IsPath().ToVisibility();

            private bool IsRoad => m_edge.Type == EdgeRegionType.Road;

            public Visibility RoadVisibility => IsRoad.ToVisibility();

            public Visibility RiverVisibility => (m_edge.Type == EdgeRegionType.River).ToVisibility();

            private bool IsFlower => m_tile.Type == TileRegionType.Flower;

            public Visibility FlowerVisibility => IsFlower.ToVisibility();

            private bool IsMonastary => m_tile.Type == TileRegionType.Monastery;

            public Visibility MonasteryVisibility => IsMonastary.ToVisibility();

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            #endregion

            public MBrush Fill => m_edge.Type switch
            {
                EdgeRegionType.City => SCityBrush,
                EdgeRegionType.River => SRiverBrush,
                EdgeRegionType.Road => SRoadBrush,
                _ => null
            };

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
