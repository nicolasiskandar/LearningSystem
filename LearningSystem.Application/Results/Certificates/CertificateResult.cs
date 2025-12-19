namespace LearningSystem.Application.Results.Certificates;

public class CertificateResult
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public string DownloadUrl { get; set; } = null!;
}
