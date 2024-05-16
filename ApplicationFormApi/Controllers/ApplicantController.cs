using ApplicationFormApi.Data;
using ApplicationFormApi.DTO;
using ApplicationFormApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationFormApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicantController : ControllerBase
{
    private readonly IApplicantServices _applicantService;

    public ApplicantController(IApplicantServices applicantService)
    {
        _applicantService = applicantService;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitApplication(string applicationId, ApplicantFormDto request)
    {
        return Ok(await _applicantService.SubmitAplication(applicationId, request));
    }
}
