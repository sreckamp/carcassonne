using System.Drawing;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class OccupiedPlaceRule : IPlaceRule<IBoard, ITile>
    {
        public bool Applies(IBoard board, ITile tile, Point location) => board[location] != NopTile.Instance;

        public bool Fits(IBoard board, ITile tile, Point location) => false;
    }
}
