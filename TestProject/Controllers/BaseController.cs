using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Work.Exceptions;

namespace Work.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        var resultHandled = result.Match<IActionResult>(
            succ => Ok(succ),
            exception =>
            {
                if (exception.GetType() == typeof(UserNotFoundException))
                    return NotFound(new ExceptionDetails(exception));
                return BadRequest(new ExceptionDetails(exception));
            });
        return resultHandled;
    }

    private class ExceptionDetails
    {
        public String Message { get; set; }
        public String? StackTrace { get; set; }
        public ExceptionDetails(Exception e)
        {
            Message = e.Message;
            StackTrace = e.StackTrace;
        }
    }
}