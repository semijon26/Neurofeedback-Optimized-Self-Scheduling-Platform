using System;
using System.Globalization;
using System.Windows.Data;
using Shared;

namespace ClientApplication.Converter;

public class VariableToImagePathConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        // Hier kannst du die Logik zum Zuordnen der Variable zum Bildpfad implementieren
        // Du kannst eine Switch-Anweisung, eine bedingte Anweisung oder eine andere Logik verwenden

        if (value == null) return null;
        var variableValue = (GameType)value; // Annahme: Die Variable ist vom Typ int

        var imagePath = variableValue switch
        {
            GameType.TextGame => "../Assets/tornado_normal.png",
            GameType.BricketBraker => "../Assets/brick_normal.png",
            GameType.ColorEcho => "../Assets/brain_normal.png",
            GameType.MemoMaster => "../Assets/back_normal.png",
            GameType.RoadRacer => "../Assets/car_normal.png",
            GameType.BackTrack => "../Assets/back_normal.png",
            _ => "../Assets/empty/logo.jpg"
        };

        return imagePath;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}