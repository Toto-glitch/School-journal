using System;
using System.Globalization;
using System.Windows.Data;

namespace SchoolJournal.Service
{
    public class IntToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intVal && intVal >= 1 && intVal <= 5)
                return intVal - 1;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index && index >= 0 && index <= 4)
                return index + 1;
            return 1;
        }
    }
}