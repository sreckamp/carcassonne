using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class EmptyBoardPlaceRule : IPlaceRule<Tile, CarcassonneMove>
    {
        public bool Applies(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return board.IsEmpty && move.Location.X == 0 && move.Location.Y == 0;
        }

        public bool Fits(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return true;
        }
    }
}
