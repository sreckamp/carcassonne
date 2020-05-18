namespace Carcassonne.Model.Rules
{
    public interface IScoreRule
    {
        bool Applies(IPointContainer region);
        int GetScore(IPointContainer region);
        int GetEndScore(IPointContainer region);
    }
}
