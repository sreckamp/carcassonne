using System.Collections.Generic;
using System.Linq;

namespace Carcassonne.Model
{
    /// <summary>
    /// Container used to hold a group of regions that score points.
    /// </summary>
    public interface IPointContainer
    {
        /// <summary>
        /// If true, the container is ready to be scored.
        /// </summary>
        bool IsClosed { get; }

        /// <summary>
        /// The player(s) who own the Container (have the most Meeple on it)
        /// </summary>
        IEnumerable<IPlayer> Owners { get; }

        /// <summary>
        /// Recalculate the owners
        /// TODO: Make this a rule?
        /// </summary>
        void UpdateOwners();

        // /// <summary>
        // /// Give any placed Meeple back to the player
        // /// </summary>
        void ReturnMeeple();
    }

    public class NopPointContainer : IPointContainer
    {
        public static readonly IPointContainer Instance = new NopPointContainer();

        private NopPointContainer() { }

        /// <inheritdoc />
        public bool IsClosed => true;

        /// <inheritdoc />
        public IEnumerable<IPlayer> Owners { get; } = Enumerable.Empty<IPlayer>();

        /// <inheritdoc />
        public void UpdateOwners()
        {
        }

        /// <inheritdoc />
        public void ReturnMeeple()
        {
        }
    }
}
