

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record CreateCommentDto
{
        
    [JsonPropertyName("user_id")]
    [Required]  
    public long UserId { get; set; }

    [JsonPropertyName("tweet_id")]
    [Required]
    public long TweetId { get; set; }

    [JsonPropertyName("text")]
    [Required]
    public string Text { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}


public record UpdateCommentDto
{
    [JsonPropertyName("text")]
    [Required]
    public string Text { get; set; }
}