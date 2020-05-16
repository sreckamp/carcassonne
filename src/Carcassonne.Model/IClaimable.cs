namespace Carcassonne.Model
{
    public interface IClaimable
    {
        void Claim(Meeple meeple);
        void ResetClaim();
        Meeple Claimer { get; }
        bool IsClosed { get; }
    }

    public class DefaultClaimable : IClaimable
    {
        public static readonly IClaimable Instance = new DefaultClaimable();

        private DefaultClaimable(){ }
        public void Claim(Meeple meeple)
        {
        }

        public void ResetClaim()
        {
        }

        public Meeple Claimer { get; } = Meeple.None;
        public bool IsClosed { get; } = false;
    }
}
