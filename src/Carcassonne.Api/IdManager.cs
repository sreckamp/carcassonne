using System;
using System.Collections.Generic;

namespace Carcassonne.Api
{
    public static class IdManager
    {
        private static readonly List<string> SIds = new List<string>();
        private static readonly Random SRandom = new Random();

        public static string Next()
        {
            string id;
            lock(SIds)
            {
                do
                {
                    id = SRandom.Next().ToString("00000000").Substring(0, 8);
                } while (SIds.Contains(id));

                SIds.Add(id);
            }
            return id;
        }
    }
}