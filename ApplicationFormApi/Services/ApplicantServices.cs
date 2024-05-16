using ApplicationFormApi.Data;
using ApplicationFormApi.DTO;

namespace ApplicationFormApi.Services;

public class ApplicantServices : IApplicantServices
{
    private readonly IApplicantRepository _applicantRepo;

    public ApplicantServices(IApplicantRepository applicantRepo)
    {
        _applicantRepo = applicantRepo;   
    }
    public Task<string> SubmitAplication(string applicationId, ApplicantFormDto request)
    {
        Task<string> submitted = _applicantRepo.SubmitAplication(applicationId, request);

        return submitted;
    }
}
