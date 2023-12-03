using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;

namespace Work.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            throw new NotImplementedException("TODO");
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            throw new NotImplementedException("TODO");
        }

        [HttpPost]
        public IActionResult Post(UserModelDto user)
        {
            throw new NotImplementedException("TODO");
        }
        
        [HttpPut]
        public IActionResult Put(UserModelDto user)
        {
            throw new NotImplementedException("TODO");
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            throw new NotImplementedException("TODO");
        }

    }
}