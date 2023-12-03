using System.Data;
using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly MockDatabase _mockDatabase_;
        private readonly ILogger _logger_;
        public UserRepository(
            MockDatabase mockDatabase,
            ILogger<UserRepository> logger)
        {
            _mockDatabase_ = mockDatabase;
            _logger_ = logger;
        }
        public void Create(User obj)
        {
            try
            {
                var userGuid = Guid.NewGuid();
                _logger_.LogInformation($"Creating user {obj.UserName} with guid {userGuid}.");
                obj.UserId = userGuid;
                _mockDatabase_.Users.TryAdd(userGuid, obj);
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when creating user {obj.UserName}", e);
            }
        }
        public User Read(Guid key)
        {
            try
            {
                _logger_.LogInformation($"Retrieving user with guid {key}.");
                var retrieve = _mockDatabase_.Users.TryGetValue(key, out var user);
                if (retrieve && user != null)
                    return user;
                throw new DataException($"User with guid {key} not found");
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when retrieving user with guid {key}.", e);
                throw;
            }
        }
        public IEnumerable<User> ReadAll()
        {
            try
            {
                _logger_.LogInformation($"Retrieving all users.");
                return _mockDatabase_.Users.Select(x => x.Value);
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when fetching users.", e);
                throw;
            }
        }
        public void Update(User obj)
        {
            try
            {
                // check users exists
                _logger_.LogInformation($"Updating user {obj.UserId}.");
                var user = Read(obj.UserId);
                // this method would have thrown otherwise so proceed with update logic

                var userInDb = _mockDatabase_.Users.Where(x => x.Key.Equals(obj.UserId))
                    .Select(x => x.Value)
                    .First();

                userInDb.UserName = obj.UserName;
                userInDb.Birthday = obj.Birthday;
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when updating user with guid {obj.UserId}.", e);
            }
        }
        public void Remove(User obj)
        {
            try
            {
                _logger_.LogInformation($"Deleting user with guid {obj.UserId}.");
                // check users exists
                var user = Read(obj.UserId);
                // this method would have thrown otherwise so proceed with delete logic
                _mockDatabase_.Users.Remove(obj.UserId);
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when deleting user {obj.UserName}.", e);
            }
        }
    }
}