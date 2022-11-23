using RestSharp;
using System.Text.Json;
using System.Net;

RestClient QuizClient = new("https://jservice.io/api/");





RestRequest QuizRequest = new("random");
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
bool hasAnswered = false;
Clues HiddenAnswer = new Clues(q);
while (!hasAnswered)
{
    
    Console.Clear();
    WriteQuestion(q);
    HiddenAnswer.WriteHidenAnswer();
    int i;
    
    hasAnswered = int.TryParse(Console.ReadLine(), out i);
    switch (i)
    {
        case 1:
            string PlayerGuess = Console.ReadLine();
            //comepare answer and guess later
            break;
        case 2:
            
            HiddenAnswer.RevealLetter(q);
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

Console.ReadLine();












static void WriteQuestion(Quiz Question)
{
    System.Console.WriteLine($" Catagory: {Question.Category.Title}");
    System.Console.WriteLine($"-------------------------------------- ");
    System.Console.WriteLine($"{Question.Question}");
    System.Console.WriteLine("What do you whant to do?");
    System.Console.WriteLine("1. Answer question");
    System.Console.WriteLine("2. Reveal one leter");
    System.Console.WriteLine("3. give Answer(remove later)");
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
/*
RestClient pokeClient = new("https://pokeapi.co/api/v2/");

RestRequest pokeRequest = new("pokemon/416");

RestResponse PokeResponse = pokeClient.GetAsync(pokeRequest).Result;

if (PokeResponse.StatusCode == HttpStatusCode.OK)
{
    Pokemon p = JsonSerializer.Deserialize<Pokemon>(PokeResponse.Content);

    System.Console.WriteLine(p.Name);
    System.Console.WriteLine(p.Weight);

}
else
{
    System.Console.WriteLine("not found");
}
*/

