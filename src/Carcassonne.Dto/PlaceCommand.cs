using System.Drawing;

namespace Carcassonne.Dto
{
    public class PlaceCommand
    {
        public string Session { get; set; }
        public Point Location { get; set; }
        public Rotation Rotation { get; set; } = Rotation.None;
    }
}