namespace Carcassonne.Model.Rules
{
    public interface IJoinRule
    {
        bool Applies(ITile newTile, ITile neighbor, EdgeDirection direction);
        void Join(ITile newTile, ITile neighbor, EdgeDirection direction);
    }
}
