using Newtonsoft.Json;

namespace ApplicationFormApi.Models;

public class ApplicantPersonalInformation
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("lastName")]
    public string Lastname { get; set; }

    [JsonProperty("firstName")]
    public string Firstname { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("mobileNumber")]
    public string MobileNumber { get; set; }

    [JsonProperty("applicationId")]
    public string ApplicationId { get; set; }

    [JsonProperty("currentResidence")]
    public string CurrentResdidence { get; set; }

    [JsonProperty("nationality")]
    public string Nationality { get; set; }

    [JsonProperty("idNumber")]
    public string IdNumber { get; set; }

    [JsonProperty("gender")]
    public Gender Gender { get; set; }

    [JsonProperty("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }
}

public enum Gender
{
    Male = 1,
    Female = 2
}
