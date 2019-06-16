using GameBase.Model;
using GameBase.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Carcassonne.Model.Rules
{
    public class DefaultPlaceRule : IPlaceRule<Tile, CarcassonneMove>
    {
        #region IPlaceRule Members

        public virtual bool Applies(IGameBoard<Tile, CarcassonneMove> board, Tile tile, CarcassonneMove move)
        {
            return true;
        }

        public virtual bool Fits(IGameBoard<Tile, CarcassonneMove> board, Tile tile, CarcassonneMove move)
        {
            bool hasNeighbor = false;
            var b = board as CarcassonneGameBoard;
            foreach (EdgeDirection d in Enum.GetValues(typeof(EdgeDirection)))
            {
                var n = b.GetNeighbor(move.Location, d);
                if (n != null)
                {
                    hasNeighbor = true;
                    if (!RegionsMatch(tile.GetRegion(d), n.GetRegion(d.Opposite())))
                    {
                        return false;
                    }
                }
            }
            return hasNeighbor;
        }

        #endregion

        protected virtual bool RegionsMatch(EdgeRegion myRegion, EdgeRegion thierRegion)
        {
            return (myRegion?.Type == thierRegion?.Type);
        }
    }
}
