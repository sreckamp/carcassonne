using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carcassonne.Model;
using Game = Carcassonne.Model.Game;
using GameDto = CarcassonneWeb.Models.Game;
using PlayerDto = CarcassonneWeb.Models.Player;
using ColorDto = CarcassonneWeb.Models.Color;

namespace CarcassonneServer.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<string, Game> m_games = new Dictionary<string, Game>();
        private readonly Random m_random = new Random();

        public async Task<GameDto> GetAsync(string id)
        {
            await Task.CompletedTask;

            return ToGameDto(id, m_games[id]);
        }

        public async Task<GameDto> FindAsync(string name, Color color)
        {
            var (id, game) = m_games.FirstOrDefault(
                kvp => kvp.Value.State == GameState.NotStarted
                       && (color == Color.None || !kvp.Value.Players.Any(p => p.Color == color)));

            if (id == null)
            {
                do
                {
                    id = m_random.Next().ToString("00000000").Substring(0, 8);
                } while (m_games.ContainsKey(id));

                m_games[id] = game = new Game(Enumerable.Empty<ExpansionPack>());
            }

            game.AddPlayer(name, color);

            await Task.CompletedTask;

            return ToGameDto(id, game);
        }

        private GameDto ToGameDto(string id, Game game) => new GameDto
        {
            Id = id,
            Players = game.Players.Select(ToPlayerDto).ToList()
        };

        private PlayerDto ToPlayerDto(IPlayer player) => new PlayerDto
        {
            Name = player.Name,
            Color = ToColorDto(player.Color)
        };

        private ColorDto ToColorDto(Color color) => color switch
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
    }
}
