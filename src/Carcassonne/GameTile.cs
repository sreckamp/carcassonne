using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Carcassonne.Model;

namespace Carcassonne
{
    public partial class GameTile : UserControl
    {
        private readonly List<AbstractTileRenderer> m_renderers = new List<AbstractTileRenderer>();

        public GameTile()
        {
            InitializeComponent();
            Paint += GameTile_Paint;
            Resize += GameTile_Resize;
        }

        private void GameTile_Resize(object sender, EventArgs e)
        {
            UpdateRegions();
        }

        private void GameTile_Paint(object sender, PaintEventArgs e)
        {
            if (Active)
            {
                ControlPaint.DrawBorder(
                    e.Graphics, ClientRectangle, 
                    Color.DarkBlue, 5, ButtonBorderStyle.Solid,
                    Color.DarkBlue, 5, ButtonBorderStyle.Solid,
                    Color.DarkBlue, 5, ButtonBorderStyle.Solid,
                    Color.DarkBlue, 5, ButtonBorderStyle.Solid);
            }
        }

        private Tile m_tile = Tile.None;
        public Tile Tile
        {
            get => m_tile;
            set
            {
                m_tile = value;
                UpdateRegions();
            }
        }

        public Point GridLocation { get; set; }

        private bool m_active;
        public bool Active
        {
            get => m_active;
            set
            {
                m_active = value;
                Invalidate();
            }
        }

