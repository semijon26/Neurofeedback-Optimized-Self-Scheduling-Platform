using System;
using System.Windows;
using System.Windows.Media;
using Shared;

namespace ClientApplication.Models;

public class Circle
{
    public GameType? FirstActiveGameType { get; set; }
    public GameType? SecondActiveGameType { get; set; }
    public GameType? ThirdActiveGameType { get; set; }
    public GameType? FourthActiveGameType { get; set; }
    public string Label { get; set; }

    public Thickness ButtonMargin { get; set; }

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
   
        rgb[0] = (byte)random.Next(128, 256); // Rot
        rgb[1] = (byte)random.Next(128, 256); // Grün
        rgb[2] = (byte)random.Next(128, 200); // Blau
      
        return new SolidColorBrush(Color.FromRgb(rgb[0], rgb[1], rgb[2]));
    }

}