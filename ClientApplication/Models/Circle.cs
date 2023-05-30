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

    public ClientObject Client { get; set; }

}