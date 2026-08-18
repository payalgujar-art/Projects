using TicTacToe.API.Interfaces;

namespace TicTacToe.API.Services
{
    public class MinimaxService : IMinimaxService
    {
        private const string AI = "O";
        private const string Human = "X";

        public int GetBestMove(string[] board)
        {
            int bestScore = int.MinValue;
            int bestMove = -1;

            for (int i = 0; i < 9; i++)
            {
                if (string.IsNullOrWhiteSpace(board[i]))
                {
                    board[i] = AI;

                    int score = Minimax(board, false);

                    board[i] = string.Empty;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = i;
                    }
                }
            }

            return bestMove;
        }

        private int Minimax(string[] board, bool isMaximizing)
        {
            if (HasWon(board, AI))
                return 10;

            if (HasWon(board, Human))
                return -10;

            if (IsBoardFull(board))
                return 0;

            if (isMaximizing)
            {
                int bestScore = int.MinValue;

                for (int i = 0; i < 9; i++)
                {
                    if (string.IsNullOrWhiteSpace(board[i]))
                    {
                        board[i] = AI;

                        bestScore = Math.Max(bestScore, Minimax(board, false));

                        board[i] = string.Empty;
                    }
                }

                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;

                for (int i = 0; i < 9; i++)
                {
                    if (string.IsNullOrWhiteSpace(board[i]))
                    {
                        board[i] = Human;

                        bestScore = Math.Min(bestScore, Minimax(board, true));

                        board[i] = string.Empty;
                    }
                }

                return bestScore;
            }
        }

        private static bool IsBoardFull(string[] board)
        {
            return board.All(c => !string.IsNullOrWhiteSpace(c));
        }

        private static bool HasWon(string[] board, string player)
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
                if (board[wins[i, 0]] == player &&
                    board[wins[i, 1]] == player &&
                    board[wins[i, 2]] == player)
                    return true;
            }

            return false;
        }
    }
}
