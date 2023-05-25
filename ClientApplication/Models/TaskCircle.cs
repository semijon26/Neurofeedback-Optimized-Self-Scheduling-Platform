using System;
using System.Windows.Media;

namespace ClientApplication.Models;

public class TaskCircle
{
    public string Label { get; set; }

    public SolidColorBrush RandomColor
    {
        get
        {
            return GetRandomBrush();
        }
    }
    private static SolidColorBrush GetRandomBrush()
    {
        Random random = new Random();
        // Zufällige helle Farbe generieren
        byte[] rgb = new byte[3];
   
        rgb[0] = (byte)random.Next(0, 0); // Rot
        rgb[1] = (byte)random.Next(128, 256); // Grün
        rgb[2] = (byte)random.Next(0, 0); // Blau
      
        return new SolidColorBrush(Color.FromRgb(rgb[0], rgb[1], rgb[2]));
    }

}