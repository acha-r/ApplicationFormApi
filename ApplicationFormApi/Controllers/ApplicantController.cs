using ApplicationFormApi.Data;
using ApplicationFormApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationFormApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicantController : ControllerBase
{
    private readonly IApplicantRepository _applicantRepo;

    public ApplicantController(IApplicantRepository applicantRepo)
    {
        _applicantRepo = applicantRepo;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitApplication(string applicationId, ApplicantFormDto request)
    {
        return Ok(await _applicantRepo.SubmitAplication(applicationId, request));
    }
}
