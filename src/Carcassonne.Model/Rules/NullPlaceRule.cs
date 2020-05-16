using GameBase.Model;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Rules
{
    public class NoneTilePlaceRule : IPlaceRule<Tile, CarcassonneMove>
    {
        public bool Applies(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return tile == Tile.None;
        }

        public bool Fits(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
        {
            return false;
        }
    }
}
