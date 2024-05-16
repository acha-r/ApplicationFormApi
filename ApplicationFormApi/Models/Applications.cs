using Newtonsoft.Json;

namespace ApplicationFormApi.Models;

public class Applications
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("questions")]
    public List<Question> Questions { get; set; }

    public Applications()
    {
        Questions = new List<Question>();
    }
}
