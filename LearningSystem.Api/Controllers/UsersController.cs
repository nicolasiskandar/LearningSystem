using LearningSystem.Api.Dtos.Courses;
using LearningSystem.Api.Dtos.Users;
using LearningSystem.Api.Mappers.Users;
using LearningSystem.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserMapper _userMapper;

    public UsersController(IUserService userService, IUserMapper userMapper)
    {
        _userService = userService;
        _userMapper = userMapper;
    }

    [HttpGet]
    public ActionResult<ICollection<UserDto>> GetUsers()
    {
        var users = _userService.GetUsers();
        var dtos = _userMapper.Map(users);
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public ActionResult<UserDto> GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        var dto = _userMapper.Map(user);
        return Ok(dto);
    }

    [HttpGet("{id:int}/courses/created")]
    public ActionResult<ICollection<CourseDto>> GetCoursesCreatedByUser(int id)
    {
        var courses = _userService.GetCoursesCreatedByUser(id);
        return Ok(courses);
    }

    [HttpGet("{id:int}/courses/enrolled")]
    public ActionResult<ICollection<CourseDto>> GetCoursesEnrolledByUser(int id)
    {
        var courses = _userService.GetCoursesEnrolledByUser(id);
        return Ok(courses);
    }

    [HttpPost]
    public ActionResult<UserDto> AddUser([FromBody] CreateUserDto dto)
    {
        var command = _userMapper.Map(dto);

        var createdUser = _userService.AddUser(command);

        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:int}")]
    public ActionResult<UserDto> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        var command = _userMapper.Map(dto, id);

        var updatedUser = _userService.UpdateUser(command);
        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        _userService.DeleteUser(id);
        return NoContent();

    }
}
