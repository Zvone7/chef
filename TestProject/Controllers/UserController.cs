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
        public UserController(
            IRepository<User, Guid> userRepository,
            IMapper mapper)
        {
            _userRepository_ = userRepository;
            _mapper_ = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(Guid id)
        {
            try
            {
                return Ok(_userRepository_.Read(id));
            }
            catch (DataException)
            {
                // todo - logging
                return NotFound($"User with guid {id} doesn't exist in database.");
            }
            catch (Exception ex)
            {
                // todo - logging
                return BadRequest($"Unhandled error: {ex.Message}.");
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
                return BadRequest($"Unhandled error: {ex.Message}.");
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
                return BadRequest($"Unhandled error: {ex.Message}.");
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
            catch (DataException)
            {
                // todo - logging
                return NotFound($"User with guid {user.Id} doesn't exist in database.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Unhandled error: {ex.Message}.");
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
            catch (DataException)
            {
                // todo - logging
                return NotFound($"User with guid {id} doesn't exist in database.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Unhandled error: {ex.Message}.");
            }
        }

    }
}