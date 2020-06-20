namespace Carcassonne.Model
{
    public interface IPlayer
    {
        string Name { get; }
        Color Color { get; }
        int Score { get; set; }
        IMoveProvider MoveChooser { get; set; }
        IClaimProvider ClaimChooser { get; set; }
        MeepleCollection Meeple { get; }

        IMeeple GetMeeple(MeepleType type);
        void ReturnMeeple(IMeeple meeple);
        // void AddMeeple(IEnumerable<IMeeple> meeple);
        Move GetMove(Game game);
        // (IClaimable, MeepleType) GetClaim(Game game);

        void Reset();
    }

    public class NopPlayer : IPlayer
    {
        public static readonly IPlayer Instance = new NopPlayer();

        private NopPlayer()
        {
        }

        /// <inheritdoc />
        public string Name => string.Empty;

        /// <inheritdoc />
        public Color Color => Color.None;

        /// <inheritdoc />
        public int Score { get; set; }

        /// <inheritdoc />
        public IMoveProvider MoveChooser { get; set; } = EmptyMoveProvider.Instance;

        /// <inheritdoc />
        public IClaimProvider ClaimChooser { get; set; } = EmptyClaimProvider.Instance;

        /// <inheritdoc />
        public MeepleCollection Meeple { get; } = new MeepleCollection();

        /// <inheritdoc />
        public IMeeple GetMeeple(MeepleType type) => NopMeeple.Instance;

        /// <inheritdoc />
        public void ReturnMeeple(IMeeple meeple)
        {
        }

        /// <inheritdoc />
        public Move GetMove(Game game) => Move.None;

        public void Reset()
        {
        }
    }
}
