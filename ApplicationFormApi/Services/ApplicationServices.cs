using ApplicationFormApi.Data;
using ApplicationFormApi.DTO;

namespace ApplicationFormApi.Services;

public class ApplicationServices : IApplicationServices
{
    private readonly IApplicationRepository _applicationRepository;

    public ApplicationServices(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public Task<ApplicationQuestionResponse> AddQuestionToApplication(string applicationId, List<AddQuestionDto> request)
    {
        Task<ApplicationQuestionResponse> response = _applicationRepository.AddQuestionToApplication(applicationId, request);
        return response;
    }

    public async Task<ApplicationResponse> CreateApplication(CreateApplicationDto application)
    {
        ApplicationResponse applicattionResponse = await _applicationRepository.CreateApplication(application);
        return applicattionResponse;
    }

    public async Task<string> DeleteQuestionFromApplication(string applicationId, string questionId)
    {
        string status = await _applicationRepository.DeleteQuestionFromApplication(applicationId, questionId);
        return status;
    }

    public async Task<ApplicationQuestionResponse> LoadApplication(string applicationId)
    {
        ApplicationQuestionResponse loadApp = await _applicationRepository.LoadApplication(applicationId);
        return loadApp;
    }

    public async Task<string> UpdateQuestion(string questionId, UpdateQuestionDto request)
    {
        string update = await _applicationRepository.UpdateQuestion(questionId, request);
        return update;
    }
}
