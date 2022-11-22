using System.Text.Json.Serialization;

public class Quiz
{
    
    [JsonPropertyName("question")]
    public string Question { get; set; }

    [JsonPropertyName("value")]
    public int Difficulty { get; set; }

    [JsonPropertyName("category_id")]
    public int CategoryId { get; set; }

    [JsonPropertyName("answer")]
    public string Answer { get; set; }

    [JsonPropertyName("category")]
    public Category Category {get; set;}

}

