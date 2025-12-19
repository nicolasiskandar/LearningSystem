using LearningSystem.Api.Dtos.Certificates;
using LearningSystem.Application.Results.Certificates;

namespace LearningSystem.Api.Mappers.Certificates;

public interface ICertificateMapper
{
    CertificateDto Map(CertificateResult certificate);
    IEnumerable<CertificateDto> Map(IEnumerable<CertificateResult> certificates);
}