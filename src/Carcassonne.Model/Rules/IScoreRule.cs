using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne.Model.Rules
{
    public interface IScoreRule
    {
        bool Applies(IPointRegion region);
        int GetScore(IPointRegion region);
        int GetEndScore(IPointRegion region);
    }
}
