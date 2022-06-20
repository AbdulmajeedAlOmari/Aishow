using Identity.API.Infrastructure.Models;
using Identity.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login", Name = "login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateRequest model)
    {
        var response = await _userService.Login(model);
        return Ok(response);
    }

    [HttpPost("register", Name = "register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var response = await _userService.Register(model);
        return Ok(response);
    }
}