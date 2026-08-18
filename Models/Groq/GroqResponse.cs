using System.Text.Json.Serialization;

namespace TicTacToe.API.Models.Groq
{
    public class GroqResponse
    {
        [JsonPropertyName("choices")]
        public List<GroqChoice> Choices { get; set; } = new();
    }

    public class GroqChoice
    {
        [JsonPropertyName("message")]
        public GroqResponseMessage Message { get; set; } = new();
    }

    public class GroqResponseMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }

}
