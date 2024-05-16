using ApplicationFormApi.DTO;
using ApplicationFormApi.Models;
using Microsoft.Azure.Cosmos;

namespace ApplicationFormApi.Data;

public class ApplicationRepository : IApplicationRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _applicationsContainer;
    private readonly Container _questionsContainer;

    public ApplicationRepository(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        _applicationsContainer = cosmosClient.GetContainer("ApplicationsDb", "Applications");
        _questionsContainer = cosmosClient.GetContainer("ApplicationsDb", "Questions");
    }

    public async Task<ApplicationQuestionResponse> AddQuestionToApplication(string applicationId, List<AddQuestionDto> request)
    {
        if (request.Count == 0)
        {
            throw new Exception("You must ask at least one question");
        }

        ItemResponse<Applications> application = await _applicationsContainer.ReadItemAsync<Applications>(applicationId, new PartitionKey(applicationId));

        List<Question> applicationQuestions = new();

        foreach (var question in request)
        {
            if ((question.QuestionType == QuestionType.MultipleChoice && (question.Choices == null || question.Choices.Count < 3))
                || (question.QuestionType == QuestionType.DropDown && (question.Choices == null || question.Choices.Count < 2))
                || (question.QuestionType == QuestionType.YesNo && (question.Choices == null || question.Choices.Count != 2)))
            {
                throw new InvalidOperationException(
                    question.QuestionType == QuestionType.MultipleChoice ? "Multiple choice questions must have atleast three options" :
                    question.QuestionType == QuestionType.DropDown ? "Drop down questions must have atleast two options" :
                    "Yes/No questions must have only yes or no");
            }

            if (question.QuestionType == QuestionType.YesNo)
            {
                foreach (var choice in question.Choices)
                {
                    if (choice.ToLower().Trim() != "yes" || choice.ToLower().Trim() != "no")
                        throw new InvalidOperationException("Yes / No questions must have only yes or no answers");
                }
            }

            Question newQuestion = new()
            {
                Id = Guid.NewGuid().ToString(),
                QuestionType = question.QuestionType,
                Text = question.Text
            };

            foreach (var choice in question.Choices)
            {
                newQuestion.Choices.Add(choice.ToLower().Trim());
            }

            await _questionsContainer.CreateItemAsync(newQuestion, new PartitionKey(newQuestion.Id));
            applicationQuestions.Add(newQuestion);
        }

        application.Resource.Questions.AddRange(applicationQuestions);

        await _applicationsContainer.UpsertItemAsync(application.Resource, new PartitionKey(applicationId));

        return new ApplicationQuestionResponse()
        {
            ApplicationName = application.Resource.Name,
            ApplicationDescription = application.Resource.Description,
            Questions = applicationQuestions
        };
    }

    public async Task<ApplicationResponse> CreateApplication(CreateApplicationDto request)
    {
        if (request == null)
        {
            throw new ArgumentNullException();
        }

        var query = new QueryDefinition("SELECT * FROM c WHERE c.name = @name")
           .WithParameter("@name", request.Name);

        var iterator = _applicationsContainer.GetItemQueryIterator<Applications>(query);

        var existingApplication = (await iterator.ReadNextAsync()).FirstOrDefault();

        if (existingApplication != null)
        {
            throw new InvalidOperationException($"applicant with {request.Name} already exisit");
        }

        Applications application = new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
        };

        await _applicationsContainer.CreateItemAsync(application, new PartitionKey(application.Id));

        return new ApplicationResponse()
        {
            Name = application.Name,
            Id = application.Id,
        };
    }

    public async Task<string> DeleteQuestionFromApplication(string applicationId, string questionId)
    {
        ItemResponse<Applications> application = await _applicationsContainer.ReadItemAsync<Applications>(applicationId, new PartitionKey(applicationId));
        ItemResponse<Question> question = await _questionsContainer.ReadItemAsync<Question>(questionId, new PartitionKey(questionId));

        await _questionsContainer.DeleteItemAsync<Question>(questionId, new PartitionKey(questionId));
        application.Resource.Questions.Remove(question.Resource);

        return "Deleted";
    }

    public async Task<ApplicationQuestionResponse> LoadApplication(string applicationId)
    {
        ItemResponse<Applications> application = await _applicationsContainer.ReadItemAsync<Applications>(applicationId, new PartitionKey(applicationId))
            ?? throw new KeyNotFoundException("Invalid application id");


        return new ApplicationQuestionResponse()
        {
            ApplicationName = application.Resource.Name,
            ApplicationDescription = application.Resource.Description,
            Questions = application.Resource.Questions
        };
    }

    public async Task<string> UpdateQuestion(string applicationId, UpdateQuestionDto request)
    {
        if (request == null)
            throw new ArgumentNullException();

        ItemResponse<Applications> application = await _applicationsContainer.ReadItemAsync<Applications>(applicationId, new PartitionKey(applicationId));

        ItemResponse<Question> question = await _questionsContainer.ReadItemAsync<Question>(request.QuestionId, new PartitionKey(request.QuestionId));

        question.Resource.Text = request.Text;
        question.Resource.QuestionType = request.QuestionType;

        if (
            (request.QuestionType == QuestionType.MultipleChoice && (request.Choices == null || request.Choices.Count < 3))
            || (request.QuestionType == QuestionType.DropDown && (request.Choices == null || request.Choices.Count < 2))
            || (request.QuestionType == QuestionType.YesNo && (request.Choices == null || request.Choices.Count != 2)))
        {
            throw new InvalidOperationException(
                request.QuestionType == QuestionType.MultipleChoice ? "Multiple choice questions must have atleast three options" :
                request.QuestionType == QuestionType.DropDown ? "Drop down questions must have atleast two options" :
                "Yes/No questions must have only yes or no");
        }

        if (request.QuestionType == QuestionType.YesNo)
        {
            foreach (var choice in request.Choices)
            {
                if (choice.ToLower().Trim() != "yes" || choice.ToLower().Trim() != "no")
                    throw new InvalidOperationException("Yes / No questions must have only yes or no answers");
            }
        }

        question.Resource.Choices.Clear();

        foreach (var choice in request.Choices)
        {
            question.Resource.Choices.Add(choice.ToLower().Trim());
        }

        await _questionsContainer.UpsertItemAsync(question.Resource, new PartitionKey(request.QuestionId));

        var applicationQuestion = application.Resource.Questions.Where(x => x.Id == request.QuestionId).FirstOrDefault();
        applicationQuestion.Choices = question.Resource.Choices;

        await _applicationsContainer.UpsertItemAsync(application.Resource, new PartitionKey(applicationId));

        return "Question updated";
    }
}
