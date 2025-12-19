namespace LearningSystem.Api.Dtos.Certificates;

public class CertificateDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public string DownloadUrl { get; set; } = null!;
}
