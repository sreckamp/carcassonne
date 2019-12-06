namespace Carcassonne.Model
{
    public interface IMoveProvider
    {
        CarcassonneMove GetMove(Game game);
    }
}
