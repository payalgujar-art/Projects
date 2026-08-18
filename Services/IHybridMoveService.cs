namespace TicTacToe.API.Services
{
    public interface IHybridMoveService
    {
        Task<int> GetNextMoveAsync(string[] board);
    }
}
