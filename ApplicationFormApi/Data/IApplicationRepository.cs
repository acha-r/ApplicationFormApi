using ApplicationFormApi.DTO;

namespace ApplicationFormApi.Data;

public interface IApplicationRepository
{
    Task<ApplicationQuestionResponse> LoadApplication(string applicationId);
    Task<ApplicationQuestionResponse> AddQuestionToApplication(string applicationId, List<AddQuestionDto> request);
    Task<string> DeleteQuestionFromApplication(string applicationId, string questionId);
    Task<ApplicationResponse> CreateApplication(CreateApplicationDto application);
    Task<string> UpdateQuestion(string questionId, UpdateQuestionDto request);
}
