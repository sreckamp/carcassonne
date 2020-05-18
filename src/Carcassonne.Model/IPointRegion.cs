using System.Collections.Generic;

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

        /// TODO:May be better to have a "HasMeeple" flag
        /// <summary>
        /// I think this is to be "forced" open to allow scoring.
        /// </summary>
        bool IsForcedOpened { get; set; }

        /// <summary>
        /// The player(s) who own the Container (have the most Meeple on it)
        /// </summary>
        List<Player> Owners { get; }
        
        /// <summary>
        /// Give any placed Meeple back to the player
        /// </summary>
        void ReturnMeeple();
    }
}
