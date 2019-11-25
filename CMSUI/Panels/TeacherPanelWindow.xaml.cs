using CMSLibrary.Models;
using CMSUI.Requesters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CMSUI.Panels
{
    /// <summary>
    /// Interaction logic for TeacherPanelWindow.xaml
    /// </summary>
    /// 
    public partial class TeacherPanelWindow
    {
        ITeacherPanelRequester CallingWindow;        
        public TeacherPanelWindow(ITeacherPanelRequester caller)
        {
            //Teacher = new TeacherModel
            //{
            //    FirstName = "Muahammed",
            //    LastName = "Bedavi",
            //    Id = 7,
            //    RegNo = 1,
            //    User = new UserModel
            //    {
            //        Id = 5,
            //    }

            //};
            InitializeComponent();
            CallingWindow = caller;
            Teacher = CallingWindow.GetTeacherInfo();
        }
        public static readonly DependencyProperty TeacherProperty =
        DependencyProperty.Register("Teacher", typeof(TeacherModel), typeof(TeacherPanelWindow), new FrameworkPropertyMetadata(null));

        private TeacherModel Teacher
        {
            get { return (TeacherModel)GetValue(TeacherProperty); }
            set { SetValue(TeacherProperty, value); }
        }        
    }
}
