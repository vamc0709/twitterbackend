using Dapper;
using Twitter.Models;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface ICommentRepository
{
    Task<CommentItem> Create(CommentItem Item);
    Task<bool> Update(CommentItem Item);
    Task Delete(long CommentId);
    Task<List<CommentItem>> GetAllComments();
    Task<CommentItem> GetById(long CommentId);
    Task<List<CommentItem>> GetByTweetId(long tweet_id);
}
public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<CommentItem> Create(CommentItem Item)
    {
         var query = $@"INSERT INTO {TableNames.comment} (tweet_id, user_id, text, created_at, updated_at) 
        VALUES (@TweetId, @UserId, @Text, @CreatedAt, @UpdatedAt) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<CommentItem>(query, Item);
    }

    public async Task Delete(long CommentId)
    {
        var query = $@"DELETE FROM {TableNames.comment} WHERE comment_id = @CommentId";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { CommentId });
    }

    public async Task<List<CommentItem>> GetAllComments()
    {
        var query = $@"SELECT * FROM {TableNames.comment}";

        using (var con = NewConnection)
            return (await con.QueryAsync<CommentItem>(query)).AsList();
    }

    public async Task<CommentItem> GetById(long CommentId)
    {
        var query = $@"SELECT * FROM {TableNames.comment} WHERE comment_id = @CommentId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<CommentItem>(query, new { CommentId });
    }

    public async Task<List<CommentItem>> GetByTweetId(long tweet_id)
    {
        var query = $@"SELECT * FROM {TableNames.comment} WHERE tweet_id = @tweet_id";

        using (var con = NewConnection)
            return (await con.QueryAsync<CommentItem>(query, new{ tweet_id })).AsList();
    }

    public async Task<bool> Update(CommentItem Item)
    {
        var query = $@"UPDATE {TableNames.comment} SET text = @Text, updated_at = @UpdatedAt WHERE comment_id = @CommentId";
        using (var con = NewConnection)
            return await con.ExecuteAsync(query, Item) > 0;
    }
}
