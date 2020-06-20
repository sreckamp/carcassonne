using System.Threading.Tasks;
using Carcassonne.Model;
using CarcassonneServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Game = CarcassonneWeb.Models.Game;

namespace CarcassonneServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> m_logger;
        private readonly IGameService m_gameService;

        public GameController(ILogger<GameController> logger, IGameService gameService)
        {
            m_logger = logger;
            m_gameService = gameService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Game> Get([FromRoute] string id)
        {
            return await m_gameService.GetAsync(id);
        }

        [HttpGet]
        [Route("")]
        public async Task<Game> FindGame([FromQuery] string name, [FromQuery] Color color = Color.None)
        {
            return await m_gameService.FindAsync(name, color);
        }
    }
}