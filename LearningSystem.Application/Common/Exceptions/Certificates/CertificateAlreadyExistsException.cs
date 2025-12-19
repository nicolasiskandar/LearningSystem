using LearningSystem.Application.Common.Exceptions;

namespace LearningSystem.Application.Common.Exceptions.Certificates;

public class CertificateAlreadyExistsException : AlreadyExistsException
{
    public CertificateAlreadyExistsException(int userId, int courseId) : base($"Certificate for user {userId} and course {courseId} already exists.")
    {
    }
}
