using LearningSystem.Api.Dtos.Courses;
using LearningSystem.Api.Dtos.Users;
using LearningSystem.Api.Mappers.Courses;
using LearningSystem.Api.Mappers.Users;
using LearningSystem.Application.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearningSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("fixed")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserMapper _userMapper;
    private readonly ICourseMapper _courseMapper;

    public UsersController(IUserService userService, IUserMapper userMapper, ICourseMapper courseMapper)
    {
        _userService = userService;
        _userMapper = userMapper;
        _courseMapper = courseMapper;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var user = await _userService.GetMeAsync();
        var dto = _userMapper.Map(user);
        return Ok(dto);
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ICollection<UserDto>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        var dtos = _userMapper.Map(users);
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        var dto = _userMapper.Map(user);
        return Ok(dto);
    }

    [HttpGet("{id:int}/courses/created")]
    [Authorize]
    public async Task<ActionResult<ICollection<CourseDto>>> GetCoursesCreatedByUser(int id)
    {
        var courses = await _userService.GetCoursesCreatedByUserAsync(id);
        var dtos = _courseMapper.Map(courses);
        return Ok(dtos);
    }

    [HttpGet("{id:int}/courses/enrolled")]
    [Authorize]
    public async Task<ActionResult<ICollection<CourseDto>>> GetCoursesEnrolledByUser(int id)
    {
        var courses = await _userService.GetCoursesEnrolledByUserAsync(id);
        var dtos = _courseMapper.Map(courses);
        return Ok(dtos);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto dto)
    {
        var command = _userMapper.Map(dto);
        var createdUser = await _userService.AddUserAsync(command);
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, _userMapper.Map(createdUser));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        var command = _userMapper.Map(dto, id);
        var updatedUser = await _userService.UpdateUserAsync(command);
        return Ok(_userMapper.Map(updatedUser));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}
