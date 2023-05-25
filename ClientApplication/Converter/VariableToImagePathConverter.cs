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
            GameType.TextGame => "../Assets/TextGame/logo.jpg",
            GameType.BricketBraker => "../Assets/BricketBraker/logo.jpg",
            GameType.ColorEcho => "../Assets/ColorEcho/logo.jpg",
            GameType.MemoMaster => "../Assets/MemoMaster/logo.jpg",
            GameType.PathPilot => "../Assets/PathPilot/logo.jpg",
            _ => "../Assets/empty/logo.jpg"
        };

        return imagePath;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}