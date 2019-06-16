using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model
{
    public interface IClaimable
    {
        void Claim(Meeple meeple);
        void ResetClaim();
        Meeple Claimer { get; }
        bool IsClosed { get; }
    }
}
