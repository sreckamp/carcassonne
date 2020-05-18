namespace Carcassonne.Model
{
    public interface IMeeple
    {
        IPlayer Player { get; }

        MeepleType Type { get; }
    }

    public class NopMeeple : IMeeple
    {
        public static readonly IMeeple Instance = new NopMeeple();

        private NopMeeple()
        {
        }

        /// <inheritdoc />
        public IPlayer Player { get; } = NopPlayer.Instance;

        /// <inheritdoc />
        public MeepleType Type { get; } = MeepleType.None;
    }
}