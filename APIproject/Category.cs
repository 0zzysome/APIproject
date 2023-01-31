using System;
using System.Text.Json.Serialization;
public class Category
{
    
    [JsonPropertyName("title")]
    public string title { get; set; }
    
}
