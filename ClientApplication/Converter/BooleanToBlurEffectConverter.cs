using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Effects;
using ClientApplication.ViewModels;

namespace ClientApplication.Converter;

public class BooleanToBlurEffectConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool booleanValue = (bool)value;
        if (booleanValue)
        {
            return new BlurEffect() { Radius = 5 };
        }
        return null; // Kein BlurEffect
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}