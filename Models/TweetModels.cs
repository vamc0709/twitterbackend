

using System.Text.Json.Serialization;

namespace Twitter.Models;

public record TweetItem
{
    [JsonPropertyName("tweet_id")]
    public long TweetId { get; set; }

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }= DateTimeOffset.UtcNow;

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }= DateTimeOffset.UtcNow;
}