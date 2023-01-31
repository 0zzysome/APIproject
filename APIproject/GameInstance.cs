using System;
using RestSharp;
using System.Text.Json;
using System.Net;

public class GameInstance
{
    RestClient quizClient = new("https://jservice.io/api/");

    RestRequest quizRequest = new("random");
    //boolean hanterar om spelet ska avlutas.
    private bool gameIsGoing = true;
    //hanterar om spelaren har gjort sit val av gamemode
    private bool hasGameType = false;
    public int gameType = 3;
    // startmeny
    public void ShowStartMenu()
    {
        while (!hasGameType)
        {

            Console.Clear();
            // lägg till: mindre kod 
            DrawStartMenuText();
            hasGameType = int.TryParse(Console.ReadLine(), out gameType);
            //om spelaren valde 1 eller 2 så ändras inte hasGameType och koden fortsätter.
            //btw jag vet att koden är dålig men den funkar
            if (gameType == 1)
            {
                
                System.Console.WriteLine("You chose: Normal mode");
                FullStop();
                hasGameType = true;

            }
            else if (gameType == 2)
            {
                
                System.Console.WriteLine("You chose: Hard mode");
                FullStop();
                hasGameType = true;

            }
            else
            {
                System.Console.WriteLine("Make sure to write a letter from the menu.");
                FullStop();
                hasGameType = false;
            }

        }
    }
    //normal frågesport
    public void StartNormalQuiz()
    {
        Quiz q = null;
        while (gameIsGoing)
        {
            while (q == null)
            {
                RestResponse quizResponse = quizClient.GetAsync(quizRequest).Result;
                // ser till så att inga fel upstod när den hämta data
                if (quizResponse.StatusCode == HttpStatusCode.OK)
                {
                    Console.Clear();
                    //skapar frågan
                    q = JsonSerializer.Deserialize<List<Quiz>>(quizResponse.Content).First();
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
            bool result = NormalQuizHandler(q);
            

            if (result)
            {
                System.Console.WriteLine($"CORRECT! You gained {q.difficulty} points!");
                q.AddPoints();
            }
            else
            {
                q.RemovePoints();
                System.Console.WriteLine($"WRONG! You lost {q.difficulty} points");
                System.Console.WriteLine($"Correct answer: {q.answer}");
            }
            //kollar om spelaren har poäng kvar och avlsutar while satsen om det har slut på poäng
            if (q.IsOutOfpoints())
            {
                gameIsGoing = false;
                System.Console.WriteLine("You have LOST the Normal Jeopardy Quiz!");
            }
            //kollar om spelaren har tillråklig med poäng för att vinna
            if (q.HasWonGame())
            {
                gameIsGoing = false;
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
        HardQuiz hardQ = new HardQuiz();
        //kör spelet medans spelaren har poäng kvar.
        while (!hardQ.IsOutOfpoints() && !hardQ.HasWonGame())
        {
            
            hardQ = null;
            while (hardQ == null)
            {

                //hämtar en respons från api 
                RestResponse quizResponse = quizClient.GetAsync(quizRequest).Result;
                
                //sparar värderna från responsen i variabeln
                try
                {
                    hardQ = JsonSerializer.Deserialize<List<HardQuiz>>(quizResponse.Content).First();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Something wrong, getting new question");
                }
            }
            Console.Clear();
            hardQ.CleanAnswer();
            //skriver ut meny
            hardQ.WritePoints();
            hardQ.WriteQuestion();
            bool isCorrect = false;
            //spelaren svarar
            string playerGuess = Console.ReadLine();
            // svaret gämförs och sparas i en boolean
            isCorrect = hardQ.answer.Equals(playerGuess, StringComparison.OrdinalIgnoreCase);
            if (isCorrect)
            {
                System.Console.WriteLine($"CORRECT! You gained {hardQ.difficulty / 2} points!");
                hardQ.AddPoints();
            }
            else
            {
                System.Console.WriteLine($"WRONG! You lost {hardQ.difficulty / 2} points");
                System.Console.WriteLine($"Correct answer: {hardQ.answer}");
                hardQ.RemovePoints();
            }
            if (hardQ.IsOutOfpoints())
            {
                gameIsGoing = false;
                System.Console.WriteLine("You have LOST the Hard Jeopardy Quiz!");
            }
            if (hardQ.HasWonGame())
            {
                gameIsGoing = false;
                System.Console.WriteLine("Congratulations! You have WON the Hard Jeopardy Quiz!");
            }
            Console.ReadLine();

        }
    }
    //ger valet av om spelaren vill ha fågan, om de vill ha det retunaras null så kommer en ny fråga
    private Quiz ChoseNormalQuestion(Quiz q)
    {
        bool madeChoice = false;
        int choice;
        //körs medans spelaren inte har valt
        while (!madeChoice)
        {
            Console.Clear();
            // ritar ut texten
            // Frågar om användaren vill ha den här frågan eller byta den
            q.WritePoints();
            q.Menu();
            //tar input och gör till en int 
            madeChoice = int.TryParse(Console.ReadLine(), out choice);
            switch (choice)
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
                    madeChoice = false;
                    break;
            }
        }
        return q;
    }
    private bool NormalQuizHandler(Quiz q)
    {
        //man kan inte ge kontroll till en "if" sats i en switch så för att få en bool behövs denna 
        bool result = false;
        bool hasAnswered = false;
        Clues hiddenAnswer = new Clues(q);
        while (!hasAnswered)
        {
            //skapar meny 
            Console.Clear();
            q.WritePoints();
            q.WriteQuestion();
            hiddenAnswer.WriteHidenAnswer();
            int i;
            //tar input och försöker göra den till en int och sparar den 
            hasAnswered = int.TryParse(Console.ReadLine(), out i);
            switch (i)
            {
                case 1:
                    System.Console.WriteLine("Write answer:");
                    //gör så att spelaren lan skriva in sin gissning 
                    string playerGuess = Console.ReadLine();
                    //gämför gissningen med svaret och ger ett reslutat
                    result = q.answer.Equals(playerGuess, StringComparison.OrdinalIgnoreCase);
                    return result;
                case 2:
                    hiddenAnswer.RevealLetter(q);
                    hasAnswered = false;
                    break;
                case 3:
                    //blir san så programet fortsätter och ger tillbaka falskt
                    hasAnswered = true;
                    System.Console.WriteLine($"{q.answer}");
                    Console.ReadLine();
                    break;
                default:
                    System.Console.WriteLine("No choice was made, make sure to only write a number. Press enter to continue");
                    Console.ReadLine();
                    hasAnswered = false;
                    break;
            }
            // ser om spelaren har använt alla sina ledtrådar 
            // eller om de har slut på poäng
            if (hiddenAnswer.cluesLeft <= 0 || q.IsOutOfpoints())
            {
                System.Console.WriteLine("you have ran out of points or clues.");
                hasAnswered = true;
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
