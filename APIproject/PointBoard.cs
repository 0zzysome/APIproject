using System;



public class PointBoard
{
    public int TotalPoints { get; set; }= 1000;
    // virtual int that changes points here
    public virtual void AddPoints()
    {
        TotalPoints += 100;
    }
}
