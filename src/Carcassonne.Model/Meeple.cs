namespace Carcassonne.Model
{
    public class Meeple_ :IMeeple
    {
        public Meeple_(MeepleType type, IPlayer player)
        {
            Type = type;
            Player = player;
        }

        public IPlayer Player { get; }
        public MeepleType Type { get; }
    }
}
