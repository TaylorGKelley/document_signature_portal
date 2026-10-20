using DocSign.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DocSign.Api.Features.Test;

[ApiController, Route("[controller]")]
public class TestController : Controller {
    [HttpGet, Route("[action]")]
    public IActionResult Get()
    {
        throw new TestException("Failed");
    }
}
