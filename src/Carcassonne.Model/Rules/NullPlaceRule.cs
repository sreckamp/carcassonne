using GameBase.Model;
using GameBase.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Carcassonne.Model.Rules
{
    public class NullPlaceRule : IPlaceRule<Tile, CarcassonneMove>
    {
        public bool Applies(IGameBoard<Tile, CarcassonneMove> board, Tile tile, CarcassonneMove move)
        {
            return tile == null;
        }

        public bool Fits(IGameBoard<Tile, CarcassonneMove> board, Tile tile, CarcassonneMove move)
        {
            return false;
        }
    }
}
