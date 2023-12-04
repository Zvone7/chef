using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Interfaces;

namespace Work.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController
{
    private readonly IService<UserVm, Guid> _userService_;
    public UserController(
        IService<UserVm, Guid> userService)
    {
        _userService_ = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var res = await _userService_.ReadByIdAsync(id, cancellationToken);
        return HandleResult(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var res = await _userService_.ReadAllAsync(cancellationToken);
        return HandleResult(res);
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserVm user, CancellationToken cancellationToken)
    {
        var res = await _userService_.CreateAsync(user, cancellationToken);
        return HandleResult(res);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UserVm user, CancellationToken cancellationToken)
    {
        var res = await _userService_.UpdateAsync(user, cancellationToken);
        return HandleResult(res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var res = await _userService_.RemoveByIdAsync(id, cancellationToken);
        return HandleResult(res);
    }
}