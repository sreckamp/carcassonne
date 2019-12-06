namespace Carcassonne.Model.Rules
{
    public interface IClaimRule
    {
        bool Applies(IClaimable region, MeepleType type);
        bool IsAvailable(IClaimable region, MeepleType type);
    }
}
