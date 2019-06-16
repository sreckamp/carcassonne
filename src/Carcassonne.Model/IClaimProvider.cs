using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model
{
    public interface IClaimProvider
    {
        IClaimable GetClaim(Game game, out MeepleType type);
    }
}
