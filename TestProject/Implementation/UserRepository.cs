using System.Data;
using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly MockDatabase _mockDatabase_;
        public UserRepository(MockDatabase mockDatabase)
        {
            _mockDatabase_ = mockDatabase;
        }
        public void Create(User obj)
        {
            try
            {
                var userGuid = Guid.NewGuid();
                _mockDatabase_.Users.TryAdd(userGuid, obj);
            }
            catch (Exception e)
            {
                // todo - logging
                Console.WriteLine($"Exception {e.Message} when creating user {obj.UserName}");
            }
        }
        public User Read(Guid key)
        {
            try
            {
                var retrieve = _mockDatabase_.Users.TryGetValue(key, out var user);
                if (retrieve && user != null)
                    return user;
                Console.WriteLine($"Unable to find user with guid {key}.");
                throw new DataException($"User with guid {key} not found");
            }
            catch (Exception e)
            {
                // todo - better logging
                Console.WriteLine($"Exception {e.Message} when retrieving user with guid {key}");
                throw;
            }
        }
        public IEnumerable<User> ReadAll()
        {
            try
            {
                return _mockDatabase_.Users.Select(x => x.Value);
            }
            catch (Exception e)
            {
                // todo - logging
                Console.WriteLine($"Exception {e.Message} when fetching users.");
                throw;
            }
        }
        public void Update(User obj)
        {
            try
            {
                // check users exists
                var user = Read(obj.UserId);
                // this method would have thrown otherwise so proceed with update logic

                var userInDb = _mockDatabase_.Users.Where(x => x.Key.Equals(obj.UserId))
                    .Select(x => x.Value)
                    .First();

                userInDb.UserName = obj.UserName;
                userInDb.Birthday = obj.Birthday;
                // logging..
            }
            catch (Exception e)
            {
                // todo - logging
                Console.WriteLine($"Exception {e.Message} when updating user {obj.UserId}");
            }
        }
        public void Remove(User obj)
        {
            try
            {
                // check users exists
                var user = Read(obj.UserId);
                // this method would have thrown otherwise so proceed with delete logic
                _mockDatabase_.Users.Remove(obj.UserId);
            }
            catch (Exception e)
            {
                // todo - logging
                Console.WriteLine($"Exception {e.Message} when deleting user {obj.UserName}");
            }
        }
    }
}