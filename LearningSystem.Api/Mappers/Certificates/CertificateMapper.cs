using LearningSystem.Api.Dtos.Certificates;
using LearningSystem.Application.Results.Certificates;

namespace LearningSystem.Api.Mappers.Certificates;

public class CertificateMapper : ICertificateMapper
{
    public CertificateDto Map(CertificateResult certificate)
    {
        return new CertificateDto
        {
            Id = certificate.Id,
            UserId = certificate.UserId,
            CourseId = certificate.CourseId,
            DownloadUrl = certificate.DownloadUrl,
        };
    }

    public IEnumerable<CertificateDto> Map(IEnumerable<CertificateResult> certificates)
    {
        return certificates.Select(Map);
    }
}