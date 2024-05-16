using ApplicationFormApi.DTO;
using ApplicationFormApi.Models;
using Microsoft.Azure.Cosmos;

namespace ApplicationFormApi.Data;

public class ApplicantRepository : IApplicantRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _applicationsContainer;
    private readonly Container _questionsContainer;
    private readonly Container _personalInformation;
    private readonly Container _additionalInformation;

    public ApplicantRepository(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        _applicationsContainer = cosmosClient.GetContainer("ApplicationsDb", "Applications");
        _questionsContainer = cosmosClient.GetContainer("ApplicationsDb", "Questions");
        _personalInformation = cosmosClient.GetContainer("ApplicationsDb", "PersonalInformation");
        _additionalInformation = cosmosClient.GetContainer("ApplicationsDb", "AdditionalInformation");
    }

    public async Task<string> SubmitAplication(string applicationId, ApplicantFormDto request)
    {
        if (request.PersonalInformation is null || request.AdditionalInformation is null)
        {
            throw new ArgumentNullException();
        }

        var application = await _applicationsContainer.ReadItemAsync<Applications>(applicationId, new PartitionKey(applicationId));


        var query = new QueryDefinition("SELECT * FROM c WHERE c.email = @email")
            .WithParameter("@email", request.PersonalInformation.Email);

        var iterator = _personalInformation.GetItemQueryIterator<ApplicantPersonalInformation>(query);

        var existingApplicant = (await iterator.ReadNextAsync()).FirstOrDefault();

        if (existingApplicant != null)
        {
            throw new InvalidOperationException($"applicant with {request.PersonalInformation.Email} already exisit");
        }

        ItemResponse<Question> nationalityQuestion = await _questionsContainer.ReadItemAsync<Question>(request.PersonalInformation.NationalityQuestionId,  //pre-seed?
            new PartitionKey(request.PersonalInformation.NationalityQuestionId));

        var nationality = nationalityQuestion.Resource.Choices[request.PersonalInformation.NationalityId]
            ?? throw new KeyNotFoundException("Invalid choice");

        ApplicantPersonalInformation personalInformation = new()
        {
            Id = Guid.NewGuid().ToString(),
            Lastname = request.PersonalInformation.Lastname,
            Firstname = request.PersonalInformation.Firstname,
            Email = request.PersonalInformation.Email,
            MobileNumber = request.PersonalInformation.MobileNumber,
            ApplicationId = applicationId,
            CurrentResdidence = request.PersonalInformation.CurrentResdidence,
            Nationality = nationality,
            Gender = request.PersonalInformation.Gender,
            DateOfBirth = request.PersonalInformation.DateOfBirth
        };

        await _personalInformation.CreateItemAsync(personalInformation, new PartitionKey(personalInformation.Id));

        ApplicantAdditionalInformation additionalInformation = new()
        {
            Id = Guid.NewGuid().ToString(),
            ApplicantId = personalInformation.Id,
            ApplicationId = applicationId,
        };

        if (request.AdditionalInformation.Answers.Count > 0)
        {
            foreach (var answer in request.AdditionalInformation.Answers)
            {
                var question = application.Resource.Questions.FirstOrDefault(x => x.Id == answer.Key) ??
                    throw new KeyNotFoundException("Invalid questionId");

                var response = answer.Value; //by default, input is a string

                switch (question.QuestionType)
                {
                    case QuestionType.YesNo:
                        var validResponse = response.ToString().ToLower();

                        if (validResponse != "yes" && validResponse != "no")
                        {
                            throw new Exception($"{response} is an invalid response");
                        }

                        additionalInformation.Answers.Add(question.Id, validResponse);
                        break;

                    case QuestionType.MultipleChoice:
                        var choices = response.ToString().Split(',');

                        foreach (var choice in choices)
                        {
                            if (!question.Choices.Contains(choice.ToLower().Trim()))
                            {
                                throw new Exception($"{choice} is an invalid response");
                            }
                        }

                        additionalInformation.Answers.Add(question.Id, choices);

                        break;

                    case QuestionType.Date:

                        if (!DateTime.TryParse(response.ToString(), out var date))
                        {
                            throw new Exception($"{response} is an invalid response");
                        }

                        additionalInformation.Answers.Add(question.Id, date);
                        break;

                    case QuestionType.DropDown:
                        var validChoice = response.ToString().ToLower().Trim();

                        if (!question.Choices.Contains(validChoice))
                        {
                            throw new Exception($"{response} is an invalid response");
                        }

                        additionalInformation.Answers.Add(question.Id, validChoice);
                        break;

                    case QuestionType.Number:

                        if (int.TryParse(response.ToString(), out var number))
                        {
                            throw new Exception($"{response} is an invalid response");
                        }

                        additionalInformation.Answers.Add(question.Id, number);
                        break;

                    case QuestionType.Paragraph:

                        additionalInformation.Answers.Add(question.Id, response.ToString());
                        break;

                    default:
                        break;
                }
            }
        }

        await _additionalInformation.CreateItemAsync(additionalInformation, new PartitionKey(personalInformation.Id));

        return "Application submitted";

    }
}
