using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CMSUI.Converters
{
    class VisibilityValueConverter : IValueConverter
    {
        public static VisibilityValueConverter ins = new VisibilityValueConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = (Visibility)value;
            if (v == Visibility.Collapsed)
                v = Visibility.Visible;
            else v = Visibility.Collapsed;
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
