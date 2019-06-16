using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Carcassonne.Model
{

    public class Deck : ObservableStack<Tile>
    {
        public void Shuffle()
        {
            //var rnd = new Random();
            //int count = 5 + rnd.Next(10);
            //for (int i = 0; i < count; i++)
            //{
            //    rnd = new Random();
            //    var toShuffle = new List<Tile>(this);
            //    Clear();
            //    while (toShuffle.Count > 0)
            //    {
            //        int idx = rnd.Next(toShuffle.Count);
            //        Push(toShuffle[idx]);
            //        toShuffle.RemoveAt(idx);
            //    }
            //}
        }
    }
}
