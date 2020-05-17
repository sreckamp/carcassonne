namespace Carcassonne.Model
{
    public interface IClaimProvider
    {
        // ReSharper disable once UnusedParameter.Global
        (IClaimable, MeepleType) GetClaim(Game game);
    }

    public class EmptyClaimProvider : IClaimProvider
    {
        public static readonly IClaimProvider Instance = new EmptyClaimProvider();

        private EmptyClaimProvider()
        {
        }

        public (IClaimable, MeepleType) GetClaim(Game game)
        {
            return (DefaultClaimable.Instance, MeepleType.None);
        }
    }
}
