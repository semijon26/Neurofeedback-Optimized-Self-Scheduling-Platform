using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientApplication.Converter;

public class BooleanToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isTrue = (bool)value;
        if (isTrue)
        {
            return Brushes.Green;
        }
        else
        {
            return Brushes.Red;
        }
            
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}