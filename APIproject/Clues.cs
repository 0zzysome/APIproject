using System.Text.Json.Serialization;

public class Clues : Quiz
{

    // ska få tag i svaret och skapa sin egna hint som har alla bokstäver utkrysna
    // spelaren kan sen välja att reveala en boktav 
    
    public string HiddenHint { get; set; } = "";
    public int CluesLeft { get; set; }

    public Clues(Quiz q)
    {
        
        CluesLeft = q.Answer.Length;
        for (var i = 0; i < q.Answer.Length; i++)
        {
            if (q.Answer[i] != ' ')
            {
                HiddenHint += "-";
            }
            else
            {
                HiddenHint += ' ';   
            }
         
        }
        
    }
    public void RevealLetter(Quiz q)
    {
        char[] ca = HiddenHint.ToArray();
        int p = generator.Next(q.Answer.Length);
        ca[p] = q.Answer[p];
        HiddenHint = ca.ToString();
        CluesLeft -= CluesLeft;    
    }
    public void WriteHidenAnswer()
    {
        System.Console.WriteLine($"Hint of answer: {HiddenHint}");
    }
    
}

