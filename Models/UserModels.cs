namespace Twitter.Models;

public record User
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}