using System.Threading.Tasks;
using Carcassonne.Model;
using Game = CarcassonneWeb.Models.Game;

namespace CarcassonneServer.Services
{
    public interface IGameService
    {
        Task<Game> GetAsync(string id);
        Task<Game> FindAsync(string name, Color color);
    }
}
