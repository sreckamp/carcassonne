using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Carcassonne.Model;
using CarcassonneWeb.Models;
using GameBase.Model;
using Color = Carcassonne.Model.Color;
using Game = Carcassonne.Model.Game;
using GameDto = CarcassonneWeb.Models.Game;
using PlayerDto = CarcassonneWeb.Models.Player;
using ColorDto = CarcassonneWeb.Models.Color;
using Rotation = Carcassonne.Model.Rotation;
using RotationDto = CarcassonneWeb.Models.Rotation;
using SessionDto = CarcassonneWeb.Models.Session;
using TileDto = CarcassonneWeb.Models.Tile;

namespace CarcassonneServer.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<string, Session> m_sessions = new Dictionary<string, Session>();
        private readonly IdMap<Game> m_gameMap = new IdMap<Game>();
        private readonly IdMap<ITile> m_tileMap = new IdMap<ITile>();

        /// <inheritdoc/>
        public Task<GameDto> GetAsync(string gameId)
        {
            return Task.FromResult(ToGameDto(m_gameMap[gameId]));
        }

        /// <inheritdoc/>
        public Task<GameDto> WaitAsync(string sessionId)
        {
            m_sessions[sessionId].WaitForStateChange();

            return Task.FromResult(ToGameDto(m_sessions[sessionId].Game));
        }

        /// <inheritdoc/>
        public Task<GameDto> StartAsync(string gameId)
        {
            if (m_gameMap[gameId].State != GameState.NotStarted)
            {
                throw new InvalidOperationException($"Game {gameId} is already started!");
            }

            m_gameMap[gameId].Start();

            return Task.FromResult(ToGameDto(m_gameMap[gameId]));
        }

        /// <inheritdoc/>
        public Task<GameDto> PlaceAsync(string gameId, string sessionId, Point location, RotationDto rotation)
        {
            var session = m_sessions[sessionId];
            var game = m_gameMap[gameId];

            if (game != session.Game)
            {
                throw new ArgumentException("Inappropriate game ID for the given session.");
            }
            if(session.Player != game.ActivePlayer)
            {
                throw new ArgumentException("Player session does not match active player.");
            }
            m_gameMap[gameId].ApplyMove(location, ToRotation(rotation));

            return Task.FromResult(ToGameDto(game));
        }

        /// <inheritdoc/>
        public Task<GameDto> ClaimAsync(string gameId, string sessionId, string meepleType)
        {
            var session = m_sessions[sessionId];
            var game = m_gameMap[gameId];

            if (game != session.Game)
            {
                throw new ArgumentException("Inappropriate game ID for the given session.");
            }
            if(session.Player != game.ActivePlayer)
            {
                throw new ArgumentException("Player session does not match active player.");
            }
            game.ApplyClaim(NopClaimable.Instance, Enum.Parse<MeepleType>(meepleType));
            game.Score();
            game.NextTurn();

            return Task.FromResult(ToGameDto(game));
        }

        /// <inheritdoc/>
        public async Task<SessionDto> FindAsync(string name, Color color)
        {
            var session = await Task.Run(() =>
            {
                var game = m_gameMap.FirstOrDefault(
                    g => g.State == GameState.NotStarted
                         && (g.Players.All(p =>
                             !p.Name.Equals(name) && (color == Color.None || p.Color != color))));

                if (game == null)
                {
                    game = new Game(Enumerable.Empty<ExpansionPack>());
                    m_gameMap.Add(game);
                }

                var player = game.AddPlayer(name, color);
                
                var s = new Session(game, player);
                m_sessions[s.Id] = s;

                return s;
            }).ConfigureAwait(false);

            return ToSessionDto(session);
        }

        private static Rotation ToRotation(RotationDto rotation) => rotation switch
        {
            RotationDto.None => Rotation.None,
            RotationDto.CounterClockwise => Rotation.CounterClockwise,
            RotationDto.UpsideDown => Rotation.UpsideDown,
            RotationDto.Clockwise => Rotation.Clockwise,
            _ => throw new ArgumentOutOfRangeException()
        };

        private SessionDto ToSessionDto(Session session) => new SessionDto
        {
            SessionId = session.Id,
            GameId = m_gameMap[session.Game],
            Name = session.Player.Name,
            Color = ToColorDto(session.Player.Color)
        };

        private GameDto ToGameDto(Game game) => new GameDto
        {
            Id = m_gameMap[game],
            State = ToState(game.State),
            Players = game.Players.Select(ToPlayerDto).ToList(),
            ActiveTile = ToTileDto(game.ActiveTile),
            ActivePlayerName = game.ActivePlayer.Name,
            Board = game.Board.Placements.Select(ToTileDto)
        };

        private static State ToState(GameState state)
        {
            return state switch
            {
                GameState.NotStarted => State.WaitingForPlayers,
                GameState.Place => State.Place,
                GameState.Claim => State.Claim,
                GameState.Score => State.Busy,
                GameState.Next => State.Busy,
                GameState.End => State.End,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private TileDto ToTileDto(ITile tile)
        {
            Rotation? rot = null;
            if (tile is RotatedTile rt)
            {
                rot = rt.Rotation;
            }

            if (!m_tileMap.Contains(tile))
            {
                m_tileMap.Add(tile);
            }

            return new TileDto
            {
                Id = m_tileMap[tile],
                Location = null,
                Rotation = ToRotationDto(rot),
                Temp = tile.ToString()
            };
        }

        private TileDto ToTileDto(Placement<ITile> placement)
        {
            var dto = ToTileDto(placement.Piece);
            dto.Location = placement.Location;
            return dto;
        }

        private static PlayerDto ToPlayerDto(IPlayer player) => new PlayerDto
        {
            Name = player.Name,
            Color = ToColorDto(player.Color),
            Score = player.Score,
            MeepleCount = player.Meeple.GroupBy(m=>m.Type).ToDictionary(g=>g.Key.ToString(), 
                g=>g.Count())
        };

        private static RotationDto? ToRotationDto(Rotation? rotation) => rotation switch
        {
            null => null,
            Rotation.None => RotationDto.None,
            Rotation.Clockwise => RotationDto.Clockwise,
            Rotation.UpsideDown => RotationDto.UpsideDown,
            Rotation.CounterClockwise => RotationDto.CounterClockwise,
            _ => RotationDto.None
        };

        private static ColorDto ToColorDto(Color color) => color switch
        {
            Color.Black => ColorDto.Black,
            Color.Red => ColorDto.Red,
            Color.Green => ColorDto.Green,
            Color.Yellow => ColorDto.Yellow,
            Color.Blue => ColorDto.Blue,
            Color.Pink => ColorDto.Pink,
            Color.Orange => ColorDto.Orange,
            Color.Gray => ColorDto.Gray,
            Color.None => ColorDto.Transparent,
            _ => ColorDto.Transparent
        };

        /// <summary>
        /// Particular User Session
        /// </summary>
        private class Session
        {
            private readonly ManualResetEventSlim m_lock = new ManualResetEventSlim();

            private State m_lastState = State.Busy;

            public Session(Game game, IPlayer player)
            {
                Id = IdManager.Next();
                Game = game;
                Player = player;
                Game.GameStateChanged += (sender, args) =>
                {
                    if (ToState(Game.State) == State.Busy) return;

                    m_lock.Set();
                };
            }

            public string Id { get; }
            public Game Game { get; }
            public IPlayer Player { get; }

            public void WaitForStateChange()
            {
                if (ToState(Game.State) == m_lastState)
                {
                    m_lock.Reset();
                    Debug.WriteLine($"Waiting for state to change.");
                    m_lock.Wait();
                    Debug.WriteLine($"State change detected.");
                }
                m_lastState = ToState(Game.State);
            }
        }
    }
}
