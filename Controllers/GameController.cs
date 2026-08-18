using Microsoft.AspNetCore.Mvc;
using TicTacToe.API.Models;
using TicTacToe.API.Services;

namespace TicTacToe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController(GameService gameService) : ControllerBase
    {
        private readonly GameService _gameService = gameService;

        [HttpPost("start")]
        public IActionResult StartGame()
        {
            var game = _gameService.CreateGame();
            return Ok(game);
        }

        [HttpPost("move")]
        public async Task<IActionResult> MakeMove(MoveRequest request)
        {
            var game = await _gameService.MakeMove(request);
            if (game == null)
                return NotFound();

            return Ok(game);
        }
    }
}
