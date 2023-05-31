using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Shared;

namespace ClientApplication.Converter;

public class ColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ClientObject clientObject)
        {
            return new SolidColorBrush(Color.FromArgb(clientObject.A, clientObject.R, clientObject.G, clientObject.B));
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}