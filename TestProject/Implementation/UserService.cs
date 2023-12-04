using System.Data;
using LanguageExt.Common;
using Work.ApiModels;
using Work.Database;
using Work.Exceptions;
using Work.Extensions;
using Work.Interfaces;

namespace Work.Implementation
{

    public class UserService : IService<UserVm, Guid>
    {
        private readonly IRepository<UserDto, Guid> _userRepository_;
        private readonly IMapperWithValidation<UserDto, UserVm> _mapper_;
        private readonly ILogger _logger_;
        public UserService(
            IRepository<UserDto, Guid> userRepository,
            IMapperWithValidation<UserDto, UserVm> mapper,
            ILogger<UserService> logger)
        {
            _userRepository_ = userRepository;
            _mapper_ = mapper;
            _logger_ = logger;
        }
        public async Task<Result<UserVm>> CreateAsync(UserVm obj, CancellationToken cancellationToken)
        {
            try
            {
                var userGuid = Guid.NewGuid();
                obj.Id = userGuid;
                _logger_.LogInformation($"Creating user {obj.Name} with guid {userGuid}.");

                var dbModelMappingResult = _mapper_.MapVmToDto(obj);

                var userCreationResult = await dbModelMappingResult.BindAsync(async dto => await _userRepository_.CreateAsync(dto, cancellationToken));

                var vmMappingResult = userCreationResult.Bind(userDto =>
                {
                    var notNullResult = userCreationResult.Bind(dto => dto.IsNotNull());
                    return notNullResult.Match(_ => _mapper_.MapDtoToVm(userDto),
                        f => new Result<UserVm>(new UserNotFoundException($"User with id {obj.Id} not found.", f)));
                });

                return vmMappingResult;
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when creating user {obj.Name}", e);
                return new Result<UserVm>(e);
            }
        }
        public async Task<Result<UserVm>> ReadByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger_.LogInformation($"Retrieving user by guid {id}.");

                var userReadResult = await _userRepository_.ReadByIdAsync(id, cancellationToken);

                var vmMappingResult = userReadResult.Bind(userDto =>
                {
                    var notNullResult = userReadResult.Bind(dto => dto.IsNotNull());
                    return notNullResult.Match(_ => _mapper_.MapDtoToVm(userDto),
                        f => new Result<UserVm>(new UserNotFoundException($"User with id {id} not found.", f)));
                });

                return vmMappingResult;
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when retrieving user with guid {id}.", e);
                return new Result<UserVm>(e);
            }
        }

        public async Task<Result<List<UserVm>>> ReadAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger_.LogInformation($"Fetching all users.");

                var userReadResult = await _userRepository_.ReadAllAsync(cancellationToken);

                var vmMappingResult = userReadResult.Match(userDtos =>
                    {
                        var userVms = new List<UserVm>();

                        foreach (var uDto in userDtos)
                        {
                            var uVmMappingResult = _mapper_.MapDtoToVm(uDto);
                            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                            uVmMappingResult.Match(succ =>
                                {
                                    userVms.Add(succ);
                                    return succ;
                                },
                                fail =>
                                {
                                    _logger_.LogError($"Exception mapping {uDto.Id}. User won't be returned.", fail);
                                    return new Result<UserVm>(fail);
                                });
                        }
                        return userVms;
                    }, fail => new Result<List<UserVm>>(fail)
                );

                return vmMappingResult;
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when retrieving users.", e);
                return new Result<List<UserVm>>(e);
            }
        }

        public async Task<Result<Boolean>> UpdateAsync(UserVm obj, CancellationToken cancellationToken)
        {
            try
            {
                _logger_.LogInformation($"Updating user {obj.Id}.");

                var dbModelMappingResult = _mapper_.MapVmToDto(obj);

                var userCreationResult = await dbModelMappingResult.BindAsync(async dto => await _userRepository_.UpdateAsync(dto, cancellationToken));

                return userCreationResult;
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when updating user {obj.Id}", e);
                return new Result<Boolean>(e);
            }
        }

        public async Task<Result<Boolean>> RemoveByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger_.LogInformation($"Deleting user {id}.");

                var userReadResult = await _userRepository_.ReadByIdAsync(id, cancellationToken);


                var vmMappingResult = await userReadResult.BindAsync(async userDto =>
                {
                    var notNullResult = userReadResult.Bind(dto => dto.IsNotNull());
                    return await notNullResult.MatchAsync(async _ => await _userRepository_.RemoveByIdAsync(id, cancellationToken),
                        async f => await Task.FromResult(new Result<Boolean>(new UserNotFoundException($"User with id {id} not found.", f))));
                });

                return vmMappingResult;
            }
            catch (Exception e)
            {
                _logger_.LogError($"Exception when deleting user {id}", e);
                return new Result<Boolean>(e);
            }
        }
    }
}