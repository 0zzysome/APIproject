using System;



public class PointGiver
{
    public static int totalPoints { get; set; }= 1000;
    
    public virtual void AddPoints()
    {
        totalPoints += 100;
    }
    public virtual void RemovePoints()
    {
        totalPoints += 100;
    }
    public void WritePoints()
    {
        System.Console.WriteLine($" Points left: {PointGiver.totalPoints}");
        System.Console.WriteLine($"-------------------------------------- ");
    }

    public bool IsOutOfpoints()
    {
        if(PointGiver.totalPoints<= 0)
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
        if(PointGiver.totalPoints>=2300)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
