using System.Drawing;
using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class NoneTilePlaceRule : IPlaceRule<IGameBoard, ITile>
    {
        public bool Applies(IGameBoard board, ITile tile, Point location)
        {
            return tile == Tile.None;
        }

        public bool Fits(IGameBoard board, ITile tile, Point location)
        {
            return false;
        }
    }
}
