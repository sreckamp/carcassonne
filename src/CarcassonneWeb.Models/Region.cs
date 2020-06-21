using System.Collections.Generic;

namespace CarcassonneWeb.Models
{
    public class Region
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string RegionId { get; set; }
        public string Meeple { get; set; }
        public string Type { get; set; }
        public IEnumerable<Edge> Edges { get; set; }
    }
}