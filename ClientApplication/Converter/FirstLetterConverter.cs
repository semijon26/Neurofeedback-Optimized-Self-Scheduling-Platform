using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ClientApplication.Converter
{
    public class FirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                // Teile den Text in Wörter auf
                string[] words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Extrahiere den Anfangsbuchstaben jedes Worts
                string result = string.Join("", words.Select(w => w[0]));

                return result;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
