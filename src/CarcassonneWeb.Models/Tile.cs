using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CarcassonneWeb.Models
{
    public class Tile
    {
        public string Id { get; set; }
        
        public Point? Location { get; set; }
        [JsonIgnore]
        public bool ShouldSerializeLocation => Location.HasValue;
        public Rotation? Rotation { get; set; }
        [JsonIgnore]
        public bool ShouldSerializeRotation => Rotation.HasValue;
        public IEnumerable<string> Features { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public string Temp { get; set; }
    }

    public class PointJsonConverter : JsonConverter<Point>
    {
        private static readonly Regex SPointRegex = new Regex(@"\((-?\d+),(-?\d+)\)");
        public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var @string = reader.GetString();
            var m = SPointRegex.Match(@string);
            return new Point(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
        }

        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"({value.X},{value.Y})");
        }
    }
}