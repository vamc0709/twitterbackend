using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.DTOs;
using Twitter.Models;
using Twitter.Repositories;
using Twitter.Utilities;

namespace Twitter.Controllers;

[ApiController]
[Authorize]
[Route("api/tweet")]
public class TweetController : ControllerBase
{
    private readonly ILogger<TweetController> _logger;
    private readonly ITweetRepository _tweet;

    public TweetController(ILogger<TweetController> logger,
    ITweetRepository Tweet)
    {
        _logger = logger;
        _tweet = Tweet;
    }
    private long GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<TweetItem>>> GetAllTweets()
    {
        var allTweets = await _tweet.GetAllTweets();
        return Ok(allTweets);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TweetItem>> GetById([FromRoute]long id)
    {
        var tweet = await _tweet.GetById(id);
        return Ok(tweet);
    }
    [HttpPost]
    public async Task<ActionResult<TweetItem>> Create([FromBody] CreateTweetDto Data)

    {
        var userId = GetUserIdFromClaims(User.Claims);

        List<TweetItem> usertweets = await _tweet.GetTweetsByUserId(userId);
        if (usertweets != null && usertweets.Count >= 5)
        {
            return BadRequest("Limit exceeded");
        }

        var toCreateItem = new TweetItem
        {
            Title = Data.Title.Trim(),
            UserId = userId,
        };
        var createdItem = await _tweet.Create(toCreateItem);
        return StatusCode(201, createdItem);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TweetItem>> Update([FromRoute] long id,
    [FromBody] TweetUpdateDto Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);
        var existingItem = await _tweet.GetById(id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "Cannot update other's tweet");

        var toUpdateItem = existingItem with
        {
            TweetId =existingItem.TweetId,
            Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),

        };

        await _tweet.Update(toUpdateItem);

        return NoContent();
    }
     [HttpDelete("{TweetId}")]
    public async Task<ActionResult<TweetItem>> Delete([FromRoute] long TweetId)
    {
        var userId = GetUserIdFromClaims(User.Claims);
        var existingItem = await _tweet.GetById(TweetId);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "Cannot delete other's tweet");

        await _tweet.Delete(TweetId);

        return NoContent();
    }  
}