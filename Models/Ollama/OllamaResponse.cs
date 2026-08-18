using System.Text.Json.Serialization;

namespace TicTacToe.API.Models.Ollama
{
    public class OllamaResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; } = string.Empty;
    }
}
