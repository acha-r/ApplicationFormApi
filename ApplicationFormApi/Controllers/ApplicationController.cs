using ApplicationFormApi.Data;
using ApplicationFormApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationFormApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ControllerBase
{
    public IApplicationRepository _applicationRepo;
    public ApplicationController(IApplicationRepository applicationRepo)
    {
        _applicationRepo = applicationRepo;
    }

    [HttpGet("application-questions")]
    public async Task<IActionResult> GetAllQuestions(string applicationId)
    {
        var applicationForm = await _applicationRepo.LoadApplication(applicationId);

        return Ok(applicationForm);
    }

    [HttpPost("add-question")]
    public async Task<IActionResult> AddQuestionToApplication(string applicationId, List<AddQuestionDto> request)
    {
        return Ok(await _applicationRepo.AddQuestionToApplication(applicationId, request));

    }

    [HttpPost("create-aplication")]
    public async Task<IActionResult> CreateApplication(CreateApplicationDto request)
    {
        var result = await _applicationRepo.CreateApplication(request);
        return Ok(result);
    }

    [HttpPut("update-question")]
    public async Task<IActionResult> UpdateQuestion(string applicationId, UpdateQuestionDto request)
    {
        return Ok(await _applicationRepo.UpdateQuestion(applicationId, request));
    }

    [HttpDelete("delete-question")]
    public async Task<IActionResult> DeleteQuestion(string applicationId, string questionId)
    {
        return Ok(await _applicationRepo.DeleteQuestionFromApplication(applicationId, questionId));
    }
}
