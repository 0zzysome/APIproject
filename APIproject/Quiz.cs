using System.Text.Json.Serialization;

public class Quiz : PointGiver
{
    
    [JsonPropertyName("question")]
    public string question { get; set; }

    [JsonPropertyName("value")]
    public int difficulty { get; set; }

    [JsonPropertyName("category_id")]
    public int categoryId { get; set; }

    [JsonPropertyName("answer")]
    public string answer { get; set; }

    [JsonPropertyName("category")]
    public Category category {get; set;}
    
    protected Random generator = new();
    
    public void CleanAnswer()
    {
        answer = answer.Replace("<i>", "");
        answer = answer.Replace("</i>", "");
        answer = answer.Replace("\"", "");
        answer = answer.Replace("\\", "");
    }
    

    public override void AddPoints()
    {
        totalPoints += difficulty;
    }
    public override void RemovePoints()
    {
        totalPoints -= difficulty;
    }


    public void WriteQuestion()
    {
        System.Console.WriteLine($" Category: {category.title} || Difficulty: {difficulty}");
        System.Console.WriteLine($"-------------------------------------- ");
        System.Console.WriteLine($"{question}");
        System.Console.WriteLine($"-------------------------------------- ");
        System.Console.WriteLine("What do you whant to do?");
        System.Console.WriteLine("1. Answer question");
        System.Console.WriteLine("2. Reveal one leter(uses 50 points)");
        System.Console.WriteLine("3. Give Answer(Removes points and gives you a diffrent question.)");
    }
    public void Menu()
    {
        System.Console.WriteLine($" Category: {category.title}");
        System.Console.WriteLine($" Difficulty: {difficulty}");
        System.Console.WriteLine("Do you whant to answer the question?");
        System.Console.WriteLine("1. Yes, give me the question.");
        System.Console.WriteLine("2. No, give me a diffrent question.");
    }
    public override bool HasWonGame()
    {
        if(PointGiver.totalPoints>=2300)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

