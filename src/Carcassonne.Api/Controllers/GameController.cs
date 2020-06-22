using System.Threading.Tasks;
using Carcassonne.Api.Services;
using Carcassonne.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Color = Carcassonne.Model.Color;
using Game = Carcassonne.Dto.Game;

namespace Carcassonne.Api.Controllers
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
        [Route("{gameId}")]
        public async Task<Game> Get([FromRoute] string gameId)
        {
            return await m_gameService.GetAsync(gameId);
        }

        [HttpGet]
        [Route("{gameId}/wait")]
        public async Task<Game> Wait([FromRoute] string gameId, [FromQuery] string session)
        {
            return await m_gameService.WaitAsync(session);
        }

        [HttpGet]
        [Route("{gameId}/start")]
        public async Task<Game> Start([FromRoute] string gameId)
        {
            return await m_gameService.StartAsync(gameId);
        }

        [HttpPost]
        [Route("{gameId}/place")]
        public async Task<Game> Place([FromRoute] string gameId, [FromBody] PlaceCommand command)
        {
            return await m_gameService.PlaceAsync(gameId, command.Session, command.Location, command.Rotation);
        }

        [HttpPost]
        [Route("{gameId}/claim")]
        public async Task<Game> Claim([FromRoute] string gameId, [FromBody] ClaimCommand command)
        {
            return await m_gameService.ClaimAsync(gameId, command.Session, command.MeepleType);
        }

        [HttpGet]
        [Route("")]
        public async Task<Session> FindGame([FromQuery] string name, [FromQuery] Color color = Color.None)
        {
            return await m_gameService.FindAsync(name, color);
        }
    }
}