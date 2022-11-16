using RestSharp;
using System.Text.Json;
using System.Net;

RestClient QuizClient = new("https://jservice.io/api/");


RestRequest QuizRequest = new("random");
RestRequest QuizRequestClues = new("clues");
Quiz q = null;

while (q == null)
{

    RestResponse QuizResponse = QuizClient.GetAsync(QuizRequest).Result;

    if (QuizResponse.StatusCode == HttpStatusCode.OK)
    {
        q = JsonSerializer.Deserialize<List<Quiz>>(QuizResponse.Content).First();

        // fixa clues genom att sätta in det i annan class (???)
        QuizRequest.AddParameter("clues", q.Catagory);
        bool MadeChoice = false;
        int Choice;
        while(!MadeChoice)
        {
            // fixa med andra classen 
            System.Console.WriteLine($" Catagory: {}");
            System.Console.WriteLine($" Difficulty: {q.Difficulty}");
            System.Console.WriteLine($" Amount of clues: {}");
            // Fråga om använmdaren vill ha den här
            // Om inte
            //  q = null;
            System.Console.WriteLine("Do you whant to awnser the question?");
            System.Console.WriteLine("1. Yes, give me the question.");
            System.Console.WriteLine("2. No, give me a diffrent question.");
            MadeChoice = int.TryParse(Console.ReadLine(), out Choice);
            switch(Choice)
            {
                case 1:
                    WriteQuestion(q);
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
    }
}
Console.ReadLine();
static void WriteQuestion(Quiz ChosenQuestion)
{

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

