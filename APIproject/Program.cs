using RestSharp;
using System.Text.Json;
using System.Net;

RestClient QuizClient = new("https://jservice.io/api/");

RestRequest QuizRequest = new("random");


// startmeny
bool hasGameType = false;
int GameType= 3;
while(!hasGameType)
{
    // skapa enn meny här sen.
    System.Console.WriteLine("chose game type ");
    hasGameType = int.TryParse(Console.ReadLine(), out GameType);
    //om spelaren valde 1 eller 2 så ändras inte hasGameType och koden fortsätter.
    if(GameType != 1 || GameType != 2)
    {
        System.Console.WriteLine("No choice was made, make shure to only write a letter. Press enter to continue");
        Console.ReadLine();
        
        hasGameType = false;
    }

}
//normal frågesport
if(GameType== 1)
{
    Quiz q = null;
    bool hasPointsLeft = true;
    while(hasPointsLeft){

        while (q == null)
        {

            RestResponse QuizResponse = QuizClient.GetAsync(QuizRequest).Result;
            // ser till så att inga fel upstod när den hämta data
            if (QuizResponse.StatusCode == HttpStatusCode.OK)
            {
                Console.Clear();
                //skapar frågan
                q = JsonSerializer.Deserialize<List<Quiz>>(QuizResponse.Content).First();
                bool MadeChoice = false;
                int Choice;
                //körs medans spelaren inte har valt
                while (!MadeChoice)
                {
                    Console.Clear();
                    // ritar ut texten
                    // Fråga om använmdaren vill ha den här frågan
                    WritePoints();
                    Menu(q);
                    //tar input och försöker göra den till en int
                    MadeChoice = int.TryParse(Console.ReadLine(), out Choice);
                    switch (Choice)
                    {
                        case 1:
                            //fortsäter och skriver ut frågan
                            break;
                        case 2:
                            
                            // ger en ny fråga för användren skrev 2
                            q = null;
                            break;
                        default:
                            // om inputen var fel så körs denna och låter användaren göra ett nytt val
                            System.Console.WriteLine("No choice was made, make shure to only write a letter. Press enter to continue");
                            Console.ReadLine();
                            
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
        // när använaren har valt fråga så körs detta 
        bool result=false;
        bool hasAnswered = false;
        Clues HiddenAnswer = new Clues(q);
        while (!hasAnswered)
        {
            //skapar meny 
            Console.Clear();
            WritePoints();
            WriteQuestion(q);
            HiddenAnswer.WriteHidenAnswer();
            int i;
            //tar input och försöker göra den till en int och sparar den 
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
                
                    HiddenAnswer.CluesLeft = 0;
                    System.Console.WriteLine($"{q.Answer}");
                    hasAnswered = false;
                    Console.ReadLine();
                    break;
                default:
                    System.Console.WriteLine("No choice was made, make shure to only write a letter. Press enter to continue");
                    Console.ReadLine();
                    hasAnswered = false;
                    break;
            }
            // ser om spelaren har använt alla sina ledtrådar 
            // eller om de har slut på poäng
            if(HiddenAnswer.CluesLeft<= 0 || IsOutOfpoints())
            {
                System.Console.WriteLine("you have ran out of points or clues.");
                
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
        System.Console.WriteLine("Press enter to continue");
        Console.ReadLine();
        q = null;
    }
}
//svår frågesport
else
{
    RestResponse QuizResponse = QuizClient.GetAsync(QuizRequest).Result;
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