using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record UserLoginDTO
{
    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MaxLength(20)]
    public string Password { get; set; }
}

public record UserLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

}


// public record UserLoginDto
// {

//     [JsonPropertyName("email")]
//     [ Required]
//     [MinLength(3)]
//     [MaxLength(255)]
//     public string Email { get; set; }

//     [JsonPropertyName("password")]
//     [Required]
//     [MinLength(4)]
//     [MaxLength(20)]
//     public string Password { get; set; }

//     [JsonPropertyName("username")]
//     [Required]
//     [MaxLength(255)]
//     public string Username { get; set; }
// }

public record UserCreateDto
{


    [JsonPropertyName("username")]
    [Required]
    [MaxLength(255)]
    public string Username { get; set; }


    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }

    [JsonPropertyName("email")]
    [MaxLength(50)]
    public string Email { get; set; }

}

public record UserUpdateDto
{
    [JsonPropertyName("username")]
    [Required]
    [MaxLength(255)]
    public string Username { get; set; }
}

