using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class NoneTilePlaceRule : IPlaceRule<Tile, CarcassonneMove>
    {
        public bool Applies(IGameBoard<Tile, CarcassonneMove> board, Tile tile, CarcassonneMove move)
        {
            return tile == Tile.None;
        }

        public bool Fits(IGameBoard<Tile, CarcassonneMove> board, Tile tile, CarcassonneMove move)
        {
            return false;
        }
    }
}
