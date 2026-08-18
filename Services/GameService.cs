using TicTacToe.API.Interfaces;
using TicTacToe.API.Models;

namespace TicTacToe.API.Services
{
    public class GameService
    {
        private static readonly Dictionary<Guid, Game> Games = new();

        private readonly IHybridMoveService _hybridMoveService;

        public GameService(IHybridMoveService hybridMoveService)
        {
            _hybridMoveService = hybridMoveService;
        }

        public Game CreateGame()
        {
            var game = new Game
            {
                Id = Guid.NewGuid()
            };

            Games[game.Id] = game;

            return game;
        }

        public Game? GetGame(Guid id)
        {
            Games.TryGetValue(id, out var game);
            return game;
        }

        public async Task<Game?> MakeMove(MoveRequest request)
        {
            if (!Games.TryGetValue(request.GameId, out var game))
                return null;

            if (game.IsGameOver)
                return game;

            if (request.Position < 0 || request.Position > 8)
                return game;

            if (!string.IsNullOrWhiteSpace(game.Board[request.Position]))
                return game;

            // Human Move (X)
            game.Board[request.Position] = "X";

            if (CheckWinner(game, "X"))
            {
                game.IsGameOver = true;
                game.Winner = "X";
                return game;
            }

            if (IsBoardFull(game.Board))
            {
                game.IsGameOver = true;
                game.Winner = "Draw";
                return game;
            }

            // AI Move (O)
            int aiMove = await _hybridMoveService.GetNextMoveAsync(game.Board);

            if (aiMove >= 0)
            {
                game.Board[aiMove] = "O";
            }

            if (CheckWinner(game, "O"))
            {
                game.IsGameOver = true;
                game.Winner = "O";
            }
            else if (IsBoardFull(game.Board))
            {
                game.IsGameOver = true;
                game.Winner = "Draw";
            }

            return game;
        }

        private static bool IsBoardFull(string[] board)
        {
            return board.All(c => !string.IsNullOrWhiteSpace(c));
        }

        private static bool CheckWinner(Game game, string player)
        {
            int[,] wins =
            {
                {0,1,2},
                {3,4,5},
                {6,7,8},

                {0,3,6},
                {1,4,7},
                {2,5,8},

                {0,4,8},
                {2,4,6}
            };

            for (int i = 0; i < wins.GetLength(0); i++)
            {
                if (game.Board[wins[i, 0]] == player &&
                    game.Board[wins[i, 1]] == player &&
                    game.Board[wins[i, 2]] == player)
                {
                    return true;
                }
            }

            return false;
        }
    }
}