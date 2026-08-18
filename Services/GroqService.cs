using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using TicTacToe.API.Interfaces;
using TicTacToe.API.Models.Groq;

namespace TicTacToe.API.Services
{
        public class GroqService : IAiMoveProvider
        {
            private readonly HttpClient _httpClient;
            private readonly IConfiguration _configuration;

            public GroqService(HttpClient httpClient, IConfiguration configuration)
            {
                _httpClient = httpClient;
                _configuration = configuration;

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        _configuration["Groq:ApiKey"]);
            }

        public async Task<int> GetNextMoveAsync(string[] board)
        {
            var request = new GroqRequest
            {
                Model = _configuration["Groq:Model"]!,
                Messages =
                [
                    new GroqRequestMessage
            {
                Role = "system",
                Content = GetSystemPrompt()
            },
            new GroqRequestMessage
            {
                Role = "user",
                Content = BuildUserPrompt(board)
            }
                ]
            };

            var json = JsonSerializer.Serialize(request);

            var response = await _httpClient.PostAsync(
                "https://api.groq.com/openai/v1/chat/completions",
                new StringContent(json, Encoding.UTF8, "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Status Code: " + response.StatusCode);
            Console.WriteLine("Response: " + result);

            response.EnsureSuccessStatusCode();

            var groqResponse = JsonSerializer.Deserialize<GroqResponse>(
                result,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (groqResponse?.Choices == null || groqResponse.Choices.Count == 0)
                return -1;

            var aiResponse = groqResponse.Choices[0].Message.Content.Trim();

            Console.WriteLine("Groq Response: " + aiResponse);

            var match = Regex.Match(aiResponse, @"^[0-8]$");

            if (!match.Success)
                return -1;

            return int.Parse(match.Value);
        }

        #region Helper
        private static string GetSystemPrompt()
        {
            return """
            You are an unbeatable Tic Tac Toe engine.

            You always play as O.
            The human player always plays as X.

            Your objective is to maximize your chance of winning while preventing the opponent from winning.

            Before selecting a move:

            - Evaluate every legal move.
            - Ignore occupied positions.
            - Compare every legal move.
            - Choose the strongest move.

            Decision priority:

            1. Win immediately.
            2. Block the opponent's immediate win.
            3. Create a fork.
            4. Block an opponent fork.
            5. Take the center.
            6. Take the opposite corner.
            7. Take any available corner.
            8. Take any available edge.

            Rules:

            - Never choose an occupied position.
            - Never choose an invalid position.
            - Before responding, verify that the selected position is empty.
            - Return exactly one integer between 0 and 8.
            - Return only the number.
            - Do not explain.
            - Do not return markdown.
            - Do not return punctuation.
            - Do not return extra text.
""";
        }
        private static string Cell(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }
        private static string BuildUserPrompt(string[] board)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Current board:");
            sb.AppendLine();

            sb.AppendLine($" {Cell(board[0])} | {Cell(board[1])} | {Cell(board[2])}");
            sb.AppendLine("---+---+---");
            sb.AppendLine($" {Cell(board[3])} | {Cell(board[4])} | {Cell(board[5])}");
            sb.AppendLine("---+---+---");
            sb.AppendLine($" {Cell(board[6])} | {Cell(board[7])} | {Cell(board[8])}");

            sb.AppendLine();
            sb.AppendLine("Board positions:");
            sb.AppendLine();

            sb.AppendLine(" 0 | 1 | 2");
            sb.AppendLine("---+---+---");
            sb.AppendLine(" 3 | 4 | 5");
            sb.AppendLine("---+---+---");
            sb.AppendLine(" 6 | 7 | 8");

            sb.AppendLine();
            sb.AppendLine("Choose the optimal move.");

            return sb.ToString();
        }
        #endregion

    }
}

