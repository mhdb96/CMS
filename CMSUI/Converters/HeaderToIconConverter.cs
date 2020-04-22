using System;
using System.Globalization;
using System.Windows.Data;

namespace CMSUI.Converters
{
    public class HeaderToIconConverter : IValueConverter
    {
        public static HeaderToIconConverter ins = new HeaderToIconConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = (string)value;

            if (type == "Departments")
            {
                return "OfficeBuilding";
            }
            if (type == "Teachers")
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
            if (type == "Assignments")
            {
                return "Briefcase";
            }
            if (type == "My Profile")
            {
                return "AccountCardDetails";
            }
            if (type == "My Courses")
            {
                return "BookMultiple";
            }
            if (type == "Exams")
            {
                return "BookMultiple";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
