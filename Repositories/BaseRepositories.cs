using Npgsql;
using Twitter.Settings;

namespace Twitter.Repositories;

public class BaseRepository
{

    private readonly IConfiguration _configuration;
    public BaseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public NpgsqlConnection NewConnection => new NpgsqlConnection(_configuration
    .GetSection(nameof(PostgresSettings)).Get<PostgresSettings>().ConnectionString);




}