namespace Carcassonne.Model
{
    public class Meeple
    {
        public Meeple(MeepleType type, Player player)
        {
            Type = type;
            Player = player;
        }

        public Player Player { get; private set; }
        public MeepleType Type { get; private set; }

        public void Return()
        {
            Player.ReturnMeeple(this);
        }
    }
}
