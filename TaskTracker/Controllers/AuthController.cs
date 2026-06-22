using Microsoft.AspNetCore.Mvc;
using TaskTracker.DTOs.User;
using TaskTracker.Services.User;


namespace TaskTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;

    public AuthController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserReadDto>> Register(UserRegisterDto dto)
    {
        return await _service.RegisterAsync(dto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var token = await _service.LoginAsync(dto);

        if (token is null) return Unauthorized();

        return Ok(new { token });
    }
}