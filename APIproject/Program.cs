using RestSharp;
using System.Text.Json;
using System.Net;

RestClient QuizClient = new("https://jservice.io/api/");

RestRequest QuizRequest = new("random");

bool HasPointsLeft = true;
// startmeny
bool HasGameType = false;
int GameType= 3;
while(!HasGameType)
{
    // lägg till: en meny här sen.
    System.Console.WriteLine("chose game type ");
    HasGameType = int.TryParse(Console.ReadLine(), out GameType);
    //om spelaren valde 1 eller 2 så ändras inte hasGameType och koden fortsätter.
    //btw jag vet att koden är dålig men den funkar
    if(GameType ==1 )
    {
        //lägg till: vilen de valde
        HasGameType = true;

    }
    else if(GameType == 2)
    {
        //lägg till: vilen de valde
        HasGameType = true;

    }
    else
    {
        //lägg till: felmedelande
        HasGameType = false;
    }

}
//normal frågesport
if(GameType== 1)
{
    Quiz q = null;
    while(HasPointsLeft){

        while (q == null)
        {

            RestResponse QuizResponse = QuizClient.GetAsync(QuizRequest).Result;
            // ser till så att inga fel upstod när den hämta data
            if (QuizResponse.StatusCode == HttpStatusCode.OK)
            {
                Console.Clear();
                //skapar frågan
                q = JsonSerializer.Deserialize<List<Quiz>>(QuizResponse.Content).First();
                q.Clean();
                bool MadeChoice = false;
                int Choice;
                //körs medans spelaren inte har valt
                while (!MadeChoice)
                {
                    Console.Clear();
                    // ritar ut texten
                    // Frågar om användaren vill ha den här frågan eller byta den
                    q.WritePoints();
                    q.Menu();
                    //tar input och gör till en int 
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
            //körs endast om något fel upstod då man förfrågade information från API:n
            else
            {
                System.Console.WriteLine("not found");
                Console.ReadLine();
            }
        }
        // när använaren har valt fråga så körs detta 
        bool Result=false;
        bool HasAnswered = false;
        Clues HiddenAnswer = new Clues(q);
        while (!HasAnswered)
        {
            //skapar meny 
            Console.Clear();
            q.WritePoints();
            q.WriteQuestion();
            HiddenAnswer.WriteHidenAnswer();
            int i;
            //tar input och försöker göra den till en int och sparar den 
            HasAnswered = int.TryParse(Console.ReadLine(), out i);
            switch (i)
            {
                case 1:
                    System.Console.WriteLine("Write answer:");
                    //gör så att spelaren lan skriva in sin gissning 
                    string PlayerGuess = Console.ReadLine();
                    //gämför gissningen med svaret och ger ett reslutat
                    Result = q.Answer.Equals(PlayerGuess, StringComparison.OrdinalIgnoreCase);
                    break;
                case 2:
                    HiddenAnswer.RevealLetter(q);
                    HasAnswered = false;
                    break;
                case 3:
                    //blir san så programet fortsätter 
                    HasAnswered = true;
                    // ser till att programet markerar svaret som fel då spelaren inte svarade
                    Result = false;
                    System.Console.WriteLine($"{q.Answer}");
                    Console.ReadLine();
                    break;
                default:
                    System.Console.WriteLine("No choice was made, make shure to only write a letter. Press enter to continue");
                    Console.ReadLine();
                    HasAnswered = false;
                    break;
            }
            // ser om spelaren har använt alla sina ledtrådar 
            // eller om de har slut på poäng
            if(HiddenAnswer.CluesLeft<= 0 || IsOutOfpoints())
            {
                System.Console.WriteLine("you have ran out of points or clues.");
                
                HasAnswered = true;
                Result = false;
            }

        }

        if(Result)
        {
            System.Console.WriteLine($"CORRECT! You gained {q.Difficulty} points!");
            q.AddPoints();
            
        }
        else
        {
            q.RemovePoints();
            System.Console.WriteLine($"WRONG! You lost {q.Difficulty} points");
            System.Console.WriteLine($"Correct answer: {q.Answer}");
        }
        //kollar om spelaren har poäng kvar och avlsutar while satsen om det har slut på poäng
        if(IsOutOfpoints())
        {
            HasPointsLeft = false;
            System.Console.WriteLine("You are out of points");
        }
        q.WritePoints();
        System.Console.WriteLine("Press enter to continue");
        Console.ReadLine();
        q = null;
    }
}
//svår frågesport
else
{   
    
    HardQuiz HardQ = null;
    //kör spelet medans spelaren har poäng kvar.
    while(!IsOutOfpoints())
    {
        Console.Clear();
        //hämtar en respons från api 
        RestResponse QuizResponse = QuizClient.GetAsync(QuizRequest).Result;
        bool IsCorrect = false;
        //sparar värderna från responsen i variabeln
        HardQ = JsonSerializer.Deserialize<List<HardQuiz>>(QuizResponse.Content).First();
        //skriver ut meny
        HardQ.WritePoints();
        HardQ.WriteQuestion();
        string PlayerGuess = Console.ReadLine();
        // svaret gämförs och sparas i en boolean
        IsCorrect = HardQ.Answer.Equals(PlayerGuess, StringComparison.OrdinalIgnoreCase);

        if(IsCorrect)
        {
            System.Console.WriteLine($"CORRECT! You gained {HardQ.Difficulty} points!");
            HardQ.AddPoints();
            
        }
        else
        {
            
            System.Console.WriteLine($"WRONG! You lost {HardQ.Difficulty/2} points");
            System.Console.WriteLine($"Correct answer: {HardQ.Answer}");
            HardQ.RemovePoints();
        }
        Console.ReadLine();
        
    }
    System.Console.WriteLine("You are out of points. Press enter to continue");
}




//lägg till: ike statiska och lägg i klasser istället

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