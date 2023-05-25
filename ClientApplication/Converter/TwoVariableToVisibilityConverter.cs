using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClientApplication.Converter;

public class TwoVariableToVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // Annahme: values[0] und values[1] enthalten die Werte der beiden Variablen

        // Überprüfe die Bedingungen und gib die Sichtbarkeit zurück
        if (values.Length >= 2 && values[0] != null && (bool)values[1] == false)
        {
            // Zeige die Ellipse an, wenn beide Variablen nicht null sind
            return Visibility.Visible;
        }

        // Verstecke die Ellipse in allen anderen Fällen
        return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
