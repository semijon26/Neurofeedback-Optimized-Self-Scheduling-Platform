using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Shared;

namespace ClientApplication.Converter;

public class MacroTaskColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ClientObject worker = (ClientObject)value;
        if (worker == null)
        {
            return Color.FromRgb(r:0, g:0, b:0);
        }
        return Color.FromRgb(r:255, g:255, b:0);
        
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}