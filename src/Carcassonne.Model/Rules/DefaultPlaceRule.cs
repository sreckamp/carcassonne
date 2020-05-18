using System;
using System.Drawing;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class DefaultPlaceRule : IPlaceRule<IGameBoard, ITile>
    {
        #region IPlaceRule Members

        public virtual bool Applies(IGameBoard board, ITile tile, Point location)
        {
            return true;
        }

        public virtual bool Fits(IGameBoard board, ITile tile, Point location)
        {
            if(!(board is GameBoard b)) return false;
            var hasNeighbor = false;
            foreach (EdgeDirection dir in Enum.GetValues(typeof(EdgeDirection)))
            {
                var n = b.GetNeighbor(location, dir);
                var mine = tile.GetRegion(dir);
                var theirs = n.GetRegion(dir.Opposite());
                hasNeighbor = hasNeighbor || theirs.Type != EdgeRegionType.Any;
                if (!RegionsMatch(mine, theirs)) return false;
            }

            return hasNeighbor;
        }

        #endregion

        protected virtual bool RegionsMatch(IEdgeRegion myRegion, IEdgeRegion theirRegion)
        {
            return theirRegion.Type == EdgeRegionType.Any || myRegion.Type == theirRegion.Type;
        }
    }
}
