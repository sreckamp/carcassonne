namespace Carcassonne.Model
{
    public class Meeple
    {
        public static readonly Meeple None = new Meeple(MeepleType.None, Player.None);

        public Meeple(MeepleType type, Player player)
        {
            Type = type;
            Player = player;
        }

        public Player Player { get; }
        public MeepleType Type { get; }

        public void Return()
        {
            Player.ReturnMeeple(this);
        }
    }
}
