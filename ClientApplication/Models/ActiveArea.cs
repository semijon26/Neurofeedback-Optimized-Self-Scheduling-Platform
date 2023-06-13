namespace ClientApplication.Models;

public class ActiveArea
{
    public double Y1 { get; set; }
    public double Y2 { get; set; }

    public double Height
    {
        get
        {
            return Y2 - Y1 + 4;
        }
    }
}