using ApplicationFormApi.Models;

namespace ApplicationFormApi.DTO;

public class CreateApplicationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class ApplicationResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class AddQuestionDto
{
    public QuestionType QuestionType { get; set; }
    public string Text { get; set; }
    public IList<string>? Choices { get; set; }
}

public class UpdateQuestionDto
{
    public string QuestionId { get; set; }
    public QuestionType QuestionType { get; set; }
    public string Text { get; set; }
    public IList<string>? Choices { get; set; }
}

public class ApplicationQuestionResponse
{
    public string ApplicationId { get; set; }
    public string ApplicationName { get; set; }
    public string ApplicationDescription { get; set; }
    public IList<Question> Questions { get; set; }
}
