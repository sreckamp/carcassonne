﻿using System;
using System.Drawing;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class DefaultPlaceRule : IPlaceRule<IBoard, ITile>
    {
        #region IPlaceRule Members

        public virtual bool Applies(IBoard board, ITile tile, Point location)
        {
            return true;
        }

        public virtual bool Fits(IBoard board, ITile tile, Point location)
        {
            if(!(board is Board b)) return false;
            var hasNeighbor = false;
            var dir = EdgeDirection.North;
            do
            {
                var n = b.GetNeighbor(location, dir);
                var mine = tile.GetRegion(dir);
                var theirs = n.GetRegion(dir.Opposite());
                hasNeighbor = hasNeighbor || theirs.Type != EdgeRegionType.Any;
                if (!RegionsMatch(mine, theirs)) return false;
                dir = dir.Rotate(Rotation.Clockwise);
            } while (dir != EdgeDirection.North);

            return hasNeighbor;
        }

        #endregion

        protected virtual bool RegionsMatch(IEdgeRegion myRegion, IEdgeRegion theirRegion)
        {
            return myRegion.Type.FitsWith(theirRegion.Type);
        }
    }
}
