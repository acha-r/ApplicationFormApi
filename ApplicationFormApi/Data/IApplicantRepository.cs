using ApplicationFormApi.DTO;

namespace ApplicationFormApi.Data;

public interface IApplicantRepository
{
    Task<string> SubmitAplication(string applicationId, ApplicantFormDto request);
}