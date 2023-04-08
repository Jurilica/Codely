using System.Text.Json.Serialization;

namespace Codely.Core.Gateways.Contracts;

public sealed class TranslateCodeResponse
{
    [JsonPropertyName("language")]
    public required string Language { get; init; }
    
    [JsonPropertyName("version")]
    public required string Version { get; init; }
}

public sealed class TranslatedCodeResult
{
    [JsonPropertyName("stdout")] 
    public string? Output { get; init; }

    [JsonPropertyName("stderr")]
    public string? Error { get; init; }
}