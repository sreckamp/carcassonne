using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameBase.Model;

namespace Carcassonne.Model
{
    public class Board : Board<ITile>, IGameBoard
    {
        public Board() : base(NopTile.Instance)
        { }

        public ITile GetNeighbor(Point point, EdgeDirection direction)
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

        public IEnumerable<ITile> GetAllNeighbors(Point point)
        {
            return Enum.GetValues(typeof(EdgeDirection)).Cast<EdgeDirection>()
                .Select(d => GetNeighbor(point, d)).Where(tile => tile != NopTile.Instance);
        }
    }
}
