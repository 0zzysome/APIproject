using System.Text.Json.Serialization;
public class Pokemon
{
    [JsonPropertyName("name")]
    public string Name { get; set; } 

    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    [JsonPropertyName("is_default")]
    public bool IsDefault { get; set; }
}
