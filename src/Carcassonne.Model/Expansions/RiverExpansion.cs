using System.Linq;
using Carcassonne.Model.Rules;
using GameBase.Model;

namespace Carcassonne.Model.Expansions
{
    public class RiverExpansion : AbstractExpansionPack
    {
        private Tile m_source;
        private Tile m_sink;
        private readonly Deck m_riverDeck = new Deck();

        public RiverExpansion()
        {
            WritablePlaceRules.Add(new RiverFitRule());
            var builder = new Tile.TileBuilder();
            m_source = builder.NewTile()
                .AddRiverRegion(EdgeDirection.South);
            m_sink = builder.NewTile()
                .AddRiverRegion(EdgeDirection.North);
        }

        public override bool IgnoreDefaultStart => true;

        public override void AfterDeckShuffle(Deck deck)
        {
            var builder = new Tile.TileBuilder();
            m_riverDeck.Clear();
            m_riverDeck.Push(builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddRiverRegion(EdgeDirection.South, EdgeDirection.West)
                );
            m_riverDeck.Push(builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRiverRegion(EdgeDirection.East, EdgeDirection.West)
                .AddCityRegion(EdgeDirection.South)
                );
            m_riverDeck.Push(builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRiverRegion(EdgeDirection.East, EdgeDirection.West)
                .AddRoadRegion(EdgeDirection.South)
                );
            m_riverDeck.Push(builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.East)
                .AddRiverRegion(EdgeDirection.South, EdgeDirection.West)
                );
            m_riverDeck.Push(builder.NewTile()
                .AddRoadRegion(EdgeDirection.North)
                .AddRiverRegion(EdgeDirection.East, EdgeDirection.West)
                .AddRoadRegion(EdgeDirection.South)
                );
            m_riverDeck.Push(builder.NewTile()
                .AddRoadRegion(EdgeDirection.South)
                .AddRiverRegion(EdgeDirection.East, EdgeDirection.West)
                .AddMonastery()
                );
            m_riverDeck.Push(builder.NewTile()
                .AddRiverRegion(EdgeDirection.North, EdgeDirection.East)
                );
            m_riverDeck.Push(builder.NewTile()
                .AddRiverRegion(EdgeDirection.North, EdgeDirection.East)
                .AddFlowers()
                );
            m_riverDeck.Push(builder.NewTile()
                .AddRiverRegion(EdgeDirection.North, EdgeDirection.South)
                );
            m_riverDeck.Push(builder.Tile.TileClone());

            m_riverDeck.Shuffle();
            deck.Push(m_sink);
            while (m_riverDeck.Count > 0)
            {
                deck.Push(m_riverDeck.Pop());
            }
            deck.Push(m_source);
        }

        private class RiverFitRule : DefaultPlaceRule
        {
            public override bool Applies(IGameBoard<Tile> board, Tile tile, CarcassonneMove move)
            {
                var riverRegions = tile.GetAvailableRegions(RegionType.River);
                return riverRegions.Count > 0;
            }

            protected override bool RegionsMatch(EdgeRegion myRegion, EdgeRegion theirRegion)
            {
                // Must map a River region
                if (myRegion.Type != RegionType.River || theirRegion.Type != RegionType.River) return false;
                // Cannot have 2 turns in the same direction
                if (myRegion.Edges.Length != 2 || myRegion.Edges[0].Opposite() == myRegion.Edges[1] ||
                    theirRegion.Edges.Length != 2 ||
                    theirRegion.Edges[0].Opposite() == theirRegion.Edges[1]) return true;
                return myRegion.Edges.All(t => theirRegion.Edges.All(t1 => t != t1));
            }
        }
    }
}
