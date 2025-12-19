using LearningSystem.Application.Common.Exceptions;

namespace LearningSystem.Application.Common.Exceptions.Certificates;

public class CertificateNotFoundException : NotFoundException
{
    public CertificateNotFoundException(int userId, int courseId) : base($"Certificate for user {userId} and course {courseId} not found.")
    {
    }
}
