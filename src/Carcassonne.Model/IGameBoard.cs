using System.Collections.Generic;
using System.Drawing;
using GameBase.Model;

namespace Carcassonne.Model
{
    public interface IGameBoard:IGameBoard<ITile>
    {
        IEnumerable<ITile> GetAllNeighbors(Point point);
    }
}