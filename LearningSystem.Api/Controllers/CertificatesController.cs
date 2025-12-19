using LearningSystem.Api.Dtos.Certificates;
using LearningSystem.Api.Mappers.Certificates;
using LearningSystem.Application.Services.Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CertificatesController : ControllerBase
{
    private readonly ICertificateService _certificateService;
    private readonly ICertificateMapper _certificateMapper;

    public CertificatesController(ICertificateService certificateService, ICertificateMapper certificateMapper)
    {
        _certificateService = certificateService;
        _certificateMapper = certificateMapper;
    }

    [Authorize]
    [HttpPost("generate/{userId}/{courseId}")]
    public async Task<ActionResult<CertificateDto>> GenerateCertificate(int userId, int courseId)
    {
        var result = await _certificateService.GenerateCertificateAsync(userId, courseId);
        var dto = _certificateMapper.Map(result);
        return CreatedAtAction(nameof(GetCertificate), new { userId, courseId }, dto);
    }

    [HttpGet("{userId}/{courseId}")]
    public async Task<ActionResult<CertificateDto>> GetCertificate(int userId, int courseId)
    {
        var result = await _certificateService.GetCertificateAsync(userId, courseId);
        var dto = _certificateMapper.Map(result);
        return Ok(dto);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ICollection<CertificateDto>>> GetUserCertificates(int userId)
    {
        var results = await _certificateService.GetUserCertificatesAsync(userId);
        var dtos = _certificateMapper.Map(results);
        return Ok(dtos);
    }
}
