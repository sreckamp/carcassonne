using System;
using System.Windows;
using System.Windows.Media;
using Carcassonne.Model;
using GameBase.Model;
using GameBase.WPF.ViewModel;
using MBrush = System.Windows.Media.Brush;
using DPoint = System.Drawing.Point;
//using System.Drawing;

namespace Carcassonne.WPF.ViewModel
{
    public class PlacementViewModel : PlacementViewModel<ITile>
    {
        private const int BackgroundDepth = 99;
        private const int ForegroundDepth = 50;

        private static readonly ClaimableViewModel SDefaultIClaimableModel = new ClaimableViewModel(new EdgeRegion(EdgeRegionType.Any), NopTileRegion.Instance);

        public PlacementViewModel(ITile tile, BoardViewModel boardViewModel) :
            this(new Placement<ITile>(tile, new DPoint(int.MinValue, int.MinValue)), boardViewModel)
        { }
        public PlacementViewModel(Placement<ITile> placement, BoardViewModel boardViewModel)
            : base(placement, boardViewModel)
        {
            ParseTile();
        }

        // private BoardViewModel BoardViewModel => GridManager as BoardViewModel;

        // protected override CarcassonneMove GetMove(int locationX, int locationY) =>
        //     new CarcassonneMove(locationX, locationY, TileRotation);
        //
        // protected override CarcassonneMove GetEmptyMove() => CarcassonneMove.None;

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

        public int Depth => IsBackground ? BackgroundDepth : (m_isForeground ? ForegroundDepth : 0);

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
        //private readonly MBrush m_color = new SolidColorBrush(Colors.Orange);
        //public MBrush Color { get { return m_color; } }

        //public CarcassonneMove Placement
        //{
        //    get
        //    {
        //        return (Tile?.Placement as CarcassonneMove) ?? m_placement;
        //    }
        //    set
        //    {
        //        m_placement = value;
        //        NotifyPropertyChanged("Placement");
        //    }
        //}

        //private Rotation m_rotation;
        //public Rotation TileRotation
        //{
        //    get
        //    {
        //        return (Tile?.Placement as CarcassonneMove)?.Rotation ?? m_rotation;
        //    }
        //    set
        //    {
        //        m_rotation = value;
        //        NotifyPropertyChanged("Rotation");
        //        NotifyPropertyChanged("Opacity");
        //    }
        //}

        //public void SetLocation(DPoint value)
        //{
        //    base.Location = value;
        //    NotifyPropertyChanged("Opacity");
        //}

        private ClaimableViewModel m_allDataContext = SDefaultIClaimableModel;
        public object AllDataContext => m_allDataContext;

        //IClaimableViewModel m_allShieldDataContext = s_defaultIClaimableModel;
        //public object AllShieldDataContext { get { return m_allShieldDataContext; } }

        //IClaimableViewModel m_centerFlowerDataContext = s_defaultIClaimableModel;
        //public object CenterFlowerDataContext { get { return m_centerFlowerDataContext; } }

        //IClaimableViewModel m_southWestFlowerDataContext = s_defaultIClaimableModel;
        //public object SouthWestFlowerDataContext { get { return m_southWestFlowerDataContext; } }

        //IClaimableViewModel m_southFlowerDataContext = s_defaultIClaimableModel;
        //public object SouthFlowerDataContext { get { return m_southFlowerDataContext; } }

        //IClaimableViewModel m_roadEndDataContext = s_defaultIClaimableModel;
        //public object RoadEndDataContext { get { return m_roadEndDataContext; } }

        readonly ClaimableViewModel m_tileDataContext = SDefaultIClaimableModel;
        public object TileDataContext => m_tileDataContext;

        ClaimableViewModel m_northEastWestDataContext = SDefaultIClaimableModel;
        public object NorthEastWestDataContext => m_northEastWestDataContext;

        ClaimableViewModel m_northDataContext = SDefaultIClaimableModel;
        public object NorthDataContext => m_northDataContext;

        ClaimableViewModel m_southDataContext = SDefaultIClaimableModel;
        public object SouthDataContext => m_southDataContext;

