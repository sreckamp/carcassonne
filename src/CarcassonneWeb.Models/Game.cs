using System;
using System.Collections.Generic;
using System.Linq;

namespace CarcassonneWeb.Models
{
    public class Game
    {
        public string Id { get; set; }
        public IEnumerable<Player> Players { get; set; } = Enumerable.Empty<Player>();

        public string ActivePlayerName { get; set; }
        public Tile ActiveTile { get; set; }
        public IEnumerable<Tile> Board { get; set; } = Enumerable.Empty<Tile>();
        public State State { get; set; } = State.WaitingForPlayers;
    }
}
