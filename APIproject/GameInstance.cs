using System;
using RestSharp;
using System.Text.Json;
using System.Net;

public class GameInstance
{
    RestClient QuizClient = new("https://jservice.io/api/");

    RestRequest QuizRequest = new("random");
    //boolean hanterar om spelet ska avlutas.
    private bool GameIsGoing = true;
    //hanterar om spelaren har gjort sit val av gamemode
    private bool HasGameType = false;
    public int GameType = 3;
    // startmeny
    public void ShowStartMenu()
    {
        while (!HasGameType)
        {

            Console.Clear();
            // lägg till: mindre kod 
            DrawStartMenuText();
            HasGameType = int.TryParse(Console.ReadLine(), out GameType);
            //om spelaren valde 1 eller 2 så ändras inte hasGameType och koden fortsätter.
            //btw jag vet att koden är dålig men den funkar
            if (GameType == 1)
            {
                //lägg till: vilen de valde
                System.Console.WriteLine("You chose: Normal mode");
                FullStop();
                HasGameType = true;

            }
            else if (GameType == 2)
            {
                //lägg till: vilen de valde
                System.Console.WriteLine("You chose: Hard mode");
                FullStop();
                HasGameType = true;

            }
            else
            {
                System.Console.WriteLine("Make sure to write a letter from the menu.");
                FullStop();
                HasGameType = false;
            }

        }
    }
    //normal frågesport
    public void StartNormalQuiz()
    {
        Quiz q = null;
        while (GameIsGoing)
        {
            while (q == null)
            {
                RestResponse QuizResponse = QuizClient.GetAsync(QuizRequest).Result;
                // ser till så att inga fel upstod när den hämta data
                if (QuizResponse.StatusCode == HttpStatusCode.OK)
                {
                    Console.Clear();
                    //skapar frågan
                    q = JsonSerializer.Deserialize<List<Quiz>>(QuizResponse.Content).First();
                    // tar bort tecken som inte behövs i frågan
                    q.CleanAnswer();

                    q = ChoseNormalQuestion(q);
                }
                //körs endast om något fel upstod då man förfrågade information från API:n
                else
                {
                    System.Console.WriteLine("ERROR:API conection not found");
                    Console.ReadLine();
                }
            }

            // när använaren har valt fråga så körs detta 
            bool Result = NormalQuizHandler(q);
            

            if (Result)
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
            if (q.IsOutOfpoints())
            {
                GameIsGoing = false;
                System.Console.WriteLine("You have LOST the Normal Jeopardy Quiz!");
            }
            //kollar om spelaren har tillråklig med poäng för att vinna
            if (q.HasWonGame())
            {
                GameIsGoing = false;
                System.Console.WriteLine("Congratulations! You have WON the Normal Jeopardy Quiz!");
            }
            q.WritePoints();
            FullStop();
            q = null;
        }

    }
    //svår frågesport
    public void StartHardQuiz()
    {
        HardQuiz HardQ = new HardQuiz();
        //kör spelet medans spelaren har poäng kvar.
        while (!HardQ.IsOutOfpoints() && !HardQ.HasWonGame())
        {
            
            HardQ = null;
            while (HardQ == null)
            {

                //hämtar en respons från api 
                RestResponse QuizResponse = QuizClient.GetAsync(QuizRequest).Result;
                
                //sparar värderna från responsen i variabeln
                try
                {
                    HardQ = JsonSerializer.Deserialize<List<HardQuiz>>(QuizResponse.Content).First();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Something wrong, getting new question");
                }
            }
            Console.Clear();
            HardQ.CleanAnswer();
            //skriver ut meny
            HardQ.WritePoints();
            HardQ.WriteQuestion();
            bool IsCorrect = false;
            //spelaren svarar
            string PlayerGuess = Console.ReadLine();
            // svaret gämförs och sparas i en boolean
            IsCorrect = HardQ.Answer.Equals(PlayerGuess, StringComparison.OrdinalIgnoreCase);
            if (IsCorrect)
            {
                System.Console.WriteLine($"CORRECT! You gained {HardQ.Difficulty / 2} points!");
                HardQ.AddPoints();
            }
            else
            {
                System.Console.WriteLine($"WRONG! You lost {HardQ.Difficulty / 2} points");
                System.Console.WriteLine($"Correct answer: {HardQ.Answer}");
                HardQ.RemovePoints();
            }
            if (HardQ.IsOutOfpoints())
            {
                GameIsGoing = false;
                System.Console.WriteLine("You have LOST the Hard Jeopardy Quiz!");
            }
            if (HardQ.HasWonGame())
            {
                GameIsGoing = false;
                System.Console.WriteLine("Congratulations! You have WON the Hard Jeopardy Quiz!");
            }
            Console.ReadLine();

        }
    }
    //ger valet av om spelaren vill ha fågan, om de vill ha det retunaras null så kommer en ny fråga
    private Quiz ChoseNormalQuestion(Quiz q)
    {
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
                    System.Console.WriteLine("No choice was made, make sure to only write a number in the list. Press enter to continue");
                    Console.ReadLine();
                    MadeChoice = false;
                    break;
            }
        }
        return q;
    }
    private bool NormalQuizHandler(Quiz q)
    {
        //man kan inte ge kontroll till en "if" sats i en switch så för att få en bool behövs denna 
        bool Result = false;
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
                    return Result;
                case 2:
                    HiddenAnswer.RevealLetter(q);
                    HasAnswered = false;
                    break;
                case 3:
                    //blir san så programet fortsätter och ger tillbaka falskt
                    HasAnswered = true;
                    System.Console.WriteLine($"{q.Answer}");
                    Console.ReadLine();
                    break;
                default:
                    System.Console.WriteLine("No choice was made, make sure to only write a number. Press enter to continue");
                    Console.ReadLine();
                    HasAnswered = false;
                    break;
            }
            // ser om spelaren har använt alla sina ledtrådar 
            // eller om de har slut på poäng
            if (HiddenAnswer.CluesLeft <= 0 || q.IsOutOfpoints())
            {
                System.Console.WriteLine("you have ran out of points or clues.");
                HasAnswered = true;
            }

        }
        //om spelaren valde att får svaret eller poången är slut fortsäter programet och retunerar falsk på boolen.
        return false;
    }
    private void DrawStartMenuText()
    {
        System.Console.WriteLine("Welcome to the Jeopardy Quiz!");
        System.Console.WriteLine("Chose game type: ");
        System.Console.WriteLine("1. Normal mode. ");
        System.Console.WriteLine("You get the topic and difficulty of a question and can choose to answer it or to get a diffrent question.");
        System.Console.WriteLine("After choosing the question you can spend 50 points to unlock one letter from the answer.");
        System.Console.WriteLine("You start with 1000 points, you gain and lose points equal to the difficulty based on if you answer correctly. ");
        System.Console.WriteLine("You win at 2300 points ");
        System.Console.WriteLine("2. Hard mode. ");
        System.Console.WriteLine("For those who are Jeopardy kings. You get random questions and no hints. You start with 1000 points.");
        System.Console.WriteLine("However you gain or lose only half the points of the difficulty.");
        System.Console.WriteLine("You win at 2000 points ");
    }
    private void FullStop()
    {
        System.Console.WriteLine("Press enter to continue");
        Console.ReadLine();
    }
}
