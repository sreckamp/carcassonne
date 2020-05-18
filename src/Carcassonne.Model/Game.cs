using System;
using System.Collections.Generic;
using System.Drawing;
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
    public sealed class Game
    {
        private Tile m_defaultStartTile = Tile.None;
        private readonly ExpansionPack[] m_expansions;
        public RuleSet RuleSet { get; }

        public Game(params ExpansionPack[] expansions)
        {
            ActiveTileChanged += (sender, args) => { };
            ActivePlayerChanged += (sender, args) => { };
            m_expansions = expansions;
            RuleSet = new RuleSet(expansions);
            Board = new GameBoard();
            Players = new ObservableList<Player>();
        }

        public GameBoard Board { get; }
        public ObservableList<IPointContainer> PointRegions { get; } = new ObservableList<IPointContainer>();

        private readonly Deck m_deck = new Deck();

        public GameState State { get; private set; } = GameState.NotStarted;

        public event EventHandler<ChangedValueArgs<Tile>> ActiveTileChanged;
        private Tile m_activeTile = Tile.None;

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

        private void Score(IEnumerable<IPointContainer> changed)
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
            if (m_deck.Count > 0)
            {
                var t = m_deck.Pop();
                ActiveTile = t;
            }
            else
            {
                ActiveTile = Tile.None;
            }
            return ActiveTile;
        }

        private void Reset()
        {
            foreach (var player in Players)
            {
                player.Reset();
                RuleSet.UpdatePlayer(player);
            }
            m_deck.Clear();
            PopulateDeck();
            Shuffle();
            Board.Clear();
            var useDefaultStart = m_expansions.All(exp => !exp.IgnoreDefaultStart);
            if (useDefaultStart)
            {
                Place(m_defaultStartTile, new Point(0, 0));
            }
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
            Reset();
            do
            {
                var player = NextPlayer();
                if (player == Player.None) continue;
                if (Draw() == Tile.None) break;

                State = GameState.Place;
                var changed = new List<IPointContainer>();
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
                //TODO: Implement claiming
                // IClaimable claimed;
                // do
                // {
                //     var (region, type) = player.GetClaim(this);
                //     if (region != DefaultClaimable.Instance)
                //     {
                //         claimed = Claim(region, type);
                //     }
                //     else
                //     {
                //         //Skip the claim state.
                //         break;
                //     }
                // } while (claimed == DefaultClaimable.Instance);
                Score(changed);
            } while (ActiveTile != Tile.None);
            End();
            State = GameState.End;
        }

        public void Shuffle()
        {
            foreach (var e in m_expansions)
            {
                e.BeforeDeckShuffle(m_deck);
            }
            // m_deck.Shuffle();
            foreach (var e in m_expansions)
            {
                e.AfterDeckShuffle(m_deck);
            }
        }

        private void AddTile(int count, Tile tile)
        {
            for (var i = 1; i < count; i++)
            {
                m_deck.Push(tile.TileClone());
            }
            m_deck.Push(tile);
        }

        private List<IPointContainer> Place(Move move)
        {
            var changed = new List<IPointContainer>();
            var tile = new RotatedTile(ActiveTile, move.Rotation);
            if (TryFit(tile, move.Location))
            {
                changed = Place(tile, move.Location);
            }
            return changed;
        }

        private List<IPointContainer> Place(ITile tile, Point location)
        {
            var available = Enum.GetValues(typeof(EdgeDirection)).Cast<EdgeDirection>().ToList();
            var changedPointRegions = new List<IPointContainer>();

            Board.Add(new Placement<ITile>(tile, location));
            if (tile.TileRegion is IPointContainer pc)
            {
                PointRegions.Add(pc);
                changedPointRegions.Add(pc);
            }
            var allNeighbors = Board.GetAllNeighbors(location);
            foreach (var n in allNeighbors)
            {
                n.TileRegion.Add(tile);
                if (n.TileRegion is IPointContainer container)
                {
                    changedPointRegions.Add(container);
                }
                tile.TileRegion.Add(n);
            }
//            var neighbors = Board.GetNeighbors(move.Location);
            while (available.Count > 0)
            {
                var r = tile.GetRegion(available[0]);
                if (r.Type != EdgeRegionType.Any && r.Type != EdgeRegionType.None)
                {
                    var containers = new List<IPointContainer>();
                    foreach (var d in r.Edges)
                    {
                        var n = Board.GetNeighbor(location, d);
                        var nr = n.GetRegion(d.Opposite());
                        containers.Add(nr.Container);
                        available.Remove(d);
                    }
                    if (containers.Count > 0 && containers[0] is PointRegion pr)
                    {
                        while (containers.Count > 1)
                        {
                            if (containers[1] is PointRegion dup)
                            {
                                pr.Merge(dup);
                                if (PointRegions.Contains(dup))
                                {
                                    PointRegions.Remove(dup);
                                }
                            }
                            containers.RemoveAt(1);
                        }
                        pr.Add(r);
                        if (!PointRegions.Contains(pr))
                        {
                            PointRegions.Add(pr);
                        }
                        if (!changedPointRegions.Contains(pr))
                        {
                            changedPointRegions.Add(pr);
                        }
                    }
                    else
                    {
                        switch (r.Type)
                        {
                            case EdgeRegionType.City:
                                var cpr = new CityPointRegion();
                                cpr.Add(r);
                                PointRegions.Add(cpr);
                                if (!changedPointRegions.Contains(cpr))
                                {
                                    changedPointRegions.Add(cpr);
                                }
                                break;
                            case EdgeRegionType.None:
                                break;
                            case EdgeRegionType.River:
                                break;
                            case EdgeRegionType.Road:
                                var rpr = new PointRegion(r.Type);
                                rpr.Add(r);
                                PointRegions.Add(rpr);
                                if (!changedPointRegions.Contains(rpr))
                                {
                                    changedPointRegions.Add(rpr);
                                }
                                break;
                            case EdgeRegionType.Any:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
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
        /// <param name="location">Proposed location</param>
        /// <returns>true if the ActiveTile would fit at the current location.</returns>
        private bool TryFit(ITile tile, Point location)
        {
            return RuleSet.Fits(Board, tile, location);
        }

        public Player AddPlayer(string name)
        {
            if (State != GameState.NotStarted) return Player.None;
            var p = new Player(name);
            Players.Add(p);
            return p;
        }
    }
}
