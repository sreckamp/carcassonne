using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model.Rules
{
    public interface IPlayerCreationRule
    {
        void UpdatePlayer(Player player);
    }
}
