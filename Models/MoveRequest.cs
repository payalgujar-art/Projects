namespace TicTacToe.API.Models
{
    public class MoveRequest
    {
        public Guid GameId { get; set; }

        public int Position { get; set; }
    }
}
