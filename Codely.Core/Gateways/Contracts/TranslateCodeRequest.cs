using System.Text.Json.Serialization;

namespace Codely.Core.Gateways.Contracts;

public sealed class TranslateCodeRequest
{
    [JsonPropertyName("language")]
    public required string Language { get; init; }

    [JsonPropertyName("version")]
    public required string Version { get; init; }

    [JsonPropertyName("files")]
    public required List<TranslateCodeFile> Files { get; init; } = new();
    
    [JsonPropertyName("stdin")]
    public string? StandardInput { get; init; }
    
    [JsonPropertyName("args")]
    public List<string>? Arguments { get; init; }
}

public sealed class TranslateCodeFile
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }
}
