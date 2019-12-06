namespace Carcassonne.Model
{
    public interface IClaimProvider
    {
        IClaimable GetClaim(Game game, out MeepleType type);
    }
}
