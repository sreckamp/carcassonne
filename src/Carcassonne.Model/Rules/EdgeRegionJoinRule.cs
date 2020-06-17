using System;
using System.Diagnostics;
using System.Linq;

namespace Carcassonne.Model.Rules
{
    public class EdgeRegionJoinRule : IJoinRule
    {
        public bool Applies(ITile newTile, ITile neighbor, EdgeDirection direction)
        {
            if (!direction.IsCardinal()) return false;
            var mine = newTile.GetEdge(direction);
            return mine.FitsWith(neighbor.GetEdge(direction.Opposite()))
                   && (mine == EdgeRegionType.City || mine == EdgeRegionType.Road);
        }

        public void Join(ITile newTile, ITile neighbor, EdgeDirection direction)
        {
            var mine = newTile.GetRegion(direction);
            var theirs = neighbor.GetRegion(direction.Opposite());

            if (theirs.Container is PointContainer pc)
            {
                if (mine.Container is PointContainer mpc)
                {
                    Debug.Write($"Merge {mpc}:[{string.Join(";", mpc.Regions)} with {pc}:[{string.Join(";", pc.Regions)}]");
                    pc.Merge(mpc);
                    Debug.WriteLine($" => {pc}:[{string.Join(";", pc.Regions)}]");
                    Debug.WriteLine($"{mine} in {mine.Container}");
                }
                else
                {
                    Debug.Write($"Add {mine} to {pc}:[{string.Join(";", pc.Regions)}]");
                    pc.Add(mine);
                    Debug.WriteLine($" => {pc}:[{string.Join(";", pc.Regions)}]");
                    Debug.WriteLine($"{mine} in {mine.Container}");
                }
            }
            else switch (mine.Type)
            {
                case EdgeRegionType.City when mine.Container == NopPointContainer.Instance:
                {
                    var container = new CityPointContainer();
                    container.Add(mine);
                    Debug.WriteLine($"{mine} in {mine.Container}");
                    break;
                }
                case EdgeRegionType.Road when mine.Container == NopPointContainer.Instance:
                {
                    var container = new PointContainer(mine.Type);
                    container.Add(mine);
                    Debug.WriteLine($"{mine} in {mine.Container}");
                    break;
                }
            }
        }
    }
}