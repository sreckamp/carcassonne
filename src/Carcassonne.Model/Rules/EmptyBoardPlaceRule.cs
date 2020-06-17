using System.Drawing;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class EmptyBoardPlaceRule : IPlaceRule<IBoard, ITile>
    {
        public bool Applies(IBoard board, ITile tile, Point location) => board.IsEmpty;

        public bool Fits(IBoard board, ITile tile, Point location) => location.X == 0 && location.Y == 0;
    }
}
