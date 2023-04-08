using System.Text.Json.Serialization;

namespace Codely.Core.Gateways.Contracts;

public sealed class TranslateCodeResponse
{
    [JsonPropertyName("language")]
    public required string Language { get; init; }
    
    [JsonPropertyName("version")]
    public required string Version { get; init; }
    
    [JsonPropertyName("run")]
    public required TranslatedCodeResult Result { get; set; }
}

public sealed class TranslatedCodeResult
{
    [JsonPropertyName("stdout")] 
    public string? Output { get; init; }

    [JsonPropertyName("stderr")]
    public string? Error { get; init; }
}