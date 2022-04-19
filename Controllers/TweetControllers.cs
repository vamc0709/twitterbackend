using Microsoft.AspNetCore.Mvc;
using Twitter.Repositories;

namespace Twitter.Controllers;

[ApiController]
[Route("api/tweet")]
public class TweetController : ControllerBase
{
    private readonly ILogger<TweetController> _logger;
    private readonly ITweetRepository _Tweet;
    private readonly IConfiguration _config;

    public TweetController(ILogger<TweetController> logger,
    ITweetRepository Tweet, IConfiguration config)
    {
        _logger = logger;
        _Tweet = Tweet;
        _config = config;
    }

    
}