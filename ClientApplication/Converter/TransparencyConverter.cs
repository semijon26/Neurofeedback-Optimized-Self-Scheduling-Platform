using System;
using System.Globalization;
using System.Windows.Data;
using Shared;

namespace ClientApplication.Converter
{
    public class TransparencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Überprüfe, ob das übergebene Objekt vom Typ ClientObject mit dem gespeicherten Objekt übereinstimmt
            if (value is ClientObject clientObject && clientObject.UniqueId == ClientObject.GetInstance().UniqueId)
            {
                return 0.3; // Vollständig sichtbar (undurchsichtig)
            }
            else
            {
                return 1.0; // Halbtransparent
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}