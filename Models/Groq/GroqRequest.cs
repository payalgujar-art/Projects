using System.Text.Json.Serialization;

namespace TicTacToe.API.Models.Groq
{
    public class GroqRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("messages")]
        public List<GroqRequestMessage> Messages { get; set; } = [];

        //Make output deterministic
       [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0;

        // Keep all valid choices
        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 1;

        // Only need a single number
        [JsonPropertyName("max_completion_tokens")]
        public int MaxCompletionTokens { get; set; } = 5;
    }

    public class GroqRequestMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
