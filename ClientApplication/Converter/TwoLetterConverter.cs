using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ClientApplication.Converter;

public class TwoLetterConverter: IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string text)
        {
            var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var result = string.Join("", words.Select(w => w[..2]));
            return result;
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}