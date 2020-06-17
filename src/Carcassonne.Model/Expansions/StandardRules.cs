using System.Collections.Generic;
using System.Collections.ObjectModel;
using Carcassonne.Model.Rules;
using GameBase.Model.Rules;

namespace Carcassonne.Model.Expansions
{
    public class StandardRules : ExpansionPack
    {
        public static readonly StandardRules Instance = new StandardRules();

        private readonly Tile.TileBuilder m_builder = new Tile.TileBuilder();
        private readonly List<IPlaceRule<IBoard, ITile>> m_defaultPlaceRules = new List<IPlaceRule<IBoard, ITile>>();
        private readonly Tile m_startTile;

        private StandardRules()
        {
            DefaultPlaceRules = new ReadOnlyCollection<IPlaceRule<IBoard, ITile>>(m_defaultPlaceRules);
            //Before
            WritablePlaceRules.Add(new EmptyBoardPlaceRule());
            WritablePlaceRules.Add(new NoneTilePlaceRule());
            WritablePlaceRules.Add(new OccupiedPlaceRule());
            WritableClaimRules.Add(new CityRoadRegionClaimRule());
            WritableClaimRules.Add(new MonasteryClaimRule());
            WritablePlayerRules.Add(new CreateMeeplePlayerCreationRule(7, MeepleType.Meeple));

            //After
            WritableJoinRules.Add(new MonasteryJoinRule());
            WritableJoinRules.Add(new EdgeRegionJoinRule());
            WritableScoreRules.Add(new TileRegionScoreRule(TileRegionType.Monastery));
            WritableScoreRules.Add(new CityRegionScoreRule());
            WritableScoreRules.Add(new RoadRegionScoreRule());
            m_defaultPlaceRules.Add(new DefaultPlaceRule());

            m_startTile = m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.East, EdgeDirection.West);
        }

        public ReadOnlyCollection<IPlaceRule<IBoard, ITile>> DefaultPlaceRules { get; }

        public override void AfterDeckShuffle(Deck deck)
        {
            deck.Push(m_startTile);
        }

        public override void BeforeDeckShuffle(Deck deck)
        {
            deck.Clear();

            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.South, EdgeDirection.West)
                .AddShield(EdgeDirection.North)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                .AddRoadRegion(EdgeDirection.South)
                );
            AddTile(deck, 2, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                .AddShield(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.South)
                );
            AddTile(deck, 2, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                .AddFlowers()
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                .AddShield(EdgeDirection.North)
                );
            AddTile(deck, 3, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddRoadRegion(EdgeDirection.South, EdgeDirection.West)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddShield(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.South, EdgeDirection.West)
                );
            AddTile(deck, 2, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddFlowers()
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddShield(EdgeDirection.North)
                );
            AddTile(deck, 2, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddShield(EdgeDirection.North)
                .AddFlowers()
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddCityRegion(EdgeDirection.West)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddFlowers()
                .AddCityRegion(EdgeDirection.West)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.East, EdgeDirection.West)
                );
            AddTile(deck, 2, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.East, EdgeDirection.West)
                .AddShield(EdgeDirection.East)
                );
            AddTile(deck, 2, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddCityRegion(EdgeDirection.South)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddCityRegion(EdgeDirection.South)
                .AddFlowers()
                );
            AddTile(deck, 3, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.East)
                .AddRoadRegion(EdgeDirection.South)
                .AddRoadRegion(EdgeDirection.West)
                );
            AddTile(deck, 3, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.South, EdgeDirection.West)
                );
            AddTile(deck, 3, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.East, EdgeDirection.South)
                );
            AddTile(deck, 3, m_startTile.TileClone());
            AddTile(deck, 4, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddFlowers()
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddRoadRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.East)
                .AddRoadRegion(EdgeDirection.South)
                .AddRoadRegion(EdgeDirection.West)
                );
            AddTile(deck, 4, m_builder.NewTile()
                .AddRoadRegion(EdgeDirection.East)
                .AddRoadRegion(EdgeDirection.South)
                .AddRoadRegion(EdgeDirection.West)
                );
            AddTile(deck, 8, m_builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.East)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.East)
                .AddFlowers()
                );
            AddTile(deck, 7, m_builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.South)
                );
            AddTile(deck, 1, m_builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.South)
                .AddFlowers()
                );
            AddTile(deck, 4, m_builder.NewTile()
                .AddMonastery()
                );
            AddTile(deck, 2, m_builder.NewTile()
                .AddRoadRegion(EdgeDirection.South)
                .AddMonastery()
                );
        }

        private static void AddTile(Deck deck, int count, Tile tile)
        {
            for (var i = 1; i < count; i++)
            {
                deck.Push(tile.TileClone());
            }
            deck.Push(tile);
        }
    }
}