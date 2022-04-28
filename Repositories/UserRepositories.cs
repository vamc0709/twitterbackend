using Twitter.Models;
using Dapper;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmail(string Username);
    Task<User> Create(User Item);
    Task<bool> Update(User Item);

    Task<User> GetById(long UserId);
    // Task Getall();
    Task<List<User>> GetAll();
}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<User> GetByEmail(string Email)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" 
        WHERE Email = @Email";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Email });
    }


    public async Task<User> Create(User Item)
    {
        var query = $@"INSERT INTO ""{TableNames.users}"" (username, password, email) 
       VALUES (@Username, @Password, @Email)
       RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, Item);
            return res;
        }
    }

   public async Task<bool> Update(User Item)
    {
        var query = $@"UPDATE ""{TableNames.users}"" SET username = @Username
        WHERE user_id = @UserId";


        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);

            return rowCount == 1;
        }
    }

    public async Task<User> GetById(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""
        WHERE user_id = @UserId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query,
            new
            {
                UserId = UserId
            });
    }

    public async Task<List<User>> GetAll()
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""";
        List<User> res;
        using (var con = NewConnection)
        {
            res = (await con.QueryAsync<User>(query)).AsList();
        }
         return res;

    }
}