using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using TicTacToe.API.Interfaces;
using TicTacToe.API.Models.Ollama;

namespace TicTacToe.API.Services
{
    public class OllamaService : IAiMoveProvider
    {
        private readonly HttpClient _httpClient;

        public OllamaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetNextMoveAsync(string[] board)
        {
            var prompt = BuildPrompt(board);

            var request = new OllamaRequest
            {
                Model = "gemma3:1b",
                Prompt = prompt,
                Stream = false
            };

            var json = JsonSerializer.Serialize(request);

            var response = await _httpClient.PostAsync(
                "http://localhost:11434/api/generate",
                new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(result);

            if (ollamaResponse == null)
                return -1;

            Console.WriteLine("LLM Response: " + ollamaResponse.Response);

            var match = Regex.Match(ollamaResponse.Response, @"\d");

            if (!match.Success)
                return -1;

            return int.Parse(match.Value);
        }

        private static string BuildPrompt(string[] board)
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are an expert Tic Tac Toe player.");

            sb.AppendLine();

            sb.AppendLine("Board:");

            for (int i = 0; i < board.Length; i++)
            {
                sb.AppendLine($"{i}:{(string.IsNullOrWhiteSpace(board[i]) ? "-" : board[i])}");
            }

            sb.AppendLine();

            sb.AppendLine("You are playing as O.");

            sb.AppendLine("Return ONLY one number between 0 and 8.");

            sb.AppendLine("Do not explain.");

            return sb.ToString();
        }
    }
}
