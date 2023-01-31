using System.Text.Json.Serialization;

public class Clues : Quiz
{

    // ska få tag i svaret och skapa sin egna hint som har alla bokstäver utkrysna
    // spelaren kan sen välja att visa en boktav i taget
    public string hiddenHint { get; set; } = "";
    public int cluesLeft { get; set; } = 0;
    public Clues(Quiz q)
    {
        
        //konverterar svaret till ett hemligt svar
        for (var i = 0; i < q.answer.Length; i++)
        {
            
            
            if (q.answer[i] != ' ')
            {
                hiddenHint += "-";
                cluesLeft += 1;
            }
            //gör så alla mellanslag inte räknas som gömda bokstäver
            else
            {
                hiddenHint += ' ';   
            }
            
            
         
        }
        
    }
    public void RevealLetter(Quiz q)
    {
        bool notRevealed = true;
        //körs tills en ny slupad bokstav har visats
        while(notRevealed)
        {
            char[] ca = hiddenHint.ToArray();
            char[] answerChar = q.answer.ToArray();
            int p = generator.Next(0, q.answer.Length);
            if(ca[p]=='-')
            {
                ca[p] = answerChar[p];
                hiddenHint = string.Join("", ca);
                cluesLeft -= 1; 
                RemovePoints();
                notRevealed = false;
            }
        }
        
        
    }
    public void WriteHidenAnswer()
    {
        System.Console.WriteLine($"Hints Left: {cluesLeft}");
        System.Console.WriteLine($"Hint of answer: {hiddenHint}");
    }
    public override void RemovePoints()
    {
        totalPoints -= 50;
    }
    
}

