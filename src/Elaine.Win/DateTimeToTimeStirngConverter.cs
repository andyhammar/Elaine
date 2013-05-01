using System;
using Windows.UI.Xaml.Data;

namespace Elaine.Win
{
    public sealed class DateTimeToTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return DateTimeConverter.ToTimeString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateTimeConverter.FromTimeString(value);
        }
    }

    public class DateTimeConverter
    {
        public static string ToTimeString(object value)
        {
            if (!(value is DateTime)) return null;
            var dateTime = (DateTime)value;

            return dateTime.ToString("ddd d MMM HH:mm");
        }

        public static object FromTimeString(object value)
        {
            return null;
        }
    }
}