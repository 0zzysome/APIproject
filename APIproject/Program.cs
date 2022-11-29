using RestSharp;
using System.Text.Json;
using System.Net;

RestClient QuizClient = new("https://jservice.io/api/");

RestRequest QuizRequest = new("random");
PointBoard board = new PointBoard();
Quiz q = null;

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
            WritePoints(board);
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
                    //fortsäter till näs man ska svara
                    break;
                case 2:
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
    WritePoints(board);
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
            q.AddPoints();
            hasAnswered = false;
            break;
        case 3:
            System.Console.WriteLine($"awnser {q.Answer}");
            hasAnswered = false;
            Console.ReadLine();
            break;
        default:
            hasAnswered = false;
            break;
    }


}

if(result)
{
    System.Console.WriteLine("Correct");
    WritePoints(board);
}
else
{
    System.Console.WriteLine("WRONG");
}

Console.ReadLine();












static void WriteQuestion(Quiz Question)
{
    System.Console.WriteLine($" Catagory: {Question.Category.Title}");
    System.Console.WriteLine($"-------------------------------------- ");
    System.Console.WriteLine($"{Question.Question}");
    System.Console.WriteLine($"-------------------------------------- ");
    System.Console.WriteLine("What do you whant to do?");
    System.Console.WriteLine("1. Answer question");
    System.Console.WriteLine("2. Reveal one leter");
    System.Console.WriteLine("3. give Answer(remove later)");
}
static void WritePoints(PointBoard pb)
{
    System.Console.WriteLine($" Points left: {pb.TotalPoints}");
    System.Console.WriteLine($"-------------------------------------- ");

}
static void Menu(Quiz Question)
{
    System.Console.WriteLine($" Catagory: {Question.Category.Title}");
    System.Console.WriteLine($" Difficulty: {Question.Difficulty}");

    // Fråga om använmdaren vill ha den här
    // Om inte
    //  q = null;
    System.Console.WriteLine("Do you whant to awnser the question?");
    System.Console.WriteLine("1. Yes, give me the question.");
    System.Console.WriteLine("2. No, give me a diffrent question.");
}
