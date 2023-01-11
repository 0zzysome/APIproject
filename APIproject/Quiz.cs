using System.Text.Json.Serialization;

public class Quiz : PointGiver
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
    
    protected Random Generator = new();
    
    public void CleanAnswer()
    {
        Answer = Answer.Replace("<i>", "");
        Answer = Answer.Replace("</i>", "");
        Answer = Answer.Replace("\"", "");
        Answer = Answer.Replace("\\", "");
    }
    

    public override void AddPoints()
    {
        TotalPoints += Difficulty;
    }
    public override void RemovePoints()
    {
        TotalPoints -= Difficulty;
    }


    public void WriteQuestion()
    {
        System.Console.WriteLine($" Category: {Category.Title} || Difficulty: {Difficulty}");
        System.Console.WriteLine($"-------------------------------------- ");
        System.Console.WriteLine($"{Question}");
        System.Console.WriteLine($"-------------------------------------- ");
        System.Console.WriteLine("What do you whant to do?");
        System.Console.WriteLine("1. Answer question");
        System.Console.WriteLine("2. Reveal one leter(uses 50 points)");
        System.Console.WriteLine("3. Give Answer(Removes points and gives you a diffrent question.)");
    }
    public void Menu()
    {
        System.Console.WriteLine($" Category: {Category.Title}");
        System.Console.WriteLine($" Difficulty: {Difficulty}");
        System.Console.WriteLine("Do you whant to answer the question?");
        System.Console.WriteLine("1. Yes, give me the question.");
        System.Console.WriteLine("2. No, give me a diffrent question.");
    }
    public override bool HasWonGame()
    {
        if(PointGiver.TotalPoints>=2300)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

