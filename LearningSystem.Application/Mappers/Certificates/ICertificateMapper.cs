using LearningSystem.Application.Results.Certificates;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Certificates;

public interface ICertificateMapper
{
    CertificateResult Map(Certificate certificate);
    IEnumerable<CertificateResult> Map(IEnumerable<Certificate> certificates);
}
