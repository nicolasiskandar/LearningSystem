using LearningSystem.Application.Results.Certificates;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Certificates;

public class CertificateMapper : ICertificateMapper
{
    public CertificateResult Map(Certificate certificate)
    {
        return new CertificateResult
        {
            Id = certificate.Id,
            CourseId = certificate.CourseId,
            UserId = certificate.UserId,
            DownloadUrl = certificate.DownloadUrl
        };
    }

    public IEnumerable<CertificateResult> Map(IEnumerable<Certificate> certificates)
    {
        return certificates.Select(Map);
    }
}
