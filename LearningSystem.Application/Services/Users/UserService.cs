using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Common.Exceptions.Users;
using LearningSystem.Application.Mappers.Courses;
using LearningSystem.Application.Mappers.Users;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.Courses;
using LearningSystem.Application.Results.Users;
using LearningSystem.Domain.Entities;
using LearningSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using LearningSystem.Application.Mappers.Lessons;
using LearningSystem.Application.Common.Caching;

namespace LearningSystem.Application.Services.Users;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserMapper _userMapper;
    private readonly ICourseMapper _courseMapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICacheService _cacheService;

    public UserService(
        UserManager<User> userManager,
        IUserMapper userMapper,
        ICourseMapper courseMapper,
        ILessonMapper lessonMapper,
        ICourseRepository courseRepository,
        ILessonRepository lessonRepository,
        IHttpContextAccessor httpContextAccessor,
        ICacheService cacheService)
    {
        _userManager = userManager;
        _userMapper = userMapper;
        _courseMapper = courseMapper;
        _lessonMapper = lessonMapper;
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _httpContextAccessor = httpContextAccessor;
        _cacheService = cacheService;
    }

    public async Task<UserResult> AddUserAsync(CreateUserCommand command)
    {
        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if (existingUser != null)
            throw new UserAlreadyExistsException($"User with email {command.Email} already exists.");

        var user = _userMapper.Map(command);
        user.CreatedAt = DateTime.UtcNow;

        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
            throw new UserRegistrationFailedException();

        await _userManager.AddToRoleAsync(user, Roles.Student.ToString());

        await _cacheService.RemoveAsync("users-all");

        return await GetUserResult(user);
    }

    public async Task<UserResult> GetUserByIdAsync(int id)
    {
        var cachedUser = await _cacheService.GetAsync<UserResult>($"user-{id}");
        if (cachedUser != null)
            return cachedUser;

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new UserNotFoundException(id);

        var result = await GetUserResult(user);
        await _cacheService.SetAsync($"user-{id}", result);

        return result;
    }

    public async Task<IEnumerable<UserResult>> GetUsersAsync()
    {
        var cachedUsers = await _cacheService.GetAsync<IEnumerable<UserResult>>("users-all");
        if (cachedUsers != null)
            return cachedUsers;

        var users = _userManager.Users.ToList();
        var results = new List<UserResult>();

        foreach (var user in users)
        {
            var userResult = await GetUserResult(user);
            results.Add(userResult);
        }

        await _cacheService.SetAsync("users-all", results);

        return results;
    }

    public async Task<IEnumerable<CourseResult>> GetCoursesCreatedByUserAsync(int userId)
    {
        var user = await _userManager.Users
            .Include(u => u.Courses)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var courses = await _courseRepository.GetCoursesByUserIdAsync(userId);

        return courses.Select(c => _courseMapper.Map(c));
    }

    public async Task<IEnumerable<CourseResult>> GetCoursesEnrolledByUserAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new UserNotFoundException(userId);

        var courses = await _courseRepository.GetCoursesEnrolledByUserAsync(userId);
        return _courseMapper.Map(courses);
    }

    public async Task<UserResult> UpdateUserAsync(UpdateUserCommand command)
    {
        var user = await GetUserFromRepo(command.Id);

        if (UserWithEmailAlreadyExists(command))
            throw new UserAlreadyExistsException($"User with email {command.Email} already exists.");

        _userMapper.Map(command, user);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new UserUpdateFailedException();

        await _cacheService.RemoveAsync($"user-{command.Id}");
        await _cacheService.RemoveAsync("users-all");

        return await GetUserResult(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await GetUserFromRepo(id);

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new UserDeleteFailedException();

        await _cacheService.RemoveAsync($"user-{id}");
        await _cacheService.RemoveAsync("users-all");
    }

    public async Task<UserResult> GetMeAsync()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UserNotFoundException();

        if (!int.TryParse(userIdClaim.Value, out int userId))
            throw new UserNotFoundException();

        return await GetUserByIdAsync(userId);
    }

    private async Task<User?> GetUserFromRepo(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new UserNotFoundException(id);
        return user;
    }

    private async Task<UserResult> GetUserResult(User user)
    {
        var userResult = _userMapper.Map(user);
        var roles = await _userManager.GetRolesAsync(user);
        userResult.RoleName = roles.FirstOrDefault();

        return userResult;
    }

    private bool UserWithEmailAlreadyExists(UpdateUserCommand command)
    {
        var existingUser = _userManager.Users.FirstOrDefault(u => u.Email == command.Email);
        return existingUser != null && existingUser.Id != command.Id;
    }
}
