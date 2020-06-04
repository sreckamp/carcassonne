namespace Carcassonne.Model
{
    public class Meeple :IMeeple
    {
        public Meeple(MeepleType type, IPlayer player)
        {
            Type = type;
            Player = player;
        }

        public IPlayer Player { get; }
        public MeepleType Type { get; }

        public override string ToString() => $"{Type} ({Player.Name})";
    }
}
