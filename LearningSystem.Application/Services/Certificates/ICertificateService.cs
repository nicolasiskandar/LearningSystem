using LearningSystem.Application.Results.Certificates;

namespace LearningSystem.Application.Services.Certificates;

public interface ICertificateService
{
    Task<CertificateResult> GenerateCertificateAsync(int userId, int courseId);
    Task<CertificateResult?> GetCertificateAsync(int userId, int courseId);
    Task<IEnumerable<CertificateResult>> GetUserCertificatesAsync(int userId);
}
