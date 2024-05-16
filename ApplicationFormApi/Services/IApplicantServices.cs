using ApplicationFormApi.DTO;

namespace ApplicationFormApi.Services;

public interface IApplicantServices
{
    Task<string> SubmitAplication(string applicationId, ApplicantFormDto request);
}