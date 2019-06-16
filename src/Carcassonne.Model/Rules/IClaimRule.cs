using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model.Rules
{
    public interface IClaimRule
    {
        bool Applies(IClaimable region, MeepleType type);
        bool IsAvailable(IClaimable region, MeepleType type);
    }
}
