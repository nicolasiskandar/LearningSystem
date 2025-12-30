using LearningSystem.Application.Common.Exceptions.Certificates;
using LearningSystem.Application.Common.Exceptions.Courses;
using LearningSystem.Application.Common.Exceptions.Users;
using LearningSystem.Application.Mappers.Certificates;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.Certificates;
using LearningSystem.Domain.Entities;

using LearningSystem.Application.Common.Caching;

namespace LearningSystem.Application.Services.Certificates;

public class CertificateService : ICertificateService
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly ICertificateMapper _certificateMapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILessonCompletedRepository _lessonCompletedRepository;
    private readonly ICacheService _cacheService;

    public CertificateService(
        ICertificateRepository certificateRepository, 
        ICertificateMapper certificateMapper,
        ICourseRepository courseRepository, 
        IUserRepository userRepository,
        ILessonCompletedRepository lessonCompletedRepository,
        ICacheService cacheService)
    {
        _certificateRepository = certificateRepository;
        _certificateMapper = certificateMapper;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _lessonCompletedRepository = lessonCompletedRepository;
        _cacheService = cacheService;
    }

    public async Task<CertificateResult> GenerateCertificateAsync(int userId, int courseId)
    {
        var existingCertificate = await _certificateRepository.GetByUserAndCourse(userId, courseId);
        if (existingCertificate != null)
            throw new CertificateAlreadyExistsException(userId, courseId);

        
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new UserNotFoundException(userId);

        var course = await _courseRepository.GetCourseByIdAsync(courseId);
        if (course == null)
            throw new CourseNotFoundException(courseId);

        var allLessonsCompleted = await CheckIfAllLessonsCompletedAsync(userId, courseId);
        if (!allLessonsCompleted)
            throw new NotAllLessonsCompletedException(userId, courseId);

        var downloadUrl = $"/certificates/{userId}/{courseId}";

        var certificate = new Certificate
        {
            UserId = userId,
            CourseId = courseId,
            DownloadUrl = downloadUrl,
            User = user,
            Course = course
        };

        await _certificateRepository.AddAsync(certificate);

        await _cacheService.RemoveAsync($"user-certificates-{userId}");
        await _cacheService.RemoveAsync($"certificate-{userId}-{courseId}");

        return _certificateMapper.Map(certificate);
    }

    public async Task<CertificateResult?> GetCertificateAsync(int userId, int courseId)
    {
        var cachedCertificate = await _cacheService.GetAsync<CertificateResult>($"certificate-{userId}-{courseId}");
        if (cachedCertificate != null)
            return cachedCertificate;

        var certificate = await _certificateRepository.GetByUserAndCourse(userId, courseId);
        if (certificate == null)
            throw new CertificateNotFoundException(userId, courseId);

        var result = _certificateMapper.Map(certificate);
        await _cacheService.SetAsync($"certificate-{userId}-{courseId}", result);

        return result;
    }

    public async Task<IEnumerable<CertificateResult>> GetUserCertificatesAsync(int userId)
    {
        var cachedCertificates = await _cacheService.GetAsync<IEnumerable<CertificateResult>>($"user-certificates-{userId}");
        if (cachedCertificates != null)
            return cachedCertificates;

        var certificates = await _certificateRepository.GetByUserIdAsync(userId);
        var result = _certificateMapper.Map(certificates);
        await _cacheService.SetAsync($"user-certificates-{userId}", result);

        return result;
    }

    private async Task<bool> CheckIfAllLessonsCompletedAsync(int userId, int courseId)
    {
        var courseLessons = await _courseRepository.GetLessonsByCourseIdAsync(courseId);
        if (courseLessons == null || !courseLessons.Any())
            return false;

        var lessonsCompleted = await _lessonCompletedRepository.GetCompletedLessonsForCourseAsync(userId, courseId);

        return courseLessons.Count() == lessonsCompleted?.Count();
    }
}

