using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using Carcassonne.Model.Rules;
using System.Collections.ObjectModel;
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
        protected Tile m_defaultStartTile = null;
        private readonly ObservableList<IPointRegion> m_pointRegions =
                                    new ObservableList<IPointRegion>();
        private readonly AbstractExpansionPack[] m_expansions;
        public static RuleSet RuleSet;

        public Game(params AbstractExpansionPack[] expansions)
        {
            m_expansions = expansions;
            RuleSet = new RuleSet(expansions);
            Board = new CarcassonneGameBoard(RuleSet);
            Players = new ObservableList<Player>();
            ResetGame();
        }

        public CarcassonneGameBoard Board { get; private set; }
        public ObservableList<IPointRegion> PointRegions => m_pointRegions;

        public readonly Deck m_deck = new Deck();

        private GameState m_state;
        public GameState State
        {
            get => m_state;
            private set
            {
                m_state = value;
            }
        }

        public event EventHandler<ChangedValueArgs<Tile>> ActiveTileChanged;
        private Tile m_activeTile;
        public Tile ActiveTile
        {
            get => m_activeTile;
            private set
            {
                var old = m_activeTile;
                m_activeTile = value;
                ChangedValueArgs<Tile>.Trigger(ActiveTileChanged, this, old, value);
            }
        }

        public event EventHandler<ChangedValueArgs<Player>> ActivePlayerChanged;
        private int m_activePlayerIndex = int.MaxValue;
        public Player ActivePlayer
        {
            get { return m_activePlayerIndex < Players.Count ? Players[m_activePlayerIndex] : null; }
        }

        public ObservableList<Player> Players { get; private set; }

        private Player nextPlayer()
        {
            Player prev = ActivePlayer;
            if (m_activePlayerIndex >= Players.Count - 1)
            {
                m_activePlayerIndex = 0;
            }
            else
            {
                m_activePlayerIndex++;
            }
            ChangedValueArgs<Player>.Trigger(ActivePlayerChanged, this, prev, ActivePlayer);
            return ActivePlayer;
        }

        public IClaimable Claim(IClaimable region, MeepleType type)
        {
            if (region != null)
            {
                var m = ActivePlayer.GetMeeple(type);
                if (m != null)
                {
                    if (RuleSet.IsAvailable(region, m.Type))
                    {
                        region.Claim(m);
                        return region;
                    }
                }
            }
            return null;
        }

        private void end()
        {
            foreach (var pr in m_pointRegions)
            {
                int score = RuleSet.GetEndScore(pr);
                if (score > 0)
                {
                    foreach (var o in pr.Owners)
                    {
                        o.Score += score;
                    }
                    pr.ReturnMeeple();
                }
            }
        }

        private void score(List<IPointRegion> changed)
        {
            State = GameState.Score;
            if (changed != null)
            {
                foreach (var pr in changed)
                {
                    if (pr.IsForcedOpened)
                    {
                        pr.IsForcedOpened = false;
                    }
                    int score = RuleSet.GetScore(pr);
                    if (score > 0)
                    {
                        foreach (var o in pr.Owners)
                        {
                            o.Score += score;
                        }
                        pr.ReturnMeeple();
                    }
                }
            }
        }

        private Tile draw()
        {
            if (m_deck.Count > 0)
            {
                var t = m_deck.Pop();
                ActiveTile = t;
            }
            else
            {
                ActiveTile = null;
            }
            return ActiveTile;
        }

        protected virtual void ResetGame()
        {
            Players.Clear();
            m_deck.Clear();
            populateDeck();
            Board.Clear();
            State = GameState.NotStarted;
        }

        private void populateDeck()
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
                .AddMonestary()
                );
            AddTile(2, builder.NewTile()
                .AddRoadRegion(EdgeDirection.South)
                .AddMonestary()
                );
        }

        public void Play()
        {
            if (State == GameState.NotStarted)
            {
                Shuffle();
                var useDefaultStart = true;
                foreach (var exp in m_expansions)
                {
                    if (exp.IgnoreDefaultStart)
                    {
                        useDefaultStart = false;
                        break;
                    }
                }
                if (useDefaultStart)
                {
                    place(m_defaultStartTile, new CarcassonneMove(0, 0, Rotation.None));
                }
                do
                {
                    var player = nextPlayer();
                    if (player != null)
                    {
                        if (draw() == null)
                        {
                            break;
                        }
                        State = GameState.Place;
                        List<IPointRegion> changed = null;
                        do
                        {
                            var mv = player.GetMove(this);
                            if (mv != null)
                            {
                                changed = place(mv);
                            }
                            else
                            {
                                State = GameState.Discard;
                                break;
                            }
                        } while (changed == null);
                        if (State == GameState.Place)
                        {
                            State = GameState.Claim;
                            IClaimable claimed = null;
                            do
                            {
                                var region = player.GetClaim(this, out MeepleType type);
                                if (region != null)
                                {
                                    claimed = Claim(region, type);
                                }
                                else
                                {
                                    //Skip the claim state.
                                    break;
                                }
                            } while (claimed == null);
                            score(changed);
                        }
                    }
                } while (ActiveTile != null);
                end();
                State = GameState.End;
            }
        }

        public void Shuffle()
        {
            foreach (var e in m_expansions)
            {
                e.BeforeDeckShuffle(m_deck);
            }
            m_deck.Shuffle();
            foreach (var e in m_expansions)
            {
                e.AfterDeckShuffle(m_deck);
            }
        }

        protected void AddTile(int count, Tile tile)
        {
            for (int i = 1; i < count; i++)
            {
                m_deck.Push(tile.TileClone());
            }
            m_deck.Push(tile);
        }

        private List<IPointRegion> place(CarcassonneMove move)
        {
            List<IPointRegion> changed = null;
            if (tryFit(ActiveTile, move))
            {
                changed = place(ActiveTile, move);
            }
            return changed;
        }

        private List<IPointRegion> place(Tile tile, CarcassonneMove move)
        {
            var available = new List<EdgeDirection>((EdgeDirection[])Enum.GetValues(typeof(EdgeDirection)));
            var changedPointRegions = new List<IPointRegion>();

            Board.Add(new Placement<Tile, CarcassonneMove>(tile, move));
            if (tile.TileRegion != null)
            {
                m_pointRegions.Add(tile.TileRegion);
                changedPointRegions.Add(tile.TileRegion);
            }
            var allNeighbors = Board.GetAllNeighbors(move.Location);
            foreach (var n in allNeighbors)
            {
                if (n.TileRegion != null)
                {
                    n.TileRegion.Add(tile);
                    changedPointRegions.Add(n.TileRegion);
                }
                if (tile.TileRegion != null)
                {
                    tile.TileRegion.Add(n);
                }
            }
//            var neighbors = Board.GetNeighbors(move.Location);
            while (available.Count > 0)
            {
                var r = tile.GetRegion(available[0]);
                if (r != null)
                {
                    var regions = new List<PointRegion>();
                    foreach (var d in r.Edges)
                    {
                        var n=Board.GetNeighbor(move.Location, d);
                        if (n != null)
                        {
                            var nr = n.GetRegion(d.Opposite());
                            if (nr?.Container is PointRegion pr)
                            {
                                regions.Add(pr);
                            }
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
                            if (m_pointRegions.Contains(dup))
                            {
                                m_pointRegions.Remove(dup);
                            }
                        }
                        regions[0].Add(r);
                        if (!m_pointRegions.Contains(regions[0]))
                        {
                            m_pointRegions.Add(regions[0]);
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
                                m_pointRegions.Add(cpr);
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
                                m_pointRegions.Add(rpr);
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
        /// <param name="move">Proposed location and rotation of ActiveTile</param>
        /// <returns>true if the ActiveTile would fit at the current location.</returns>
        private bool tryFit(Tile tile, CarcassonneMove move)
        {
            return RuleSet.Fits(Board, tile, move);
        }

        public Player AddPlayer(string name)
        {
            if (State == GameState.NotStarted)
            {
                var p = new Player(name);
                Players.Add(p);
                RuleSet.UpdatePlayer(p);
                return p;
            }
            return null;
        }
    }
}
