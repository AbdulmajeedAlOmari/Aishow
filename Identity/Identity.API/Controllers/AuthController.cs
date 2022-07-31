using Common.API.Helpers;
using Common.API.Models.Entities;
using Identity.API.Infrastructure.Attributes;
using Identity.API.Infrastructure.Models;
using Identity.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : CommonControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login", Name = "Login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateRequest model)
    {
        var response = await _userService.Login(model);
        return Ok(response);
    }

    [HttpPost("register", Name = "Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var response = await _userService.Register(model);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("user", Name = "Get User Information")]
    public async Task<IActionResult> GetUserInformation()
    {
        // First, validate token in [Authorize] attribute

        // Then, we get user details from user service using their Token
        CommonUserDto user = await _userService.GetUserFromToken();

        // Return user details
        return Ok(user);
    }
}