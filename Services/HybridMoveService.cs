using TicTacToe.API.Interfaces;

namespace TicTacToe.API.Services
{
    public class HybridMoveService : IHybridMoveService
    {
        private readonly IAiMoveProvider _groq;
        private readonly IMinimaxService _minimax;

        public HybridMoveService(
            IAiMoveProvider groq,
            IMinimaxService minimax)
        {
            _groq = groq;
            _minimax = minimax;
        }

        public async Task<int> GetNextMoveAsync(string[] board)
        {
            int groqMove = await _groq.GetNextMoveAsync(board);

            if (!IsValidMove(board, groqMove))
            {
                Console.WriteLine("Groq returned an invalid move. Using Minimax.");

                return _minimax.GetBestMove(board);
            }

            int bestMove = _minimax.GetBestMove(board);

            if (groqMove == bestMove)
            {
                Console.WriteLine("Groq selected the optimal move.");

                return groqMove;
            }

            Console.WriteLine(
                $"Groq suggested {groqMove}, but Minimax recommends {bestMove}. Using Minimax.");

            return bestMove;
        }

        private static bool IsValidMove(string[] board, int move)
        {
            return move >= 0 &&
                   move < 9 &&
                   string.IsNullOrWhiteSpace(board[move]);
        }
    }
}
