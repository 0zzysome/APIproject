using RestSharp;
using System.Text.Json;
using System.Net;

RestClient QuizClient = new("https://jservice.io/api/");

RestRequest QuizRequest = new("random");
Quiz q = null;
bool hasPointsLeft = true;
while(hasPointsLeft){
    while (q == null)
    {

        RestResponse QuizResponse = QuizClient.GetAsync(QuizRequest).Result;

        if (QuizResponse.StatusCode == HttpStatusCode.OK)
        {
            Console.Clear();
            q = JsonSerializer.Deserialize<List<Quiz>>(QuizResponse.Content).First();
            
            // fixa clues genom att sätta in det i annan class (???)

            bool MadeChoice = false;
            int Choice;
            while (!MadeChoice)
            {
                WritePoints();
                // ritar ut texten
                Menu(q);
                
                // fixa med andra classen 
                // Fråga om använmdaren vill ha den här
                // Om inte
                //  q = null;

                MadeChoice = int.TryParse(Console.ReadLine(), out Choice);
                switch (Choice)
                {
                    case 1:
                        //fortsäter och skriver ut frågan
                        break;
                    case 2:
                        // ger en ny fråga 
                        q = null;
                        break;
                    default:
                        MadeChoice = false;
                        break;
                }

            }


        }
        else
        {
            System.Console.WriteLine("not found");
            Console.ReadLine();
        }
    }
    bool result=false;
    bool hasAnswered = false;
    Clues HiddenAnswer = new Clues(q);
    while (!hasAnswered)
    {
        
        Console.Clear();
        WritePoints();
        WriteQuestion(q);
        HiddenAnswer.WriteHidenAnswer();
        int i;
        
        hasAnswered = int.TryParse(Console.ReadLine(), out i);
        switch (i)
        {
            case 1:
                System.Console.WriteLine("Write answer:");
                string PlayerGuess = Console.ReadLine();
                result = q.Answer.Equals(PlayerGuess, StringComparison.OrdinalIgnoreCase);

                
                break;
            case 2:
                
                HiddenAnswer.RevealLetter(q, HiddenAnswer);
                
                hasAnswered = false;
                break;
            case 3:
                
                //HiddenAnswer.CluesLeft = 0;
                System.Console.WriteLine($"{q.Answer}");
                hasAnswered = false;
                Console.ReadLine();
                break;
            default:
                hasAnswered = false;
                break;
        }
        // ser om spelaren har använt alla sina ledtrådar 
        // eller om de har slut på poäng
        if(HiddenAnswer.CluesLeft<= 0 || IsOutOfpoints())
        {
            hasAnswered = true;
            result = false;
        }

    }

    if(result)
    {
        System.Console.WriteLine($"Correct! You gained {q.Difficulty} points!");
        q.AddPoints();
        
    }
    else
    {
        q.RemovePoints();
        System.Console.WriteLine($"WRONG! You lost {q.Difficulty} points");
        System.Console.WriteLine($"Correct answer: {q.Answer}");
    }
    if(IsOutOfpoints())
    {
        hasPointsLeft = false;
        System.Console.WriteLine("You are out of points");
    }
    WritePoints();
    Console.ReadLine();
    q = null;
}











static void WriteQuestion(Quiz Question)
{
    System.Console.WriteLine($" Catagory: {Question.Category.Title}");
    System.Console.WriteLine($"-------------------------------------- ");
    System.Console.WriteLine($"{Question.Question}");
    System.Console.WriteLine($"-------------------------------------- ");
    System.Console.WriteLine("What do you whant to do?");
    System.Console.WriteLine("1. Answer question");
    System.Console.WriteLine("2. Reveal one leter(uses 50 points)");
    System.Console.WriteLine("3. Give Answer(Removes points and gives you a diffrent question.)");
}
static void WritePoints()
{
    System.Console.WriteLine($" Points left: {PointGiver.TotalPoints}");
    System.Console.WriteLine($"-------------------------------------- ");

}
static void Menu(Quiz Question)
{
    System.Console.WriteLine($" Catagory: {Question.Category.Title}");
    System.Console.WriteLine($" Difficulty: {Question.Difficulty}");

    // Fråga om använmdaren vill ha den här
    // Om inte
    //  q = null;
    System.Console.WriteLine("Do you want to answer the question?");
    System.Console.WriteLine("1. Yes, give me the question.");
    System.Console.WriteLine("2. No, give me a diffrent question.");
}
static bool IsOutOfpoints()
{
    if(PointGiver.TotalPoints<= 0)
    {
        return true;
    }
    else
    {
        return false;
    }
}