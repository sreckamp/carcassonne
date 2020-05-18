namespace Carcassonne.Model
{
    public interface IClaimable
    {
        /// <summary>
        /// Claim this with the given meeple.
        /// </summary>
        /// <param name="meeple"></param>
        void Claim(IMeeple meeple);

        /// <summary>
        /// Remove the claim on this.
        /// </summary>
        void ResetClaim();

        /// <summary>
        /// The meeple that is claiming this.
        /// </summary>
        IMeeple Claimer { get; }

        /// <summary>
        /// True when this is ready to receive a claim.
        /// </summary>
        bool IsAvailable { get; }
    }

    public class NopClaimable : IClaimable
    {
        public static readonly IClaimable Instance = new NopClaimable();

        private NopClaimable()
        {
        }

        public void Claim(IMeeple meeple)
        {
        }

        public void ResetClaim()
        {
        }

        public IMeeple Claimer { get; } = NopMeeple.Instance;

        /// <inheritdoc />
        public bool IsAvailable { get; } = false;
    }
}
