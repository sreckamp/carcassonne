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
            var xOffset = direction switch
            {
                EdgeDirection.NorthEast => 1,
                EdgeDirection.East => 1,
                EdgeDirection.SouthEast => 1,
                EdgeDirection.SouthWest => -1,
                EdgeDirection.West => -1,
                EdgeDirection.NorthWest => -1,
                _ => 0
            };
            var yOffset = direction switch
            {
                EdgeDirection.NorthWest => -1,
                EdgeDirection.North => -1,
                EdgeDirection.NorthEast => -1,
                EdgeDirection.SouthWest => 1,
                EdgeDirection.South => 1,
                EdgeDirection.SouthEast => 1,
                _ => 0
            };
            return base[point.X + xOffset, point.Y + yOffset];
        }
    }
}
