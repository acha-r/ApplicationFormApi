using ApplicationFormApi.Data;
using ApplicationFormApi.DTO;
using ApplicationFormApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationFormApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ControllerBase
{
    public IApplicationServices _applicationService;
    public ApplicationController(IApplicationServices applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("application-questions")]
    public async Task<IActionResult> GetAllQuestions(string applicationId)
    {
        var applicationForm = await _applicationService.LoadApplication(applicationId);

        return Ok(applicationForm);
    }

    [HttpPost("add-question")]
    public async Task<IActionResult> AddQuestionToApplication(string applicationId, List<AddQuestionDto> request)
    {
        return Ok(await _applicationService.AddQuestionToApplication(applicationId, request));

    }

    [HttpPost("create-aplication")]
    public async Task<IActionResult> CreateApplication(CreateApplicationDto request)
    {
        var result = await _applicationService.CreateApplication(request);
        return Ok(result);
    }

    [HttpPut("update-question")]
    public async Task<IActionResult> UpdateQuestion(string applicationId, UpdateQuestionDto request)
    {
        return Ok(await _applicationService.UpdateQuestion(applicationId, request));
    }

    [HttpDelete("delete-question")]
    public async Task<IActionResult> DeleteQuestion(string applicationId, string questionId)
    {
        return Ok(await _applicationService.DeleteQuestionFromApplication(applicationId, questionId));
    }
}
