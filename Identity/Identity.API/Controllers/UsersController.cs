using Identity.API.Infrastructure.Attributes;
using Identity.API.Infrastructure.Models;
using Identity.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("login", Name = "authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
    {
        var response = await _userService.Authenticate(model);
        return Ok(response);
    }

    [HttpPost("register", Name = "register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var response = await _userService.Register(model);
        return Ok(response);
    }

    [Authorize]
    [HttpGet(Name = "get all")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }
}