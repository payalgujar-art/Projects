namespace TicTacToe.API.Models.Ollama
{
    public class OllamaRequest
    {
        public string Model { get; set; } = string.Empty;

        public string Prompt { get; set; } = string.Empty;

        public bool Stream { get; set; } = false;
    }
}
