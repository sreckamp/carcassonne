using System.Drawing;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class OccupiedPlaceRule : IPlaceRule<IGameBoard, ITile>
    {
        public bool Applies(IGameBoard board, ITile tile, Point location) => board[location] != NopTile.Instance;

        public bool Fits(IGameBoard board, ITile tile, Point location) => false;
    }
}
