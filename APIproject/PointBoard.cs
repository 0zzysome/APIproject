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
}
