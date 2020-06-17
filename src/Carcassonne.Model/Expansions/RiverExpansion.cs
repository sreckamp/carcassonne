using System.Drawing;
using System.Linq;
using Carcassonne.Model.Rules;

namespace Carcassonne.Model.Expansions
{
    public class RiverExpansion : ExpansionPack
    {
        public static readonly ExpansionPack Instance = new RiverExpansion();

        private readonly Deck m_riverDeck = new Deck();
        private readonly Tile.TileBuilder m_builder = new Tile.TileBuilder();

        private readonly Tile m_spring;
        private readonly Tile m_lake;

        private RiverExpansion()
        {
            WritablePlaceRules.Add(new RiverFitRule());
            m_spring = m_builder.NewTile()
                .AddRiverRegion(EdgeDirection.South);
            m_lake = m_builder.NewTile()
                .AddRiverRegion(EdgeDirection.North);
        }

        public override void AfterDeckShuffle(Deck deck)
        {
            if (m_riverDeck.Count == 0)
            {
                m_riverDeck.Push(m_builder.NewTile()
                    .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                    .AddRiverRegion(EdgeDirection.South, EdgeDirection.West)
                );
                m_riverDeck.Push(m_builder.NewTile()
                    .AddCityRegion(EdgeDirection.North)
                    .AddRiverRegion(EdgeDirection.East, EdgeDirection.West)
                    .AddCityRegion(EdgeDirection.South)
                );
                m_riverDeck.Push(m_builder.NewTile()
                    .AddCityRegion(EdgeDirection.North)
                    .AddRiverRegion(EdgeDirection.East, EdgeDirection.West)
                    .AddRoadRegion(EdgeDirection.South)
                );
                m_riverDeck.Push(m_builder.NewTile()
                    .AddRoadRegion(EdgeDirection.North, EdgeDirection.East)
                    .AddRiverRegion(EdgeDirection.South, EdgeDirection.West)
                );
                m_riverDeck.Push(m_builder.NewTile()
                    .AddRoadRegion(EdgeDirection.North)
                    .AddRiverRegion(EdgeDirection.East, EdgeDirection.West)
                    .AddRoadRegion(EdgeDirection.South)
                );
                m_riverDeck.Push(m_builder.NewTile()
                    .AddRoadRegion(EdgeDirection.South)
                    .AddRiverRegion(EdgeDirection.East, EdgeDirection.West)
                    .AddMonastery()
                );
                m_riverDeck.Push(m_builder.NewTile()
                    .AddRiverRegion(EdgeDirection.North, EdgeDirection.East)
                );
                m_riverDeck.Push(m_builder.NewTile()
                    .AddRiverRegion(EdgeDirection.North, EdgeDirection.East)
                    .AddFlowers()
                );
                m_riverDeck.Push(m_builder.NewTile()
                    .AddRiverRegion(EdgeDirection.North, EdgeDirection.South)
                );
                m_riverDeck.Push(m_builder.Tile.TileClone());
            }

            m_riverDeck.Shuffle();
            deck.Push(m_lake);
            foreach (var tile in m_riverDeck)
            {
                deck.Push(tile);
            }
            deck.Push(m_spring);
        }

        private class RiverFitRule : DefaultPlaceRule
        {
            public override bool Applies(IBoard board, ITile tile, Point location)
            {
                return tile.Contains(EdgeRegionType.River);
            }

            protected override bool RegionsMatch(IEdgeRegion myRegion, IEdgeRegion theirRegion)
            {
                if (theirRegion.Type == EdgeRegionType.Any) return true;
                // Must map a River region
                if (myRegion.Type != EdgeRegionType.River || theirRegion.Type != EdgeRegionType.River) return false;
                // Cannot have 2 turns in the same direction
                if (myRegion.Edges.Count != 2 || myRegion.Edges[0].Opposite() == myRegion.Edges[1] ||
                    theirRegion.Edges.Count != 2 ||
                    theirRegion.Edges[0].Opposite() == theirRegion.Edges[1]) return true;
                return myRegion.Edges.All(t => theirRegion.Edges.All(t1 => t != t1));
            }
        }
    }
}
