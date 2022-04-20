

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record CreateTweetDto
{

    [JsonPropertyName("user_id")]
    [Required]
    public long UserId { get; set; }

    [JsonPropertyName("title")]
    [MinLength(3)]
    [MaxLength(50)]
    public string Title { get; set; }
}
public record TweetUpdateDto
{
        
        [JsonPropertyName("title")]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }
}