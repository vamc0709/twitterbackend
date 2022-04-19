

using System.Text.Json.Serialization;

namespace Twitter.Models;

public record CommentItem
{
    [JsonPropertyName("comment_id")]
    public long CommentId { get; set; }

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("tweet_id")]
    public long TweetId { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}