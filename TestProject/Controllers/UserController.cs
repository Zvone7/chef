using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;

namespace Work.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User, Guid> _userRepository_;
        private readonly IMapper _mapper_;
        private readonly ILogger<UserController> _logger_;
        public UserController(
            IRepository<User, Guid> userRepository,
            IMapper mapper,
            ILogger<UserController> logger)
        {
            _userRepository_ = userRepository;
            _mapper_ = mapper;
            _logger_ = logger;
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(Guid id)
        {
            try
            {
                return Ok(_userRepository_.Read(id));
            }
            catch (DataException e)
            {
                var message = $"User with guid {id} doesn't exist in database.";
                _logger_.LogError(message, e);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                var message = $"Unhandled error: {ex.Message}.";
                _logger_.LogError(message, ex);
                return BadRequest(message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            try
            {
                return Ok(_userRepository_.ReadAll());
            }
            catch (Exception ex)
            {
                var message = $"Unhandled error: {ex.Message}.";
                _logger_.LogError(message, ex);
                return BadRequest(message);
            }
        }

        [HttpPost]
        public IActionResult Post(UserModelDto user)
        {
            try
            {
                _userRepository_.Create(_mapper_.Map<User>(user));
                return Ok("User created.");
            }
            catch (Exception ex)
            {
                var message = $"Unhandled error: {ex.Message}.";
                _logger_.LogError(message, ex);
                return BadRequest(message);
            }
        }

        [HttpPut]
        public IActionResult Put(UserModelDto user)
        {
            try
            {
                _userRepository_.Update(_mapper_.Map<User>(user));
                return Ok("User updated.");
            }
            catch (DataException e)
            {
                var message = $"User with guid {user.Id} doesn't exist in database.";
                _logger_.LogError(message, e);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                var message = $"Unhandled error: {ex.Message}.";
                _logger_.LogError(message, ex);
                return BadRequest(message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var user = _userRepository_.Read(id);
                _userRepository_.Remove(user);
                return Ok("User deleted.");
            }
            catch (DataException e)
            {
                var message = $"User with guid {id} doesn't exist in database.";
                _logger_.LogError(message, e);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                var message = $"Unhandled error: {ex.Message}.";
                _logger_.LogError(message, ex);
                return BadRequest(message);
            }
        }

    }
}