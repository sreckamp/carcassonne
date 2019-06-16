using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Carcassonne.Model;
using System.ComponentModel;
using MBrush = System.Windows.Media.Brush;
using System.Windows.Shapes;
using System.Diagnostics;
using DPoint = System.Drawing.Point;
//using System.Drawing;
using System.Windows.Media;
using System.Windows;
using GameBase.WPF.ViewModel;
using GameBase.Model;
using System.Drawing;

namespace Carcassonne.WPF.ViewModel
{
    public class PlacementViewModel : AbstractPlacementViewModel<Tile, CarcassonneMove>
    {
        private static ClaimableViewModel s_defaultIClaimableModel = new ClaimableViewModel(null, null);

        public PlacementViewModel(Tile tile, GameBoardViewModel boardModel) :
            this(new Placement<Tile, CarcassonneMove>(tile, null), boardModel)
        { }
        public PlacementViewModel(Placement<Tile, CarcassonneMove> placement, IGridManager gridManager)
            : base(placement, gridManager)
        {
            if (Tile != null)
            {
                parseTile();
            }
        }

        private GameBoardViewModel m_boardViewModel => m_GridManager as GameBoardViewModel;
        protected override CarcassonneMove GetMove(int locationX, int locationY)
        {
            return new CarcassonneMove(locationX, locationY, TileRotation);
        }

        public override void SetCell(DPoint cell)
        {
            base.SetCell(cell);
            NotifyPropertyChanged(nameof(Opacity));
        }

        public double Opacity
        {
            get
            {
                if (m_boardViewModel?.Fits(this) ?? true)
                {
                    return 1;
                }
                return 0.5;
            }
        }

        public void ChangedDepth()
        {
            NotifyPropertyChanged(nameof(Depth));
        }

        public int Depth
        {
            get
            {
                if (this is PointViewModel)
                    Debug.WriteLine($"PointViewModel");
                if (m_boardViewModel?.IsBackground(this) ?? true)
                {
                    Debug.WriteLine($"{Name} Depth:150");
                    return 99;
                }
                else if (m_boardViewModel?.IsForeground(this) ?? true)
                {
                    Debug.WriteLine($"{Name} Depth:100");
                    return 50;
                }
                Debug.WriteLine($"{Name} Depth:0");
                return 0;
            }
        }

        private Rotation m_tileRotation = Rotation.None;
        public Rotation TileRotation
        {
            get => m_placement?.Move?.Rotation ?? m_tileRotation;
            set
            {
                m_tileRotation = value;
                NotifyPropertyChanged(nameof(RotationAngle));
                NotifyPropertyChanged(nameof(NegRotationAngle));
                NotifyPropertyChanged(nameof(Opacity));
            }
        }

        public float RotationAngle => m_placement?.Move?.Rotation.ToDegrees() ?? 0;
        public float NegRotationAngle => -RotationAngle;

        public string Name => Tile?.ToString() ?? "<<null>>";
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

        //public override DPoint Location
        //{
        //    get
        //    {
        //        return Tile?.Placement?.Location ?? base.Location;
        //    }
        //}

        public Tile Tile => m_placement?.Piece;

        ClaimableViewModel m_allDataContext = s_defaultIClaimableModel;
        public object AllDataContext { get { return m_allDataContext; } }

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

        ClaimableViewModel m_tileDataContext = s_defaultIClaimableModel;
        public object TileDataContext { get { return m_tileDataContext; } }

        ClaimableViewModel m_northEastWestDataContext = s_defaultIClaimableModel;
        public object NorthEastWestDataContext { get { return m_northEastWestDataContext; } }

        ClaimableViewModel m_northDataContext = s_defaultIClaimableModel;
        public object NorthDataContext { get { return m_northDataContext; } }

        ClaimableViewModel m_southDataContext = s_defaultIClaimableModel;
        public object SouthDataContext { get { return m_southDataContext; } }

        ClaimableViewModel m_westDataContext = s_defaultIClaimableModel;
        public object WestDataContext { get { return m_westDataContext; } }

        ClaimableViewModel m_eastDataContext = s_defaultIClaimableModel;
        public object EastDataContext { get { return m_eastDataContext; } }

        ClaimableViewModel m_southWestDataContext = s_defaultIClaimableModel;
        public object SouthWestDataContext { get { return m_southWestDataContext; } }

        ClaimableViewModel m_eastWestDataContext = s_defaultIClaimableModel;
        public object EastWestDataContext { get { return m_eastWestDataContext; } }

