namespace Carcassonne.Model
{
    public interface IMoveProvider
    {
        // ReSharper disable once UnusedParameter.Global
        CarcassonneMove GetMove(Game game);
    }

    public class EmptyMoveProvider : IMoveProvider
    {
        public static readonly IMoveProvider Instance = new EmptyMoveProvider();
        private EmptyMoveProvider()
        {
        }

        public CarcassonneMove GetMove(Game game)
        {
            return CarcassonneMove.None;
        }
    }
}
