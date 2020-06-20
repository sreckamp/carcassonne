using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Next,
        End
    }

    /// <summary>
    /// </summary>
    public sealed class Game
    {
        public RuleSet RuleSet { get; }

        public Game(IEnumerable<ExpansionPack> expansions)
        {
            ActiveTileChanged += (sender, args) => { };
            ActivePlayerChanged += (sender, args) => { };
            GameStateChanged += (sender, args) => { };
            RuleSet = new RuleSet(expansions);
            Board = new Board();
            Players = new ObservableList<IPlayer>();
        }

        public Board Board { get; }

        public IEnumerable<IPointContainer> PointRegions { get; } = new ObservableList<IPointContainer>();

        private readonly Deck m_deck = new Deck();

        public event EventHandler<ChangedValueArgs<GameState>> GameStateChanged;
        private GameState m_state = GameState.NotStarted;

        public GameState State
        {
            get => m_state;
            private set
            {
                var old = m_state;
                m_state = value;
                GameStateChanged.Invoke(this, new ChangedValueArgs<GameState>(old, value));
            }
        }

        public event EventHandler<ChangedValueArgs<ITile>> ActiveTileChanged;
        private ITile m_activeTile = NopTile.Instance;

        public ITile ActiveTile
        {
            get => m_activeTile;
            private set
            {
                var old = m_activeTile;
                m_activeTile = value;
                ActiveTileChanged.Invoke(this, new ChangedValueArgs<ITile>(old, value));
            }
        }

        public void DumpDeck(IList<ITile> target)
        {
            while (m_deck.Count > 0)
            {
                target.Add(m_deck.Pop());
            }
        }

        public event EventHandler<ChangedValueArgs<IPlayer>> ActivePlayerChanged;
        private int m_activePlayerIndex = int.MinValue;
        public IPlayer ActivePlayer => m_activePlayerIndex >= 0 ? Players[m_activePlayerIndex] : NopPlayer.Instance;

        public ObservableList<IPlayer> Players { get; }

        private void NextPlayer()
        {
            var old = ActivePlayer;

            if (++m_activePlayerIndex >= Players.Count || m_activePlayerIndex < 0)
            {
                m_activePlayerIndex = 0;
            }
            ActivePlayerChanged.Invoke(this, new ChangedValueArgs<IPlayer>(old, ActivePlayer));
        }

        public bool Claim(IClaimable claim, MeepleType type)
        {
            var m = ActivePlayer.GetMeeple(type);
            if (!claim.IsAvailable && m.Type == MeepleType.None || !RuleSet.IsAvailable(claim, m.Type)) return false;
            claim.Claim(m);
            return true;
        }

        public void End()
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

        private bool Draw()
        {
            while (m_deck.Count > 0)
            {
                var unplacable = new List<Tile>();
                var tile = m_deck.Pop();
                foreach (Rotation rotation in Enum.GetValues(typeof(Rotation)))
                {
                    var rotated = new RotatedTile(tile, rotation);
                    for (var y = Board.MinY - 1; y <= Board.MaxY + 1; y++)
                    {
                        for (var x = Board.MinX - 1; x <= Board.MaxX + 1; x++)
                        {
                            if (!TryFit(rotated, new Point(x, y))) continue;

                            ActiveTile = tile;
                            m_deck.ShuffleIn(unplacable);
                            return true;
                        }
                    }
                }
                unplacable.Add(tile);
            }

            ActiveTile = NopTile.Instance;

            return false;
        }

        public void Start()
        {
            State = GameState.NotStarted;
            foreach (var player in Players)
            {
                player.Reset();
                RuleSet.UpdatePlayer(player);
            }
            Shuffle();
            Draw();
            Board.Clear();
            Place(ActiveTile, new Point(0, 0), new List<IPointContainer>());
            NextPlayer();
            Draw();
            State = GameState.Place;
        }

        // public void Play()
        // {
        //     Start();
        //     do
        //     {
        //         do
        //         {
        //             var mv = ActivePlayer.GetMove(this);
        //             ApplyMove(mv.Location, mv.Rotation);
        //         } while (State == GameState.Place);
        //         // var changed = new List<IPointContainer>();
        //         // bool placed;
        //         // do
        //         // {
        //         //     if (!mv.IsEmpty)
        //         //     {
        //         //         placed = Place(mv, changed);
        //         //     }
        //         //     else
        //         //     {
        //         //         placed = true;
        //         //         State = GameState.Discard;
        //         //     }
        //         // } while (!placed);
        //
        //         //TODO: Get available regions here (including closed ones on the most recently placed & possibly any open one, or removals, etc.
        //         do
        //         {
        //             var (c, meeple) = ActivePlayer.GetClaim(this);
        //             ApplyClaim(c, meeple);
        //         } while (State == GameState.Claim);
        //
        //         //TODO: Collect closed regions with 
        //         // bool claimed;
        //         // do
        //         // {
        //         //     var (claim, type) = player.GetClaim(this);
        //         //     if (claim.IsAvailable)
        //         //     {
        //         //         claimed = Claim(claim, type);
        //         //     }
        //         //     else
        //         //     {
        //         //         //Skip the claim state.
        //         //         break;
        //         //     }
        //         // } while (!claimed);
        //         Score();
        //         NextTurn();
        //     } while (State != GameState.End);
        //     End();
        // }

        public void Shuffle()
        {
            RuleSet.BeforeDeckShuffle(m_deck);
            m_deck.Shuffle();
            RuleSet.AfterDeckShuffle(m_deck);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NextTurn()
        {
            if(State != GameState.Next) throw new InvalidOperationException($"Cannot Change Turn in State {State}!");

            NextPlayer();
            
            State = Draw() ? GameState.Place : GameState.End;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ApplyMove(Point location, Rotation rotation)
        {
            if(State != GameState.Place) throw new InvalidOperationException($"Cannot Place in State {State}!");
            if (Place(new Move(location, rotation), new List<IPointContainer>()))
            {
                State = GameState.Claim;
            }
        }

        public void ApplyClaim(IClaimable claim, MeepleType type)
        {
            if(State != GameState.Claim) throw new InvalidOperationException($"Cannot Claim in State {State}!");
            State = GameState.Score;
        }

        public void Score()
        {
            if(State != GameState.Score) throw new InvalidOperationException($"Cannot Score in State {State}!");

            var toScore = Board.Placements
                .Select(p => p.Piece)
                .SelectMany(t => t.Regions, (t, r) => r.Container)
                .Union(Board.Placements.Select(p=>p.Piece.TileRegion).Where(tr => tr is IPointContainer).Cast<IPointContainer>())
                .Distinct()
                .Where(p => p.IsClosed && p.Owners.Any()).ToList();
            Debug.WriteLine(toScore.Count);

            foreach (var pr in toScore)
            {
                var score = RuleSet.GetScore(pr);
                if (score <= 0) continue;
                foreach (var o in pr.Owners)
                {
                    o.Score += score;
                }
                pr.ReturnMeeple();
            }

            State = GameState.Next;
        }

        private bool Place(Move move, IList<IPointContainer> changed)
        {
            var tile = new RotatedTile(ActiveTile, move.Rotation);
            if (!TryFit(tile, move.Location)) return false;
            Place(tile, move.Location, changed);
            return true;
        }

        private void Place(ITile tile, Point location, ICollection<IPointContainer> changes)
        {

            Board.Add(new Placement<ITile>(tile, location));

            foreach (EdgeDirection dir in Enum.GetValues(typeof(EdgeDirection)))
            {
                RuleSet.Join(tile, Board.GetNeighbor(location, dir), dir);
            }
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

        public IPlayer AddPlayer(string name, Color color)
        {
            if (State != GameState.NotStarted) return NopPlayer.Instance;
            if (color == Color.None)
            {
                color = Enum.GetValues(typeof(Color))
                    .Cast<Color>().First(c => c != Color.None && Players.All(plr => plr.Color != c));
            }
            var p = new Player(name, color);
            Players.Add(p);
            return p;
        }
    }
}
