using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientApplication.Converter;

public class LineColorConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isVisible = (bool)value;
        if (isVisible)
        {
            return Color.FromRgb(0,0,0);
        }
        return Color.FromRgb(r:240, g:240, b:240);
        
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}