using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class OccupiedPlaceRule : IPlaceRule<Tile, CarcassonneMove>
    {
        public bool Applies(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return board[move.Location] != Tile.None && board[move.Location] != tile;
        }

        public bool Fits(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return false;
        }
    }
}
