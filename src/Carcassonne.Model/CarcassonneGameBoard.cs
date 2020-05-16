using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model
{
    public class CarcassonneGameBoard : GameBoard<Tile, CarcassonneMove>
    {
        public CarcassonneGameBoard(IPlaceRule<Tile, CarcassonneMove> placeRule) : base(placeRule, Tile.None)
        { }

        public override void Clear()
        {
            base.Clear();
            AvailableLocations.Add(new Point(0, 0));
        }

        protected override IEnumerable<CarcassonneMove> GetOptions(Point point)
        {
            return Enum.GetValues(typeof(Rotation)).Cast<Rotation>().Select(rot => new CarcassonneMove(point, rot));
        }

        protected override void AddAvailableLocations(Placement<Tile, CarcassonneMove> placement)
        {
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    if (Math.Abs(x) == Math.Abs(y)) continue;
                    var p = new Point(placement.Move.Location.X + x, placement.Move.Location.Y + y);
                    if (this[p] != Tile.None) continue;
                    if (!AvailableLocations.Contains(p))
                    {
                        AvailableLocations.Add(p);
                    }
                }
            }
        }

        public Tile GetNeighbor(Point point, EdgeDirection direction)
        {
            var xOffset = 0;
            var yOffset = 0;
            switch (direction)
            {
                case EdgeDirection.North:
                    yOffset = -1;
                    break;
                case EdgeDirection.South:
                    yOffset = 1;
                    break;
                case EdgeDirection.West:
                    xOffset = -1;
                    break;
                case EdgeDirection.East:
                    xOffset = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            return base[point.X + xOffset, point.Y + yOffset];
        }

        public IEnumerable<Tile> GetAllNeighbors(Point point)
        {
            var neighbors = new List<Tile>();
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    var px = point.X + x;
                    var py = point.Y + y;
                    if (x == 0 && y == 0) continue;
                    var tmp = this[px, py];
                    if (tmp != Tile.None)
                    {
                        neighbors.Add(tmp);
                    }
                }
            }
            return neighbors;
        }
    }
}
