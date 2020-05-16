using System;
using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class DefaultPlaceRule : IPlaceRule<Tile, CarcassonneMove>
    {
        #region IPlaceRule Members

        public virtual bool Applies(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return true;
        }

        public virtual bool Fits(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            var hasNeighbor = false;
            if(!(board is CarcassonneGameBoard b)) return false;
            foreach (EdgeDirection d in Enum.GetValues(typeof(EdgeDirection)))
            {
                var n = b.GetNeighbor(move.Location, d);
                if (n == Tile.None) continue;
                hasNeighbor = true;
                if (!RegionsMatch(tile.GetRegion(d), n.GetRegion(d.Opposite())))
                {
                    return false;
                }
            }
            return hasNeighbor;
        }

        #endregion

        protected virtual bool RegionsMatch(EdgeRegion myRegion, EdgeRegion theirRegion)
        {
            return myRegion.Type == theirRegion.Type;
        }
    }
}
