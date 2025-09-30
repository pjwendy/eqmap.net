using EQLogs.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EQLogs
{
    public class PacketDirectionToColorConverter : IValueConverter
    {
        public static readonly PacketDirectionToColorConverter Instance = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PacketDirection direction)
            {
                return direction switch
                {
                    PacketDirection.ClientToServer => Colors.Orange,
                    PacketDirection.ServerToClient => Colors.LightBlue,
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectToBooleanConverter : IValueConverter
    {
        public static readonly ObjectToBooleanConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverterReverse : IValueConverter
    {
        public static readonly BooleanToVisibilityConverterReverse DefaultReverse = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}