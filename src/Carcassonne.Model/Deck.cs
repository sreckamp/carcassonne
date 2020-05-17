using System;
using System.Collections.Generic;

namespace Carcassonne.Model
{

    public class Deck : ObservableStack<Tile>
    {
        public void Shuffle()
        {
            var rnd = new Random();
            var count = 5 + rnd.Next(10);
            for (var i = 0; i < count; i++)
            {
                rnd = new Random();
                var toShuffle = new List<Tile>(this);
                Clear();
                while (toShuffle.Count > 0)
                {
                    var idx = rnd.Next(toShuffle.Count);
                    Push(toShuffle[idx]);
                    toShuffle.RemoveAt(idx);
                }
            }
        }
    }
}
