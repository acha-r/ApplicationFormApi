using ApplicationFormApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ApplicationFormApi.DTO;

public class PersonalInformationDto
{
    [Required]
    public string Lastname { get; set; }

    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string MobileNumber { get; set; }

    [Required]
    public string CurrentResdidence { get; set; }

    [Required]
    public string NationalityQuestionId { get; set; }
    [Required]
    public int NationalityId { get; set; }

    [Required]
    public string IdNumber { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
}

class QuestionAnswer
{
    public string QuestionId { get; set; }
    public string AnswerId { get; set; }
}

public class AdditionalInformationDto
{
    [Required]
    public Dictionary<string, Object> Answers { get; set; }
}

public class ApplicantFormDto
{
    public PersonalInformationDto PersonalInformation { get; set; }
    public AdditionalInformationDto AdditionalInformation { get; set; }
}
