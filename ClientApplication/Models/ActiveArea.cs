namespace ClientApplication.Models;

public class ActiveArea
{
    public int Y1 { get; set; }
    public int Y2 { get; set; }

    public int Height
    {
        get
        {
            return Y2 - Y1 + 4;
        }
    }
}