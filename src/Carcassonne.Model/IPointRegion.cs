using System.Collections.Generic;

namespace Carcassonne.Model
{
    public interface IPointRegion
    {
        bool IsClosed { get; }
        bool IsForcedOpened { get; set; }
        List<Player> Owners { get; }
        void ReturnMeeple();
    }
}
