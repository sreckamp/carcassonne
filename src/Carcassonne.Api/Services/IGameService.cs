using System.Drawing;
using System.Threading.Tasks;
using Carcassonne.Dto;
using Color = Carcassonne.Model.Color;

namespace Carcassonne.Api.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Get the state of the game
        /// </summary>
        /// <param name="gameId">The id of the game.</param>
        /// <returns></returns>
        Task<Game> GetAsync(string gameId);

        /// <summary>
        /// Wait for a change in the state of the game
        /// </summary>
        /// <param name="sessionId">The id of the player session</param>
        /// <returns></returns>
        Task<Game> WaitAsync(string sessionId);

        /// <summary>
        /// Start the game
        /// </summary>
        /// <param name="gameId">The id of the game.</param>
        /// <returns></returns>
        Task<Game> StartAsync(string gameId);

        /// <summary>
        /// Find a game to join
        /// </summary>
        /// <param name="name">The players name</param>
        /// <param name="color">The color to use.  <see cref="Carcassonne.Model.Color.None"/> to assign a color.</param>
        /// <returns></returns>
        Task<Session> FindAsync(string name, Color color);

        /// <summary>
        /// Place the active tile
        /// </summary>
        /// <param name="gameId">The id of the game</param>
        /// <param name="sessionId">The id of the player session</param>
        /// <param name="location">The location to place the tile</param>
        /// <param name="rotation">The rotation to apply to the tile</param>
        /// <returns></returns>
        Task<Game> PlaceAsync(string gameId, string sessionId, Point location, Rotation rotation);

        /// <summary>
        /// Claim a region
        /// </summary>
        /// <param name="gameId">The id of the game</param>
        /// <param name="sessionId">The id of the player session</param>
        /// <param name="meepleType">The type of meeple</param>
        /// <returns></returns>
        Task<Game> ClaimAsync(string gameId, string sessionId, string meepleType);
    }
}
