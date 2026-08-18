namespace TicTacToe.API.Interfaces
{
    public interface IAiMoveProvider
    {
        Task<int> GetNextMoveAsync(string[] board);
    }
}
