using System.Text.Json.Serialization;

public class Clues : Quiz
{

    // ska få tag i svaret och skapa sin egna hint som har alla bokstäver utkrysna
    // spelaren kan sen välja att visa en boktav i taget
    public string HiddenHint { get; set; } = "";
    public int CluesLeft { get; set; } = 0;
    public Clues(Quiz q)
    {
        
        //konverterar svaret till ett hemligt svar
        for (var i = 0; i < q.Answer.Length; i++)
        {
            
            
            if (q.Answer[i] != ' ')
            {
                HiddenHint += "-";
                CluesLeft += 1;
            }
            //gör så alla mellanslag inte räknas som gömda bokstäver
            else
            {
                HiddenHint += ' ';   
            }
            
            
         
        }
        
    }
    public void RevealLetter(Quiz q)
    {
        bool NotRevealed = true;
        //körs tills en ny slupad bokstav har visats
        while(NotRevealed)
        {
            char[] ca = HiddenHint.ToArray();
            char[] AnswerChar = q.Answer.ToArray();
            int p = Generator.Next(0, q.Answer.Length);
            if(ca[p]=='-')
            {
                ca[p] = AnswerChar[p];
                HiddenHint = string.Join("", ca);
                CluesLeft -= 1; 
                RemovePoints();
                NotRevealed = false;
            }
        }
        
        
    }
    public void WriteHidenAnswer()
    {
        System.Console.WriteLine($"Hints Left: {CluesLeft}");
        System.Console.WriteLine($"Hint of answer: {HiddenHint}");
    }
    
    public override void RemovePoints()
    {
        TotalPoints -= 50;
    }
    
}

