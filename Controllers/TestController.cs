using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ApiVersioning.Controllers;

[ApiVersion(1)]
[ApiVersion(2)]
[ApiController]
[Route("api/v{v:apiVersion}/test")]
public class TestController : ControllerBase
{
    // [MapToApiVersion(1)]
    [HttpGet]
    public IActionResult GetV1() =>
        Ok("Ok controller v1");

    [MapToApiVersion(2)]
    [HttpGet]
    public IActionResult GetV2() =>
        Ok("Ok controller v2");
}