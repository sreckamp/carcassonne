namespace Carcassonne.Model
{
    public interface IMoveProvider
    {
        // ReSharper disable once UnusedParameter.Global
        Move GetMove(Game game);
    }

    public class EmptyMoveProvider : IMoveProvider
    {
        public static readonly IMoveProvider Instance = new EmptyMoveProvider();
        private EmptyMoveProvider()
        {
        }

        public Move GetMove(Game game)
        {
            return Move.None;
        }
    }
}
