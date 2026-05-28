using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Debales.AI.Providers;

public sealed class ClaudeProvider : IAIProvider
{
    private readonly HttpClient _http;
    private readonly AISettings _settings;
    private static readonly JsonSerializerOptions _json = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

    public ClaudeProvider(HttpClient http, IOptions<AISettings> settings)
    {
        _http = http;
        _settings = settings.Value;
    }

    public string Name => "Claude";

    public async Task<string> ChatAsync(string systemPrompt, IReadOnlyList<(string Role, string Content)> messages, CancellationToken ct = default)
    {
        var request = new
        {
            model = _settings.Model,
            max_tokens = 2048,
            system = systemPrompt,
            messages = messages.Select(m => new { role = m.Role, content = m.Content }).ToArray()
        };

        using var response = await _http.PostAsJsonAsync("messages", request, _json, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ClaudeResponse>(_json, ct);
        return result?.Content?.FirstOrDefault()?.Text ?? string.Empty;
    }

    private sealed class ClaudeResponse
    {
        [JsonPropertyName("content")]
        public List<ContentBlock>? Content { get; set; }
    }

    private sealed class ContentBlock
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
