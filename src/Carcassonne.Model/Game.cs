using System;
using System.Collections.Generic;
using System.Linq;
using GameBase.Model;

namespace Carcassonne.Model
{
    public enum GameState
    {
        NotStarted,
        Place,
        Claim,
        Score,
        Discard,
        End,
    }

    /// <summary>
    /// </summary>
    public class Game
    {
        private Tile m_defaultStartTile = Tile.None;
        private readonly AbstractExpansionPack[] m_expansions;
        public static RuleSet RuleSet;

        public Game(params AbstractExpansionPack[] expansions)
        {
            ActiveTileChanged += (sender, args) => { };
            ActivePlayerChanged += (sender, args) => { };
            m_expansions = expansions;
            RuleSet = new RuleSet(expansions);
            Board = new CarcassonneGameBoard(RuleSet);
            Players = new ObservableList<Player>();
            ResetGame();
        }

        public CarcassonneGameBoard Board { get; }
        public ObservableList<IPointRegion> PointRegions { get; } = new ObservableList<IPointRegion>();

        public readonly Deck Deck = new Deck();

        public GameState State { get; private set; }

        public event EventHandler<ChangedValueArgs<Tile>> ActiveTileChanged;
        private Tile m_activeTile;

        private Tile ActiveTile
        {
            get => m_activeTile;
            set
            {
                var old = m_activeTile;
                m_activeTile = value;
                ActiveTileChanged.Invoke(this, new ChangedValueArgs<Tile>(old, value));
            }
        }

        public event EventHandler<ChangedValueArgs<Player>> ActivePlayerChanged;
        private int m_activePlayerIndex = int.MaxValue;
        public Player ActivePlayer => m_activePlayerIndex < Players.Count ? Players[m_activePlayerIndex] : Player.None;

        public ObservableList<Player> Players { get; }

        private Player NextPlayer()
        {
            var old = ActivePlayer;
            if (m_activePlayerIndex >= Players.Count - 1)
            {
                m_activePlayerIndex = 0;
            }
            else
            {
                m_activePlayerIndex++;
            }
            ActivePlayerChanged.Invoke(this, new ChangedValueArgs<Player>(old, ActivePlayer));
            return ActivePlayer;
        }

        public IClaimable Claim(IClaimable region, MeepleType type)
        {
            if (region == DefaultClaimable.Instance) return DefaultClaimable.Instance;
            var m = ActivePlayer.GetMeeple(type);
            if (m == Meeple.None) return DefaultClaimable.Instance;
            if (!RuleSet.IsAvailable(region, m.Type)) return DefaultClaimable.Instance;
            region.Claim(m);
            return region;
        }

        private void End()
        {
            foreach (var pr in PointRegions)
            {
                var score = RuleSet.GetEndScore(pr);
                if (score <= 0) continue;
                foreach (var o in pr.Owners)
                {
                    o.Score += score;
                }
                pr.ReturnMeeple();
            }
        }

        private void Score(IEnumerable<IPointRegion> changed)
        {
            State = GameState.Score;
            foreach (var pr in changed)
            {
                if (pr.IsForcedOpened)
                {
                    pr.IsForcedOpened = false;
                }
                var score = RuleSet.GetScore(pr);
                if (score <= 0) continue;
                foreach (var o in pr.Owners)
                {
                    o.Score += score;
                }
                pr.ReturnMeeple();
            }
        }

        private Tile Draw()
        {
            if (Deck.Count > 0)
            {
                var t = Deck.Pop();
                ActiveTile = t;
            }
            else
            {
                ActiveTile = Tile.None;
            }
            return ActiveTile;
        }

        protected virtual void ResetGame()
        {
            Players.Clear();
            Deck.Clear();
            PopulateDeck();
            Board.Clear();
            State = GameState.NotStarted;
        }

