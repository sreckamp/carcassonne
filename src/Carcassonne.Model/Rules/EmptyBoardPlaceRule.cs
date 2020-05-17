using System.Drawing;
using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class EmptyBoardPlaceRule : IPlaceRule<IGameBoard, ITile>
    {
        public bool Applies(IGameBoard board, ITile tile, Point location)
        {
            return board.IsEmpty && location.X == 0 && location.Y == 0;
        }

        public bool Fits(IGameBoard board, ITile tile, Point location)
        {
            return true;
        }
    }
}
