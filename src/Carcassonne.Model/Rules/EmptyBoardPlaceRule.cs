using System.Drawing;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class EmptyBoardPlaceRule : IPlaceRule<IGameBoard, ITile>
    {
        public bool Applies(IGameBoard board, ITile tile, Point location) => board.IsEmpty;

        public bool Fits(IGameBoard board, ITile tile, Point location) => location.X == 0 && location.Y == 0;
    }
}