        ClaimableViewModel m_westDataContext = SDefaultIClaimableModel;
        public object WestDataContext => m_westDataContext;

        ClaimableViewModel m_eastDataContext = SDefaultIClaimableModel;
        public object EastDataContext => m_eastDataContext;

        ClaimableViewModel m_southWestDataContext = SDefaultIClaimableModel;
        public object SouthWestDataContext => m_southWestDataContext;

        ClaimableViewModel m_eastWestDataContext = SDefaultIClaimableModel;
        public object EastWestDataContext => m_eastWestDataContext;

        ClaimableViewModel m_northSouthDataContext = SDefaultIClaimableModel;
        public object NorthSouthDataContext => m_northSouthDataContext;

        readonly ClaimableViewModel m_northSouthFlowerDataContext = SDefaultIClaimableModel;
        public object NorthSouthFlowerDataContext => m_northSouthFlowerDataContext;

        ClaimableViewModel m_northEastDataContext = SDefaultIClaimableModel;
        public object NorthEastDataContext => m_northEastDataContext;

        ClaimableViewModel m_eastSouthDataContext = SDefaultIClaimableModel;
        public object EastSouthDataContext => m_eastSouthDataContext;

        private void ParseTile()
        {
            var regions = (Placement.Piece is RotatedTile rt) ? rt.RegionsNotRotated : Placement.Piece.Regions;
            foreach (var r in regions)
            {
                var cvm = new ClaimableViewModel(r, Placement.Piece.TileRegion);
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
                m_allDataContext = new ClaimableViewModel(NopEdgeRegion.Instance, Placement.Piece.TileRegion);
            }
        }
        public override string ToString()
        {
            return Placement.ToString();
        }

        private class ClaimableViewModel
        {
            private static readonly MBrush SRiverBrush = new SolidColorBrush(Colors.DarkBlue);
            private static readonly MBrush SRoadBrush = new SolidColorBrush(Colors.Khaki);
            private static readonly MBrush SCityBrush = new SolidColorBrush(Colors.SaddleBrown);

            static ClaimableViewModel()
            {
                SRiverBrush.Freeze();
                SRoadBrush.Freeze();
                SCityBrush.Freeze();
            }

            private readonly IEdgeRegion m_edge;
            private readonly ITileRegion m_tile;
            public ClaimableViewModel(IEdgeRegion edge, ITileRegion tile)
            {
                m_edge = edge;
                m_tile = tile;
                if (m_edge == null || m_tile == null)
                {
                }
                Color.Freeze();
            }

            public MBrush Color { get; } = new SolidColorBrush(Colors.Orange);

            public Visibility FullVisibility =>
                m_edge.Type != EdgeRegionType.Any || m_tile.Type != TileRegionType.None ? Visibility.Visible : Visibility.Hidden;

            public Visibility CityVisibility =>
                m_edge.Type == EdgeRegionType.City ? Visibility.Visible : Visibility.Hidden;

            public Visibility PathVisibility =>
                m_edge.Type.IsPath() ? Visibility.Visible : Visibility.Hidden;

            public Visibility RoadVisibility =>
                m_edge.Type == EdgeRegionType.Road ? Visibility.Visible : Visibility.Hidden;

            public Visibility RoadEndVisibility =>
                m_edge.Type == EdgeRegionType.Road ? Visibility.Visible : Visibility.Hidden;

            public Visibility RiverEndVisibility =>
                m_edge.Type == EdgeRegionType.River ? Visibility.Visible : Visibility.Hidden;

            public Visibility ShieldVisibility =>
                m_edge is CityEdgeRegion cer && cer.HasShield ? Visibility.Visible : Visibility.Hidden;

            public Visibility FlowerVisibility =>
                m_tile.Type == TileRegionType.Flower ? Visibility.Visible : Visibility.Hidden;

            public Visibility MonasteryVisibility =>
                m_tile.Type == TileRegionType.Monastery ? Visibility.Visible : Visibility.Hidden;

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
        }
    }
}
