﻿using System.Collections.Generic;

namespace Carcassonne.Dto
{
    public class Player
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public int Score { get; set; }
        public Dictionary<string, int> MeepleCount { get; set; } = new Dictionary<string, int>();
    }
}