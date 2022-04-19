using Dapper;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface ITweetRepository
{
    Task<Tweet> GetByTweet(string Tweetname);
    Task<Tweet> Create(Tweet Item);
    Task<bool> Update(Tweet Item);
    Task<
}
public class TweetRepository : BaseRepository, ITweetRepository
{
    public TweetRepository(IConfiguration config) : base(config)
    {

    }

    public Task<Tweet> GetByTweet(string Tweetname)
    {
        throw new NotImplementedException();
    }

}
