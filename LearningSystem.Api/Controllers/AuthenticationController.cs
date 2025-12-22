using LearningSystem.Api.Authentication;
using LearningSystem.Api.Mappers.Authentication;
using LearningSystem.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearningSystem.Api.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("fixed")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAuthMapper _authMapper;
﻿
﻿    public AuthenticationController(IAuthService authService, IAuthMapper authMapper)
﻿    {
﻿        _authService = authService;
﻿        _authMapper = authMapper;
﻿    }
﻿
﻿    [HttpPost("register")]
﻿    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request)
﻿    {
﻿        var command = _authMapper.Map(request);
﻿        var authResult = await _authService.RegisterAsync(command);
﻿
﻿        var result = _authMapper.Map(authResult);
﻿        
﻿        var cookieOptions = new CookieOptions
﻿        {
﻿            HttpOnly = true,
﻿            Secure = true,
﻿            Expires = DateTime.UtcNow.AddDays(7)
﻿        };
﻿        
﻿        Response.Cookies.Append("refreshToken", authResult.RefreshToken, cookieOptions);
﻿        
﻿        return Ok(result);
﻿    }
﻿
﻿    [HttpPost("login")]
﻿    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request)
﻿    {
﻿        var query = _authMapper.Map(request);
﻿        var authResult = await _authService.LoginAsync(query);
﻿
﻿        var result = _authMapper.Map(authResult);
﻿        
﻿        var cookieOptions = new CookieOptions
﻿        {
﻿            HttpOnly = true,
﻿            Secure = true,
﻿            Expires = DateTime.UtcNow.AddDays(7)
﻿        };
﻿        
﻿        Response.Cookies.Append("refreshToken", authResult.RefreshToken, cookieOptions);
﻿        
﻿        return Ok(result);
﻿    }
﻿
﻿    [HttpPost("refresh-token")]
﻿    public async Task<ActionResult<AuthenticationResponse>> RefreshToken()
﻿    {
﻿        var refreshToken = Request.Cookies["refreshToken"];
﻿        if (string.IsNullOrEmpty(refreshToken))
            throw new UnauthorizedAccessException("Invalid refresh token");
﻿
﻿        var authResult = await _authService.RefreshTokenAsync(refreshToken);
﻿        var result = _authMapper.Map(authResult);
﻿
﻿        var cookieOptions = new CookieOptions
﻿        {
﻿            HttpOnly = true,
﻿            Secure = true,
﻿            Expires = DateTime.UtcNow.AddDays(7)
﻿        };
﻿
﻿        Response.Cookies.Append("refreshToken", authResult.RefreshToken, cookieOptions);
﻿
﻿        return Ok(result);
﻿    }
﻿}
﻿