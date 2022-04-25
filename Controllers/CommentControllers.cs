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
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentRepository _comment;

    public CommentController(ILogger<CommentController> logger,
    ICommentRepository comment)
    {
        _logger = logger;
        _comment = comment;
    }

     private long GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentItem>>> GetAllComments([FromQuery] CommentParameters commentParameters)
    {
        var allComments = await _comment.GetAllComments(commentParameters);
        return Ok(allComments);
    }

    [HttpGet("{tweet_id}")]
    public async Task<ActionResult<CommentItem>> GetByTweetId(long tweet_id)
    {
        var comment = await _comment.GetByTweetId(tweet_id);
        return Ok(comment);
    }

    [HttpPost]

    public async Task<ActionResult<CommentItem>> Create([FromBody] CreateCommentDto Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);
        var toCreateItem = new CommentItem
        {
            TweetId = Data.TweetId,
            UserId = userId,
            Text = Data.Text,
            // CreatedAt = DateTime.Now,
            // UpdatedAt = DateTime.Now
        };
        var createdItem = await _comment.Create(toCreateItem);
        return StatusCode(201, createdItem);
    }

    [HttpPut("{CommentId}")]
    public async Task<ActionResult<CommentItem>> Update(long CommentId, [FromBody] UpdateCommentDto Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);
        var toUpdateItem = new CommentItem
        {
            Text = Data.Text,
            // CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var updatedItem = await _comment.Update(toUpdateItem);
        return StatusCode(201, updatedItem);
    }
    [HttpDelete("{CommentId}")]
    public async Task<ActionResult<CommentItem>> Delete([FromRoute] long CommentId)
    {
        var userId = GetUserIdFromClaims(User.Claims);
        var existingItem = await _comment.GetById(CommentId);
        if (existingItem is null)
        
        return NotFound();
        if (existingItem.UserId != userId)
            return StatusCode(403,"Cannot delete other's comment");
        await _comment.Delete(CommentId);
        return Ok();
        
       
    }






}