        public void UpdateRegions()
        {
            var canvas = new Rectangle(Padding.Left, Padding.Top,
                Size.Width - Padding.Left - Padding.Right,
                Size.Height - Padding.Top - Padding.Bottom);
            m_renderers.Clear();
            if (Tile != Tile.None)
            {
                m_renderers.Add(new BackgroundRenderer(canvas, Color.DarkGreen));
                var done = new List<EdgeDirection>();
                foreach (EdgeDirection dir in Enum.GetValues(typeof(EdgeDirection)))
                {
                    if (done.Contains(dir)) continue;
                    var r = Tile.GetRegion(dir);
                    if (r.Type != RegionType.River) continue;
                    m_renderers.Add(new RiverRegionRenderer(canvas, r, 40));
                    done.AddRange(r.Edges);
                }
                foreach (EdgeDirection dir in Enum.GetValues(typeof(EdgeDirection)))
                {
                    if (done.Contains(dir)) continue;
                    var r = Tile.GetRegion(dir);
                    if (r.Type != RegionType.Road) continue;
                    m_renderers.Add(new RoadRegionRenderer(canvas, r, 40));
                    done.AddRange(r.Edges);
                }
                foreach (EdgeDirection dir in Enum.GetValues(typeof(EdgeDirection)))
                {
                    if (done.Contains(dir)) continue;
                    var r = Tile.GetRegion(dir) as CityEdgeRegion;
                    if (r.Type != RegionType.City) continue;
                    m_renderers.Add(new CityRegionRenderer(canvas, r));
                    done.AddRange(r.Edges);
                }
                if (Tile.TileRegion != null)
                {
                    switch (Tile.TileRegion.Type)
                    {
                        case TileRegionType.Monastery:
                            m_renderers.Add(new MonasteryRegionRenderer(canvas, Tile));
                            break;
                        case TileRegionType.Flower:
                            m_renderers.Add(new FlowerRegionRenderer(canvas, Tile));
                            break;
                    }
                }
            }
            else
            {
                m_renderers.Add(new BackgroundRenderer(canvas, Color.Gainsboro));
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            foreach (var r in m_renderers)
            {
                r.Draw(e.Graphics);
            }
        }

        private class BackgroundRenderer:AbstractTileRenderer
        {
            private readonly Color m_color;

            public BackgroundRenderer(RectangleF canvas, Color color)
                : base(canvas, null)
            {
                m_color = color;
                UpdateRegion(canvas);
            }

            public override void UpdateRegion(RectangleF canvas)
            {
                if (m_color != Color.Empty)
                {
                    AddRectangle(canvas, m_color, new RectangleF(0f, 0f, 1f, 1f), Rotation.None);
                }
            }
        }

        private abstract class AbstractTileRenderer
        {
            protected const float PathWidth = 0.25f;
            protected const float SinglePathWidth = 0.35f;
            protected const float SingleRegionWidth = (1f-SinglePathWidth)/4f;

            protected EdgeRegion Region;
            protected Dictionary<PointF[], Brush> Shapes = new Dictionary<PointF[],Brush>();
            protected Dictionary<RectangleF, Brush> Ellipses = new Dictionary<RectangleF, Brush>();

            protected AbstractTileRenderer(RectangleF canvas, EdgeRegion region)
            {
                Region = region;
                UpdateRegion(canvas);
            }

            public abstract void UpdateRegion(RectangleF canvas);
            public virtual void Draw(Graphics g)
            {
                foreach (var kvp in Shapes)
                {
                    g.FillPolygon(kvp.Value, kvp.Key);
                }
                foreach (var kvp in Ellipses)
                {
                    g.FillEllipse(kvp.Value, kvp.Key);
                }
            }

            protected void AddEllipse(RectangleF canvas, Color color, RectangleF rect, Rotation rotation)
            {
                var scaled = new RectangleF();
                switch (rotation)
                {
                    case Rotation.None:
                        scaled = new RectangleF(canvas.X + rect.X * canvas.Width,
                                    canvas.Y + rect.Y * canvas.Height,
                                    rect.Width * canvas.Width, rect.Height * canvas.Height);
                        break;
                    case Rotation.CounterClockwise:
                        scaled = new RectangleF(canvas.X + rect.Y * canvas.Width,
                                    canvas.Y + (1 - rect.X - rect.Width) * canvas.Height,
                                    rect.Height * canvas.Width, rect.Width * canvas.Height);
                        break;
                    case Rotation.UpsideDown:
                        scaled = new RectangleF(canvas.X + (1 - rect.X - rect.Width) * canvas.Width,
                                    canvas.Y + (1 - rect.Y - rect.Height) * canvas.Height,
                                    rect.Width * canvas.Width, rect.Height * canvas.Height);
                        break;
                    case Rotation.Clockwise:
                        scaled = new RectangleF(canvas.X + (1 - rect.Y - rect.Height) * canvas.Width,
                                    canvas.Y + rect.X * canvas.Height,
                                    rect.Height * canvas.Width, rect.Width * canvas.Height);
                        break;
                }
                Ellipses.Add(scaled, new SolidBrush(color));
            }

            protected void AddRectangle(RectangleF canvas, Color color, RectangleF rect, Rotation rotation)
            {
                var scaled = new RectangleF();
                switch (rotation)
                {
                    case Rotation.None:
                        scaled = new RectangleF(canvas.X + rect.X * canvas.Width,
                                    canvas.Y + rect.Y * canvas.Height,
                                    rect.Width * canvas.Width, rect.Height * canvas.Height);
                        break;
                    case Rotation.CounterClockwise:
                        scaled = new RectangleF(canvas.X + rect.Y * canvas.Width,
                                    canvas.Y + (1 - rect.X - rect.Width) * canvas.Height,
                                    rect.Height * canvas.Width, rect.Width * canvas.Height);
                        break;
                    case Rotation.UpsideDown:
                        scaled = new RectangleF(canvas.X + (1 - rect.X - rect.Width) * canvas.Width,
                                    canvas.Y + (1 - rect.Y - rect.Height) * canvas.Height,
                                    rect.Width * canvas.Width, rect.Height * canvas.Height);
                        break;
                    case Rotation.Clockwise:
                        scaled = new RectangleF(canvas.X + (1 - rect.Y - rect.Height) * canvas.Width,
                                    canvas.Y + rect.X * canvas.Height,
                                    rect.Height * canvas.Width, rect.Width * canvas.Height);
                        break;
                }
                var points = new[]
                {
                        scaled.Location,
                        new PointF(scaled.Right, scaled.Top),
                        new PointF(scaled.Right, scaled.Bottom),
                        new PointF(scaled.Left, scaled.Bottom),
                };
                Shapes.Add(points, new SolidBrush(color));
            }

            protected void CreatePathRegion(RectangleF canvas, Color color)
            {
                if (Region.Edges.Length == 1)
                {
                    var rot = Rotation.None;

                    switch (Region.Edges[0])
                    {
                        case EdgeDirection.North:
                            break;
                        case EdgeDirection.East:
                            rot = Rotation.Clockwise;
                            break;
                        case EdgeDirection.South:
                            rot = Rotation.UpsideDown;
                            break;
                        case EdgeDirection.West:
                            rot = Rotation.CounterClockwise;
                            break;
                    }
                    AddRectangle(canvas, color, new RectangleF((1 - PathWidth) / 2, 0, PathWidth, SinglePathWidth), rot);
                    AddRectangle(canvas, Color.Black, new RectangleF(SinglePathWidth, SinglePathWidth, 1 - 2 * SinglePathWidth, 0.5f - SinglePathWidth), rot);
                }
                else
                {
                    var lowX = (1 - PathWidth) / 2 * canvas.Width + canvas.X;
                    var highX = (1 + PathWidth) / 2 * canvas.Width + canvas.X;
                    var lowY = (1 - PathWidth) / 2 * canvas.Height + canvas.Y;
                    var highY = (1 + PathWidth) / 2 * canvas.Height + canvas.Y;

                    var pts = new List<PointF>();
                    foreach (var e in Region.Edges)
                    {
                        switch (e)
                        {
                            case EdgeDirection.North:
                                pts.Add(new PointF(lowX, canvas.Top));
                                pts.Add(new PointF(highX, canvas.Top));
                                break;
                            case EdgeDirection.East:
                                pts.Add(new PointF(canvas.Right, lowY));
                                pts.Add(new PointF(canvas.Right, highY));
                                break;
                            case EdgeDirection.South:
                                pts.Add(new PointF(highX, canvas.Bottom));
                                pts.Add(new PointF(lowX, canvas.Bottom));
                                break;
                            case EdgeDirection.West:
                                pts.Add(new PointF(canvas.Left, highY));
                                pts.Add(new PointF(canvas.Left, lowY));
                                break;
                        }
                    }
                    var points = pts.ToArray();
                    Shapes.Add(points, new SolidBrush(color));
                }
            }

            protected void CreateSolidRegion(RectangleF canvas, Color color)
            {
                var pts = new List<PointF>();
                var tl = canvas.Location;
                var tr = new PointF(canvas.Right, canvas.Top);
                var bl = new PointF(canvas.Left, canvas.Bottom);
                var br = new PointF(canvas.Right, canvas.Bottom);
                if (Region.Edges.Length == 1)
                {
                    switch (Region.Edges[0])
                    {
                        case EdgeDirection.North:
                            pts.Add(tl);
                            InsertExtraPoint(pts, tl, tr, canvas);
                            break;
                        case EdgeDirection.East:
                            pts.Add(tr);
                            InsertExtraPoint(pts, tr, br, canvas);
                            break;
                        case EdgeDirection.South:
                            pts.Add(bl);
                            InsertExtraPoint(pts, bl, br, canvas);
                            break;
                        case EdgeDirection.West:
                            pts.Add(tl);
                            InsertExtraPoint(pts, tl, bl, canvas);
                            break;
                    }
                }
                else
                {
                    PointF? firstPt = null;
                    PointF? lastPt = null;
                    foreach (var e in Region.Edges)
                    {
                        var pt1 = new PointF();
                        var pt2 = new PointF();
                        switch (e)
                        {
                            case EdgeDirection.North:
                                pt1 = tl;
                                pt2 = tr;
                                break;
                            case EdgeDirection.East:
                                pt1 = tr;
                                pt2 = br;
                                break;
                            case EdgeDirection.South:
                                pt1 = br;
                                pt2 = bl;
                                break;
                            case EdgeDirection.West:
                                pt1 = bl;
                                pt2 = tl;
                                break;
                        }
                        InsertExtraPoint(pts, lastPt, pt1, canvas);
                        pts.Add(pt2);
                        lastPt = pt2;
                        firstPt ??= pt1;
                    }
                    if (firstPt != null && lastPt != null && firstPt != lastPt)
                    {
                        InsertExtraPoint(pts, lastPt, (PointF)firstPt, canvas);
                    }
                }
                var points = pts.ToArray();
                Shapes.Add(points, new SolidBrush(color));
            }

            private void InsertExtraPoint(List<PointF> points, PointF? lastNullable, PointF next, RectangleF canvas)
            {
                if (lastNullable == null)
                {
                    points.Add(next);
                }
                else
                {
                    var last = (PointF)lastNullable;
                    if (Math.Abs(last.X - next.X) > 0.5f)
                    {
                        if (Math.Abs(last.Y - next.Y) < 0.5f)
                        {
                            for (var i = 1; i < 10; i++)
                            {
                                float x;
                                if (last.X < next.X)
                                {
                                    x = last.X + 0.1f * canvas.Width * i;
                                }
                                else
                                {
                                    x = last.X - 0.1f * canvas.Width * i;
                                }
                                var y = GetCirclePoint(x, canvas);
                                if (next.Y > canvas.Y)
                                {
                                    y = canvas.Bottom - y;
                                }
                                points.Add(new PointF(x, y));
                            }
//                            points.Add(next);
                        }
                    }
                    else if ((int)last.Y != (int)next.Y)
                    {
                        if ((int)last.X == (int)next.X)
                        {
                            for (var i = 1; i < 10; i++)
                            {
                                float y;
                                if (last.Y < next.Y)
                                {
                                    y = last.Y + 0.1f * canvas.Height * i;
                                }
                                else
                                {
                                    y = last.Y - 0.1f * canvas.Height * i;
                                }
                                var x = GetCirclePoint(y, canvas);
                                if (next.X > canvas.X)
                                {
                                    x = canvas.Right - x;
                                }
                                points.Add(new PointF(x, y));
                            }
//                            points.Add(next);
                        }
                    }
                    points.Add(next);
                }
            }

            private static float GetCirclePoint(float x, RectangleF canvas)
            {
                var h = canvas.Width / 2 + canvas.X;
                var d = SingleRegionWidth * canvas.Height + canvas.Y;
                var k = (float)(Math.Pow(d, 2) - Math.Pow(h, 2)) / (2 * d);
                var r = d - k;
                var y = (float)Math.Sqrt(r * r - (float)Math.Pow(x - h,2)) + k;
                return y;
            }
        }

        private class RoadRegionRenderer : AbstractTileRenderer
        {
            public RoadRegionRenderer(Rectangle canvas, EdgeRegion region, float roadWidth)
                : base(canvas, region)
            {
                RoadWidth = roadWidth;
            }

            public float RoadWidth { get; }

            public override void UpdateRegion(RectangleF canvas)
            {
                CreatePathRegion(canvas, Color.DimGray);
            }
        }

        private class RiverRegionRenderer : AbstractTileRenderer
        {
            public RiverRegionRenderer(Rectangle canvas, EdgeRegion region, float riverWidth)
                : base(canvas, region)
            {
                RiverWidth = riverWidth;
            }

            public float RiverWidth { get; }

            public override void UpdateRegion(RectangleF canvas)
            {
                CreatePathRegion(canvas, Color.Blue);
                if (Region.Edges.Length == 1)
                {
                    AddEllipse(canvas, Color.Blue, new RectangleF(0.25f, 0.25f, 0.5f, 0.5f), Rotation.None);
                }
            }
        }

        private class CityRegionRenderer : AbstractTileRenderer
        {
            private readonly PointF m_shieldPos = new PointF(0.125f, 0.125f);
            private readonly SizeF m_shieldSize = new SizeF(0.1f,0.175f);
            public CityRegionRenderer(RectangleF canvas, CityEdgeRegion region)
                : base(canvas, region)
            {
            }

            private CityEdgeRegion TypedRegion => Region as CityEdgeRegion;

            public override void UpdateRegion(RectangleF region)
            {
                CreateSolidRegion(region, Color.SaddleBrown);
                if (TypedRegion.HasShield)
                {
//                    AddRectangle(region, Color.DarkBlue, new RectangleF(SHIELD_POS, SHIELD_SIZE),
//                        m_region.Parent.Rotation);
                }
            }
        }

        private class MonasteryRegionRenderer : AbstractTileRenderer
        {
            private const float MonasterySize = 0.4f;
            private readonly Tile m_parent;

            public MonasteryRegionRenderer(RectangleF canvas, Tile parent)
                : base(canvas, null)
            {
                m_parent = parent;
                UpdateRegion(canvas);
            }

            public override void UpdateRegion(RectangleF canvas)
            {
                if (m_parent != null)
                {
                    var x = (1 - MonasterySize) / 2;
                    var y = x;
//                    AddRectangle(canvas, Color.Red, new RectangleF(x, y, MONESTARY_SIZE, MONESTARY_SIZE),
//                            m_parent.Rotation);
                    var crossX = x + MonasterySize / 2 - MonasterySize / 16;
                    var crossY = y + MonasterySize / 4;
//                    AddRectangle(canvas, Color.Black, new RectangleF(crossX, crossY, MONESTARY_SIZE / 8, MONESTARY_SIZE / 2),
//                            m_parent.Rotation);
                    crossX = x + MonasterySize / 2 - MonasterySize / 6;
                    crossY = y + MonasterySize / 4 + MonasterySize / 8;
//                    AddRectangle(canvas, Color.Black, new RectangleF(crossX, crossY, MONESTARY_SIZE / 3, MONESTARY_SIZE / 8),
//                            m_parent.Rotation);
                }
            }
        }

        private class FlowerRegionRenderer : AbstractTileRenderer
        {
            private const float FlowerOffsetSingle = 0.05f;
            private const float FlowerOffsetDual = 0.175f;
            private const float FlowerSize = 0.15f;
            private readonly Tile m_parent;

            public FlowerRegionRenderer(RectangleF canvas, Tile parent)
                : base(canvas, null)
            {
                m_parent = parent;
                UpdateRegion(canvas);
            }

            public override void UpdateRegion(RectangleF canvas)
            {
                if (m_parent != null)
                {
                    var grass = new List<EdgeDirection>();
                    float x, y;
                    x = y = 0.4f;
                    foreach (EdgeDirection dir in Enum.GetValues(typeof(EdgeDirection)))
                    {
                        if (m_parent.GetEdge(dir) == RegionType.Grass)
                        {
                            grass.Add(dir);
                        }
                    }
                    if (grass.Count == 1)
                    {
                        switch (grass[0])
                        {
                            case EdgeDirection.North:
                                y = FlowerOffsetSingle;
                                break;
                            case EdgeDirection.East:
                                x = 1 - FlowerOffsetSingle - FlowerSize;
                                break;
                            case EdgeDirection.South:
                                y = 1 - FlowerOffsetSingle - FlowerSize;
                                break;
                            case EdgeDirection.West:
                                x = FlowerOffsetSingle;
                                break;
                        }
                    }
                    else if (grass.Count == 2)
                    {
                        var min = (EdgeDirection)Math.Min((int)grass[0], (int)grass[1]);
                        var max = (EdgeDirection)Math.Max((int)grass[0], (int)grass[1]);
                        if (min == EdgeDirection.North && max == EdgeDirection.East)
                        {
                            x = 1 - FlowerSize - FlowerOffsetDual;
                            y = FlowerOffsetDual;
                        }
                        else if (min == EdgeDirection.North && max == EdgeDirection.West)
                        {
                            x = y = FlowerOffsetDual;
                        }
                        else if (min == EdgeDirection.East && max == EdgeDirection.South)
                        {
                            x = y = 1 - FlowerSize - FlowerOffsetDual;
                        }
                        else if (min == EdgeDirection.South && max == EdgeDirection.West)
                        {
                            x = FlowerOffsetDual;
                            y = 1 - FlowerSize - FlowerOffsetDual;
                        }
                        else if (min == EdgeDirection.North && max == EdgeDirection.South
                            && m_parent.GetRegion(EdgeDirection.West).Edges.Length == 2)
                        {
                            y = FlowerOffsetSingle;
                        }
                        else if (min == EdgeDirection.East && max == EdgeDirection.West
                            && m_parent.GetRegion(EdgeDirection.North).Edges.Length == 2)
                        {
                            x = FlowerOffsetSingle;
                        }
                    }
                    AddRectangle(canvas, Color.Yellow, new RectangleF(x, y, FlowerSize, FlowerSize),
                            Rotation.None);
                }
            }
        }

    }
}