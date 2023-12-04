using System.Data;
using FluentAssertions;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Work.ApiModels;
using Work.Database;
using Work.Implementation;
using Work.Interfaces;

namespace Work.Tests;

public class UserServiceTest
{
    private static readonly Guid TestGuid = Guid.NewGuid();
    private static readonly DateTime TestBirthday = DateTime.UtcNow.AddDays(-14);
    private static readonly String TestUserName = "Simon";
    private static readonly UserDto UserDto = new UserDto()
    {
        Birthday = TestBirthday,
        UserName = TestUserName
    };
    readonly UserVm _userVm_ = new UserVm()
    {
        Birthdate = TestBirthday,
        Name = TestUserName
    };

    [Fact]
    public async Task UserService_CreateAsync_CreatesCorrectly()
    {
        var mockedLogger = new Mock<ILogger<UserService>>();
        var mockedDatabase = new Mock<IRepository<UserDto, Guid>>();
        var mockedMapper = new Mock<IMapperWithValidation<UserDto, UserVm>>();

        mockedDatabase.Setup(x => x.CreateAsync(It.IsAny<UserDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<UserDto>(UserDto));

        mockedMapper.Setup(x => x.MapDtoToVm(It.IsAny<UserDto>()))
            .Returns(new Result<UserVm>(_userVm_));
        mockedMapper.Setup(x => x.MapVmToDto(It.IsAny<UserVm>()))
            .Returns(new Result<UserDto>(UserDto));

        var userRepository = new UserService(mockedDatabase.Object, mockedMapper.Object, mockedLogger.Object);

        var res = await userRepository.CreateAsync(_userVm_, It.IsAny<CancellationToken>());

        var userResult = res.GetValue();
        userResult.Name.Should().Be(TestUserName);
        userResult.Birthdate.Should().Be(TestBirthday);
    }

    [Fact]
    public async Task UserService_UpdateAsync_UpdatesCorrectly()
    {
        var mockedLogger = new Mock<ILogger<UserService>>();
        var mockedDatabase = new Mock<IRepository<UserDto, Guid>>();
        var mockedMapper = new Mock<IMapperWithValidation<UserDto, UserVm>>();

        mockedDatabase.Setup(x => x.UpdateAsync(It.IsAny<UserDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Boolean>(true));

        mockedMapper.Setup(x => x.MapVmToDto(It.IsAny<UserVm>()))
            .Returns(new Result<UserDto>(UserDto));

        var userRepository = new UserService(mockedDatabase.Object, mockedMapper.Object, mockedLogger.Object);

        var res = await userRepository.UpdateAsync(_userVm_, It.IsAny<CancellationToken>());

        var userResult = res.GetValue();
        userResult.Should().Be(true);
    }

    [Fact]
    public async Task UserService_ReadByIdAsync_ReadsCorrectly()
    {
        var mockedLogger = new Mock<ILogger<UserService>>();
        var mockedDatabase = new Mock<IRepository<UserDto, Guid>>();
        var mockedMapper = new Mock<IMapperWithValidation<UserDto, UserVm>>();

        mockedDatabase.Setup(x => x.ReadByIdAsync(TestGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<UserDto>(UserDto));

        mockedMapper.Setup(x => x.MapDtoToVm(It.IsAny<UserDto>()))
            .Returns(new Result<UserVm>(_userVm_));
        mockedMapper.Setup(x => x.MapVmToDto(It.IsAny<UserVm>()))
            .Returns(new Result<UserDto>(UserDto));

        var userRepository = new UserService(mockedDatabase.Object, mockedMapper.Object, mockedLogger.Object);

        var res = await userRepository.ReadByIdAsync(TestGuid, It.IsAny<CancellationToken>());

        var userResult = res.GetValue();
        userResult.Name.Should().Be(TestUserName);
        userResult.Birthdate.Should().Be(TestBirthday);
    }

    [Fact]
    public async Task UserService_ReadAllAsync_ReadsCorrectly()
    {
        var mockedLogger = new Mock<ILogger<UserService>>();
        var mockedDatabase = new Mock<IRepository<UserDto, Guid>>();
        var mockedMapper = new Mock<IMapperWithValidation<UserDto, UserVm>>();

        mockedDatabase.Setup(x => x.ReadAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<IEnumerable<UserDto>>(new List<UserDto>() { UserDto }));

        mockedMapper.Setup(x => x.MapDtoToVm(It.IsAny<UserDto>()))
            .Returns(new Result<UserVm>(_userVm_));
        mockedMapper.Setup(x => x.MapVmToDto(It.IsAny<UserVm>()))
            .Returns(new Result<UserDto>(UserDto));

        var userRepository = new UserService(mockedDatabase.Object, mockedMapper.Object, mockedLogger.Object);

        var res = await userRepository.ReadAllAsync(It.IsAny<CancellationToken>());

        var userResult = res.GetValue();
        userResult.Count().Should().Be(1);
        userResult.First().Name.Should().Be(TestUserName);
    }


    [Fact]
    public async Task UserService_RemoveByIdAsync_DeletesCorrectly()
    {
        var mockedLogger = new Mock<ILogger<UserService>>();
        var mockedDatabase = new Mock<IRepository<UserDto, Guid>>();
        var mockedMapper = new Mock<IMapperWithValidation<UserDto, UserVm>>();

        mockedDatabase.Setup(x => x.ReadByIdAsync(TestGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<UserDto>(UserDto));
        
        mockedDatabase.Setup(x => x.RemoveByIdAsync(TestGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Boolean>(true));

        var userRepository = new UserService(mockedDatabase.Object, mockedMapper.Object, mockedLogger.Object);

        var res = await userRepository.RemoveByIdAsync(TestGuid, It.IsAny<CancellationToken>());

        var userResult = res.GetValue();
        userResult.Should().Be(true);
    }
}