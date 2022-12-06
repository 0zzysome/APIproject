using System.Text.Json.Serialization;

public class Clues : Quiz
{

    // ska få tag i svaret och skapa sin egna hint som har alla bokstäver utkrysna
    // spelaren kan sen välja att reveala en boktav 
    
    public string HiddenHint { get; set; } = "";
    public int CluesLeft { get; set; } = 0;

    public Clues(Quiz q)
    {
        
        
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
    public void RevealLetter(Quiz q, Clues c)
    {
        bool NotRevealed = true;
        while(NotRevealed)
        {
            char[] ca = c.HiddenHint.ToArray();
            char[] AnswerChar = q.Answer.ToArray();
            int p = generator.Next(0, q.Answer.Length);
            if(ca[p]=='-')
            {
                ca[p] = AnswerChar[p];
                c.HiddenHint = string.Join("", ca);
                c.CluesLeft -= 1; 
                c.RemovePoints();
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

