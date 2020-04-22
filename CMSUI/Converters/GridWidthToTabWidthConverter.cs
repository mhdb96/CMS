using System;
using System.Globalization;
using System.Windows.Data;

namespace CMSUI.Converters
{
    public class GridWidthToTabWidthConverter : IValueConverter
    {
        public static GridWidthToTabWidthConverter ins = new GridWidthToTabWidthConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Console.WriteLine(value);
            double width = (double)value / 6.9;
            int e = (int)width;
            return e;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
