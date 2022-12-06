using System.Text.Json.Serialization;


public class HardQuiz : PointGiver
{
    //ide av denna klass
    // rätt ger vanliga antal poäng
    // för varje fel förlorar man hälften av svårigheten  
    // men man får inga tips och man får inte välja frågor
    // får man 2000 poäng vinner man (kanske vara på andra också)
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
    
    public int CorrectAmount { get; set; }

    public override void RemovePoints()
    {
        TotalPoints -= Difficulty/2;
    } 
    public override void AddPoints()
    {
        TotalPoints += Difficulty;
    }
}
