using Newtonsoft.Json;
using System.Reflection;

namespace ApplicationFormApi.Models;

public class ApplicantAdditionalInformation
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("applicantId")]
    public string ApplicantId { get; set; }

    [JsonProperty("applicationId")]
    public string ApplicationId { get; set; }

    public Dictionary<string, Object> Answers { get; set; }

    public ApplicantAdditionalInformation()
    {
        Answers = new Dictionary<string, object>();
    }
}
