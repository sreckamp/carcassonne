using GameBase.Model;
using GameBase.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Carcassonne.Model.Rules
{
    public class OccupiedPlaceRule : IPlaceRule<Tile, CarcassonneMove>
    {
        public bool Applies(IGameBoard<Tile, CarcassonneMove> board, Tile tile, CarcassonneMove move)
        {
            return (board[move.Location] != null && board[move.Location] != tile);
        }

        public bool Fits(IGameBoard<Tile, CarcassonneMove> board, Tile tile, CarcassonneMove move)
        {
            return false;
        }
    }
}
