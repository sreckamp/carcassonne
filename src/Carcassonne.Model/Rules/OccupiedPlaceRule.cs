using System.Drawing;
using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class OccupiedPlaceRule : IPlaceRule<IGameBoard, ITile>
    {
        public bool Applies(IGameBoard board, ITile tile, Point location)
        {
            return board[location] != NopTile.Instance && board[location] != tile;
        }

        public bool Fits(IGameBoard board, ITile tile, Point location)
        {
            return false;
        }
    }
}