        private void PopulateDeck()
        {
            var builder = new Tile.TileBuilder();
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.South, EdgeDirection.West)
                .AddShield(EdgeDirection.North)
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                .AddRoadRegion(EdgeDirection.South)
                );
            AddTile(2, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                .AddShield(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.South)
                );
            AddTile(2, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                .AddFlowers()
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East, EdgeDirection.West)
                .AddShield(EdgeDirection.North)
                );
            AddTile(3, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddRoadRegion(EdgeDirection.South, EdgeDirection.West)
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddShield(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.South, EdgeDirection.West)
                );
            AddTile(2, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddFlowers()
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddShield(EdgeDirection.North)
                );
            AddTile(2, builder.NewTile()
                .AddCityRegion(EdgeDirection.North, EdgeDirection.East)
                .AddShield(EdgeDirection.North)
                .AddFlowers()
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddCityRegion(EdgeDirection.West)
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddFlowers()
                .AddCityRegion(EdgeDirection.West)
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.East, EdgeDirection.West)
                );
            AddTile(2, builder.NewTile()
                .AddCityRegion(EdgeDirection.East, EdgeDirection.West)
                .AddShield(EdgeDirection.East)
                );
            AddTile(2, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddCityRegion(EdgeDirection.South)
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddCityRegion(EdgeDirection.South)
                .AddFlowers()
                );
            AddTile(3, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.East)
                .AddRoadRegion(EdgeDirection.South)
                .AddRoadRegion(EdgeDirection.West)
                );
            AddTile(3, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.South, EdgeDirection.West)
                );
            AddTile(3, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.East, EdgeDirection.South)
                );
            AddTile(3, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.East, EdgeDirection.West)
                );
            m_defaultStartTile = builder.Tile.TileClone();
            AddTile(4, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                );
            AddTile(1, builder.NewTile()
                .AddCityRegion(EdgeDirection.North)
                .AddFlowers()
                );
            AddTile(1, builder.NewTile()
                .AddRoadRegion(EdgeDirection.North)
                .AddRoadRegion(EdgeDirection.East)
                .AddRoadRegion(EdgeDirection.South)
                .AddRoadRegion(EdgeDirection.West)
                );
            AddTile(4, builder.NewTile()
                .AddRoadRegion(EdgeDirection.East)
                .AddRoadRegion(EdgeDirection.South)
                .AddRoadRegion(EdgeDirection.West)
                );
            AddTile(8, builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.East)
                );
            AddTile(1, builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.East)
                .AddFlowers()
                );
            AddTile(7, builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.South)
                );
            AddTile(1, builder.NewTile()
                .AddRoadRegion(EdgeDirection.North, EdgeDirection.South)
                .AddFlowers()
                );
            AddTile(4, builder.NewTile()
                .AddMonastery()
                );
            AddTile(2, builder.NewTile()
                .AddRoadRegion(EdgeDirection.South)
                .AddMonastery()
                );
        }

        public void Play()
        {
            if (State != GameState.NotStarted) return;
            Shuffle();
            var useDefaultStart = m_expansions.All(exp => !exp.IgnoreDefaultStart);
            if (useDefaultStart)
            {
                Place(m_defaultStartTile, new CarcassonneMove(0, 0, Rotation.None));
            }
            do
            {
                var player = NextPlayer();
                if (player == Player.None) continue;
                if (Draw() == Tile.None) break;

                State = GameState.Place;
                var changed = new List<IPointRegion>();
                do
                {
                    var mv = player.GetMove(this);
                    if (!mv.IsEmpty)
                    {
                        changed = Place(mv);
                    }
                    else
                    {
                        State = GameState.Discard;
                        break;
                    }
                } while (changed.Count == 0);

                if (State != GameState.Place) continue;
                State = GameState.Claim;
                IClaimable claimed;
                do
                {
                    var (region, type) = player.GetClaim(this);
                    if (region != DefaultClaimable.Instance)
                    {
                        claimed = Claim(region, type);
                    }
                    else
                    {
                        //Skip the claim state.
                        break;
                    }
                } while (claimed == DefaultClaimable.Instance);
                Score(changed);
            } while (ActiveTile != null);
            End();
            State = GameState.End;
        }

        public void Shuffle()
        {
            foreach (var e in m_expansions)
            {
                e.BeforeDeckShuffle(Deck);
            }
            Deck.Shuffle();
            foreach (var e in m_expansions)
            {
                e.AfterDeckShuffle(Deck);
            }
        }

        protected void AddTile(int count, Tile tile)
        {
            for (var i = 1; i < count; i++)
            {
                Deck.Push(tile.TileClone());
            }
            Deck.Push(tile);
        }

        private List<IPointRegion> Place(CarcassonneMove move)
        {
            var changed = new List<IPointRegion>();
            if (TryFit(ActiveTile, move))
            {
                changed = Place(ActiveTile, move);
            }
            return changed;
        }

        private List<IPointRegion> Place(Tile tile, CarcassonneMove move)
        {
            var available = new List<EdgeDirection>((EdgeDirection[])Enum.GetValues(typeof(EdgeDirection)));
            var changedPointRegions = new List<IPointRegion>();

            Board.Add(new Placement<Tile, CarcassonneMove>(tile, move));
            if (tile.TileRegion.Type != TileRegionType.None)
            {
                PointRegions.Add(tile.TileRegion);
                changedPointRegions.Add(tile.TileRegion);
            }
            var allNeighbors = Board.GetAllNeighbors(move.Location);
            foreach (var n in allNeighbors)
            {
                if (n.TileRegion != TileRegion.None)
                {
                    n.TileRegion.Add(tile);
                    changedPointRegions.Add(n.TileRegion);
                }
                if (tile.TileRegion != TileRegion.None)
                {
                    tile.TileRegion.Add(n);
                }
            }
//            var neighbors = Board.GetNeighbors(move.Location);
            while (available.Count > 0)
            {
                var r = tile.GetRegion(available[0]);
                if (r.Type != RegionType.None)
                {
                    var regions = new List<PointRegion>();
                    foreach (var d in r.Edges)
                    {
                        var n=Board.GetNeighbor(move.Location, d);
                        if (n != Tile.None)
                        {
                            var nr = n.GetRegion(d.Opposite());
                            regions.Add(nr.Container);
                        }
                        available.Remove(d);
                    }
                    if (regions.Count > 0)
                    {
                        while (regions.Count > 1)
                        {
                            var dup = regions[1];
                            regions[0].Merge(dup);
                            regions.Remove(dup);
                            if (PointRegions.Contains(dup))
                            {
                                PointRegions.Remove(dup);
                            }
                        }
                        regions[0].Add(r);
                        if (!PointRegions.Contains(regions[0]))
                        {
                            PointRegions.Add(regions[0]);
                        }
                        if (!changedPointRegions.Contains(regions[0]))
                        {
                            changedPointRegions.Add(regions[0]);
                        }
                    }
                    else
                    {
                        switch (r.Type)
                        {
                            case RegionType.City:
                                var cpr = new CityPointRegion();
                                cpr.Add(r);
                                PointRegions.Add(cpr);
                                if (!changedPointRegions.Contains(cpr))
                                {
                                    changedPointRegions.Add(cpr);
                                }
                                break;
                            case RegionType.Grass:
                                break;
                            case RegionType.River:
                                break;
                            case RegionType.Road:
                                var rpr = new PointRegion(r.Type);
                                rpr.Add(r);
                                PointRegions.Add(rpr);
                                if (!changedPointRegions.Contains(rpr))
                                {
                                    changedPointRegions.Add(rpr);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    available.RemoveAt(0);
                }
            }
            foreach (var r in changedPointRegions)
            {
                r.IsForcedOpened = true;
            }
            return changedPointRegions;
        }

        /// <summary>
        /// See if the tile fits at the current location.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="move">Proposed location and rotation of ActiveTile</param>
        /// <returns>true if the ActiveTile would fit at the current location.</returns>
        private bool TryFit(Tile tile, CarcassonneMove move)
        {
            return RuleSet.Fits(Board, tile, move);
        }

        public Player AddPlayer(string name)
        {
            if (State != GameState.NotStarted) return Player.None;
            var p = new Player(name);
            Players.Add(p);
            RuleSet.UpdatePlayer(p);
            return p;
        }
    }
}
