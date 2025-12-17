using LearningSystem.Api.Authentication;
using LearningSystem.Api.Mappers.Authentication;
using LearningSystem.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAuthMapper _authMapper;

    public AuthenticationController(IAuthService authService, IAuthMapper authMapper)
    {
        _authService = authService;
        _authMapper = authMapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request)
    {
        var command = _authMapper.Map(request);
        var authResult = await _authService.RegisterAsync(command);

        var result = _authMapper.Map(authResult);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request)
    {
        var query = _authMapper.Map(request);
        var authResult = await _authService.LoginAsync(query);

        var result = _authMapper.Map(authResult);
        return Ok(result);
    }
}