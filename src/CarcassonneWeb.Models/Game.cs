using System;
using System.Collections.Generic;

namespace CarcassonneWeb.Models
{
    public class Game
    {
        public string Id { get; set; }
        public IEnumerable<Player> Players { get; set; } = new List<Player>();
        public State State { get; set; } = State.WaitingForPlayers;
    }
}
