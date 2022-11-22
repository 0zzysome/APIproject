using System.Text.Json.Serialization;

public class Clues : Category
{

    public int CluesLeft { get; set; }

    public Clues()
    {
        CluesLeft = CluesAmount;
    }
    void ReduceClueCount()
    {
        CluesLeft -= CluesLeft;    
    }
}

