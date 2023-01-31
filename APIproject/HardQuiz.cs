using System.Text.Json.Serialization;


public class HardQuiz : PointGiver
{
    //ide av denna klass
    // rätt ger vanliga antal poäng
    // för varje fel förlorar man hälften av svårigheten  
    // men man får inga tips och man får inte välja frågor
    // får man 2000 poäng vinner man (kanske vara på andra också)
    [JsonPropertyName("question")]
    public string question { get; set; }

    [JsonPropertyName("value")]
    public int difficulty { get; set; }

    [JsonPropertyName("answer")]
    public string answer { get; set; }

    [JsonPropertyName("category")]
    public Category category {get; set;}
    
    public void CleanAnswer()
    {
        answer = answer.Replace("<i>", "");
        answer = answer.Replace("</i>", "");
        answer = answer.Replace("\"", "");
        answer = answer.Replace("\\", "");
    }

    public override void RemovePoints()
    {
        totalPoints -= difficulty/2;
    } 
    public override void AddPoints()
    {
        totalPoints += difficulty/2;
    }
    public void WriteQuestion()
    {
        System.Console.WriteLine($" Category: {category.title} || Difficulty: {difficulty}");
        System.Console.WriteLine($"-------------------------------------- ");
        System.Console.WriteLine($"{question}");
        System.Console.WriteLine($"-------------------------------------- ");
        System.Console.WriteLine("What is the answer? Press enter to submit answer.");
        
    }
    public override bool HasWonGame()
    {
        if(PointGiver.totalPoints>=2000)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
