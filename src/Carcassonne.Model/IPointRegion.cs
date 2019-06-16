using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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
