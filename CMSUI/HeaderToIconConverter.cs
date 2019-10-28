using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CMSUI
{
    public class HeaderToIconConverter : IValueConverter
    {
        public static HeaderToIconConverter ins = new HeaderToIconConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = (string)value;
            
            if(type == "Departments")
            {
                return "OfficeBuilding";
            }
            if(type == "Teachers")
            {
                return "Account";
            }
            if (type == "Terms")
            {
                return "Calendar";
            }
            if (type == "Courses")
            {
                return "Book";
            }
            if (type == "Assignment")
            {
                return "Briefcase";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
