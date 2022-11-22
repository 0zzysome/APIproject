using System;
using System.Text.Json.Serialization;
public class Category
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("clues_count")]
    public int CluesAmount { get; set; }
}
