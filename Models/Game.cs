namespace TicTacToe.API.Models
{
    public class Game
    {
        public Guid Id { get; set; }

        public string[] Board { get; set; } =
        [
            "", "", "",
            "", "", "",
            "", "", ""
        ];

        public bool IsGameOver { get; set; }

        public string Winner { get; set; } = "";
    }
}
