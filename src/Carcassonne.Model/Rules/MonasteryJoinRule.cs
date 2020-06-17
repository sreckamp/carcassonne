using System.Diagnostics;

namespace Carcassonne.Model.Rules
{
    public class MonasteryJoinRule : TileCountJoinRule
    {
        public MonasteryJoinRule() : base(TileRegionType.Monastery, t => t.HasMonastery) { }
    }
}