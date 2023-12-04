using System.Data;
using System.Data.SqlClient;
using Dapper;
using LanguageExt.Common;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;

namespace Work.Implementation;

public class UserRepository : IRepository<UserDto, Guid>
{
    private readonly String _connectionString_;
    private readonly ILogger<UserRepository> _logger_;

    public UserRepository(
        DbConfig dbConfig,
        ILogger<UserRepository> logger
    )
    {
        _connectionString_ = dbConfig.ConnectionString;
        _logger_ = logger;
    }

    public async Task<Result<UserDto>> CreateAsync(UserDto obj, CancellationToken cancellationToken)
    {
        try
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString_);
            dbConnection.Open();
            var query = "INSERT INTO UserTable (Id, UserName, Birthday) " +
                        "OUTPUT INSERTED.Id " +
                        "VALUES (@Id, @UserName, @Birthday)";
            var id = await dbConnection.ExecuteScalarAsync<Guid>(query, obj);
            return new Result<UserDto>(obj);
        }
        catch (Exception e)
        {
            _logger_.LogError(e, $"Exception on {nameof(CreateAsync)}.");
            return new Result<UserDto>(e);
        }
    }
    public async Task<Result<UserDto>> ReadByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString_);
            dbConnection.Open();
            return new Result<UserDto>(
                await dbConnection.QueryFirstOrDefaultAsync<UserDto>(
                    "SELECT * FROM UserTable WHERE Id = @Id",
                    new { Id = id }));
        }
        catch (Exception e)
        {
            _logger_.LogError(e, $"Exception on {nameof(ReadByIdAsync)}.");
            return new Result<UserDto>(e);
        }
    }
    public async Task<Result<IEnumerable<UserDto>>> ReadAllAsync( CancellationToken cancellationToken)
    {
        try
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString_);
            dbConnection.Open();
            return new Result<IEnumerable<UserDto>>(
                await dbConnection.QueryAsync<UserDto>(
                    "SELECT * FROM UserTable"));
        }
        catch (Exception e)
        {
            _logger_.LogError(e, $"Exception on {nameof(ReadAllAsync)}.");
            return new Result<IEnumerable<UserDto>>(e);
        }
    }
    public async Task<Result<Boolean>> UpdateAsync(UserDto obj, CancellationToken cancellationToken)
    {
        try
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString_);
            dbConnection.Open();
            var query = "Update UserTable Set " +
                        "UserName = @UserName, " +
                        "Birthday = @Birthday " +
                        "WHERE Id = @Id";
            await dbConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = obj.Id,
                UserName = obj.UserName,
                Birthday = obj.Birthday

            });
            return new Result<Boolean>(true);
        }
        catch (Exception e)
        {
            _logger_.LogError(e, $"Exception on {nameof(UpdateAsync)}.");
            return new Result<Boolean>(e);
        }
    }
    public async Task<Result<Boolean>> RemoveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString_);
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                "DELETE FROM UserTable WHERE Id = @Id",
                new { Id = id });
            return new Result<Boolean>(true);
        }
        catch (Exception e)
        {
            _logger_.LogError(e, $"Exception on {nameof(RemoveByIdAsync)}.");
            return new Result<Boolean>(e);
        }
    }
}