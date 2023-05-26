using System.Text.Json.Serialization;

namespace MiniETicaret.Application.DTOs.Facebook;

public class FacebookAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    
}