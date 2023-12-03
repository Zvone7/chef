using System.Data;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Work.Database;
using Work.Implementation;

namespace Work.Tests;

public class UserRepositoryTest
{
    private readonly Guid _testGuid_ = Guid.NewGuid();
    MockDatabase CreateDatabase()
    {
        var usersDict = new Dictionary<Guid, User>()
        {
            {
                _testGuid_,
                new User()
                {
                    Birthday = DateTime.Now.AddDays(-7),
                    UserId = _testGuid_,
                    UserName = "test"
                }
            }
        };
        return new MockDatabase(usersDict);
    }

    [Fact]
    public void UserRepository_Create_CreatesCorrectly()
    {
        var emulatedDatabase = CreateDatabase();
        var mockedLogger = new Mock<ILogger<UserRepository>>();

        var userRepository = new UserRepository(emulatedDatabase, mockedLogger.Object);
        var newGuid = Guid.NewGuid();
        var userToAdd = new User()
        {
            UserId = newGuid,
            Birthday = DateTime.UtcNow.AddDays(-14),
            UserName = "Simon"
        };

        userRepository.Create(userToAdd);

        emulatedDatabase.Users.Count.Should().Be(2);
        emulatedDatabase.Users.FirstOrDefault(x => x.Key.Equals(newGuid)).Should().NotBeNull();
    }

    [Fact]
    public void UserRepository_Update_UpdatesCorrectly()
    {
        var emulatedDatabase = CreateDatabase();
        var mockedLogger = new Mock<ILogger<UserRepository>>();

        var userRepository = new UserRepository(emulatedDatabase, mockedLogger.Object);
        var newGuid = Guid.NewGuid();
        var userToUpdate = new User()
        {
            UserId = _testGuid_,
            Birthday = DateTime.UtcNow.AddDays(-14),
            UserName = "Simon"
        };

        userRepository.Update(userToUpdate);

        emulatedDatabase.Users.Count.Should().Be(1);
        var userInEmulatedDatabaseKvp = emulatedDatabase.Users.FirstOrDefault(x => x.Key.Equals(_testGuid_));
        userInEmulatedDatabaseKvp.Should().NotBeNull();
        var userInDb = userInEmulatedDatabaseKvp.Value;
        userInDb.UserName.Should().Be(userToUpdate.UserName);
        userInDb.Birthday.Should().Be(userToUpdate.Birthday);
    }

    [Fact]
    public void UserRepository_Read_ReadsCorrectly()
    {
        var emulatedDatabase = CreateDatabase();
        var mockedLogger = new Mock<ILogger<UserRepository>>();

        var userRepository = new UserRepository(emulatedDatabase, mockedLogger.Object);

        var user = userRepository.Read(_testGuid_);
        user.UserName.Should().NotBeNull();
    }
    
    [Fact]
    public void UserRepository_ReadAll_ReadsCorrectly()
    {
        var emulatedDatabase = CreateDatabase();
        var mockedLogger = new Mock<ILogger<UserRepository>>();

        var userRepository = new UserRepository(emulatedDatabase, mockedLogger.Object);

        var users = userRepository.ReadAll();
        users.Count().Should().Be(1);
    }


    [Fact]
    public void UserRepository_Delete_DeletesCorrectly()
    {
        var emulatedDatabase = CreateDatabase();
        var mockedLogger = new Mock<ILogger<UserRepository>>();

        var userRepository = new UserRepository(emulatedDatabase, mockedLogger.Object);

        var user = userRepository.Read(_testGuid_);
        user.UserName.Should().NotBeNull();

        userRepository.Remove(user);

        var readUserAgainAction = () => userRepository.Read(_testGuid_);
        readUserAgainAction.Should().Throw<DataException>($"User with guid {_testGuid_} not found");
    }
}