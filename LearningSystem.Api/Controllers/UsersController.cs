using LearningSystem.Application.Dtos.Users;
using LearningSystem.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<ICollection<UserDto>> GetUsers()
    {
        var users = _userService.GetUsers();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public ActionResult<UserDto> GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        return Ok(user);

    }

    [HttpPost]
    public ActionResult<UserDto> AddUser([FromBody] CreateUserDto dto)
    {
        var createdUser = _userService.AddUser(dto);
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:int}")]
    public ActionResult<UserDto> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {

        var updatedUser = _userService.UpdateUser(id, dto);
        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        _userService.DeleteUser(id);
        return NoContent();

    }
}
