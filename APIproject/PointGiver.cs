using System;



public class PointGiver
{
    public static int TotalPoints { get; set; }= 1000;
    
    public virtual void AddPoints()
    {
        TotalPoints += 100;
    }
    public virtual void RemovePoints()
    {
        TotalPoints += 100;
    }
    public void WritePoints()
    {
        System.Console.WriteLine($" Points left: {PointGiver.TotalPoints}");
        System.Console.WriteLine($"-------------------------------------- ");
    }

    public bool IsOutOfpoints()
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
    //kan göra virtual för de olika subklaserna
    public virtual bool HasWonGame()
    {
        //normal quiz
        if(PointGiver.TotalPoints>=2300)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
