using Newtonsoft.Json;

namespace ApplicationFormApi.Models;

public class Question
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("description")]
    public string Text { get; set; }

    [JsonProperty("questionType")]
    public QuestionType QuestionType { get; set; }

    [JsonProperty("choices")]
    public IList<string>? Choices { get; set; }
}