        ClaimableViewModel m_northSouthDataContext = s_defaultIClaimableModel;
        public object NorthSouthDataContext { get { return m_northSouthDataContext; } }

        ClaimableViewModel m_northSouthFlowerDataContext = s_defaultIClaimableModel;
        public object NorthSouthFlowerDataContext { get { return m_northSouthFlowerDataContext; } }

        ClaimableViewModel m_northEastDataContext = s_defaultIClaimableModel;
        public object NorthEastDataContext { get { return m_northEastDataContext; } }

        ClaimableViewModel m_eastSouthDataContext = s_defaultIClaimableModel;
        public object EastSouthDataContext { get { return m_eastSouthDataContext; } }

        private void parseTile()
        {
            foreach (var r in Tile.Regions)
            {
                ClaimableViewModel cvm = new ClaimableViewModel(r, Tile.TileRegion);
                if (r.RawEdges.Length == 1)
                {
                    switch (r.RawEdges[0])
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
                    }
                }
                else if (r.Edges.Length == 2)
                {
                    if (r.Edges.Contains(EdgeDirection.South)
                        && r.Edges.Contains(EdgeDirection.West))
                    {
                        m_southWestDataContext = cvm;
                    }
                    else if (r.Edges.Contains(EdgeDirection.East)
                        && r.Edges.Contains(EdgeDirection.West))
                    {
                        m_eastWestDataContext = cvm;
                    }
                    else if (r.Edges.Contains(EdgeDirection.North)
                        && r.Edges.Contains(EdgeDirection.South))
                    {
                        m_northSouthDataContext = cvm;
                    }
                    else if (r.Edges.Contains(EdgeDirection.North)
                        && r.Edges.Contains(EdgeDirection.East))
                    {
                        m_northEastDataContext = cvm;
                    }
                    else if (r.Edges.Contains(EdgeDirection.East)
                                && r.Edges.Contains(EdgeDirection.South))
                    {
                        m_eastSouthDataContext = cvm;
                    }
                }
                else if (r.Edges.Length == 3)
                {
                    m_northEastWestDataContext = cvm;
                }
                else if (r.Edges.Length == 4)
                {
                    m_allDataContext = cvm;
                    //m_northEastWestDataContext = cvm;
                }
            }
            if (m_allDataContext == s_defaultIClaimableModel)
            {
                m_allDataContext = new ClaimableViewModel(null, Tile.TileRegion);
            }
        }
        public override string ToString()
        {
            return m_placement?.ToString() ?? "<<null>>";
        }

        private class ClaimableViewModel
        {
            private static readonly MBrush s_riverBrush = new SolidColorBrush(Colors.DarkBlue);
            private static readonly MBrush s_roadBrush = new SolidColorBrush(Colors.Khaki);
            private static readonly MBrush s_cityBrush = new SolidColorBrush(Colors.SaddleBrown);

            private readonly EdgeRegion m_edge;
            private readonly TileRegion m_tile;
            public ClaimableViewModel(EdgeRegion edge, TileRegion tile)
            {
                m_edge = edge;
                m_tile = tile;
            }

            public Visibility FullVisibility
            {
                get
                {
                    if (m_edge != null || m_tile != null)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public Visibility CityVisibility
            {
                get
                {
                    if (m_edge?.Type == RegionType.City)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public Visibility PathVisibility
            {
                get
                {
                    if (m_edge?.Type.IsPath() ?? false)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public Visibility RoadVisibility
            {
                get
                {
                    if ((m_edge?.Type ?? RegionType.Grass) == RegionType.Road)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public Visibility RoadEndVisibility
            {
                get
                {
                    if (m_edge?.Type == RegionType.Road)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public Visibility RiverEndVisibility
            {
                get
                {
                    if (m_edge?.Type == RegionType.River)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public Visibility ShieldVisibility
            {
                get
                {
                    if (m_edge is CityEdgeRegion cer && cer.HasShield)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public Visibility FlowerVisibility
            {
                get
                {
                    if (m_tile?.Type == TileRegionType.Flower)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public Visibility MonestaryVisibility
            {
                get
                {
                    if (m_tile?.Type == TileRegionType.Monestary)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Hidden;
                }
            }

            public MBrush Fill
            {
                get
                {
                    switch (m_edge?.Type)
                    {
                        case RegionType.City:
                            return s_cityBrush;
                        case RegionType.River:
                            return s_riverBrush;
                        case RegionType.Road:
                            return s_roadBrush;
                    }
                    return null;
                }
            }
        }
    }
}
