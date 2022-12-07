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

    

    [JsonPropertyName("answer")]
    public string Answer { get; set; }

    [JsonPropertyName("category")]
    public Category Category {get; set;}
    //Lägg till: fixa svaren och mer
    public void Clean()
    {
        Answer = Answer.Replace("<i>", "");
        Answer = Answer.Replace("<i>", "");
    }

    public override void RemovePoints()
    {
        TotalPoints -= Difficulty/2;
    } 
    public override void AddPoints()
    {
        TotalPoints += Difficulty;
    }
    public void WriteQuestion()
    {
        System.Console.WriteLine($" Catagory: {Category.Title} || Difficulty: {Difficulty}");
        System.Console.WriteLine($"-------------------------------------- ");
        System.Console.WriteLine($"{Question}");
        System.Console.WriteLine($"-------------------------------------- ");
        System.Console.WriteLine("What is the answer? Press enter to submit answer.");
        //Lägg till: ta bort detta 
        System.Console.WriteLine($"Answer: {Answer}");
    }
}