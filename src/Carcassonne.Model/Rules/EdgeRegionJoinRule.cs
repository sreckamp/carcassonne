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
                    pc.Merge(mpc);
                }
                else
                {
                    pc.Add(mine);
                }
            }
            else switch (mine.Type)
            {
                case EdgeRegionType.City when mine.Container == NopPointContainer.Instance:
                {
                    var container = new CityPointContainer();
                    container.Add(mine);
                    break;
                }
                case EdgeRegionType.Road when mine.Container == NopPointContainer.Instance:
                {
                    var container = new PointContainer(mine.Type);
                    container.Add(mine);
                    break;
                }
            }
        }
    }
}