using System;
using System.Globalization;
using System.Windows.Data;

namespace CMSUI.Converters
{
    class GridWidthToTabWidthConverterTeacher : IValueConverter
    {
        public static GridWidthToTabWidthConverterTeacher ins = new GridWidthToTabWidthConverterTeacher();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Console.WriteLine(value);
            double width = (double)value / 2.2;
            int e = (int)width;
            return e;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
