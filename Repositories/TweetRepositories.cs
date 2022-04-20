using Dapper;
using Twitter.Models;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface ITweetRepository
{
    Task<TweetItem> Create(TweetItem Item);
    Task<bool> Update(TweetItem Item);
    Task<bool> Delete(long TweetId);
    Task<List<TweetItem>> GetAllTweets();
    Task<TweetItem> GetById(long TweetId);
    Task<List<TweetItem>> GetTweetsByUserId(long UserId);


}
public class TweetRepository : BaseRepository, ITweetRepository
{
    public TweetRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<TweetItem> Create(TweetItem Item)
    {
       var query = $@"INSERT INTO ""{TableNames.tweet}"" (user_id, title) 
        VALUES (@UserId, @Title)
        RETURNING *";

        using (var con = NewConnection)
            return await con.QueryFirstOrDefaultAsync<TweetItem>(query, Item);
    }

    public async Task<bool> Delete(long TweetId)
    {
        var query = $@"DELETE FROM ""{TableNames.tweet}"" WHERE tweet_id = @TweetId";

        using (var con = NewConnection)
        
            return await con.ExecuteAsync(query, new { TweetId }) > 0;
    }

    public async Task<List<TweetItem>> GetAllTweets()
    {
       var query = $@"SELECT * FROM ""{TableNames.tweet}""";
            using (var con = NewConnection)
                return (await con.QueryAsync<TweetItem>(query)).AsList();
    }

    
    public async Task<TweetItem> GetById(long TweetId)
    {
        var query = $@"SELECT * FROM ""{TableNames.tweet}"" WHERE tweet_id = @TweetId";
         
         using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<TweetItem>(query, new { TweetId });
    }

    public async Task<List<TweetItem>> GetTweetsByUserId(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.tweet}"" WHERE user_id = @UserId";
        using (var con = NewConnection)
            return (await con.QueryAsync<TweetItem>(query, new { UserId })).AsList();
    }

    public async Task<bool> Update(TweetItem Item)
    {
        var query = $@"UPDATE ""{TableNames.tweet}"" SET title = @Title WHERE tweet_id = TweetId";

        using (var con = NewConnection)
            return await con.ExecuteAsync(query, Item) > 0;
    }
}
