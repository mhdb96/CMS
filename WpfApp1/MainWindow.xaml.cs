using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using CMSLibrary;
using CMSLibrary.Models;

namespace WpfApp1
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Evaluate ev = new Evaluate();
        public MainWindow()
        {
            InitializeComponent();
            GlobalConfig.Connection.CheckConniction();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.Connection.CheckConniction();
            DataGridListe.Items.Clear();
            List<StudentAnswersModel> myInfo = ev.GetRightAnswers();
            foreach (StudentAnswersModel i in myInfo)
            {
                DataGridListe.Items.Add(new { ad = i.Student.FirstName, soyad = i.Student.LastName, no = i.Student.RegNo, kitapcik = i.Group.Name, cevaplar = i.AnswersList, dogru = i.CorrectAnswersCount });
            }
        }
        public string GetFilePath()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = ".txt Dosyası |*.txt";
            file.ShowDialog();
            string path = file.FileName;
            return path;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string listPath = GetFilePath();
            List<StudentAnswersModel> myInfo = ev.GetStudentsAnswers(listPath);
            foreach (StudentAnswersModel i in myInfo)
            {
                DataGridListe.Items.Add(new { ad = i.Student.FirstName, soyad = i.Student.LastName, no = i.Student.RegNo, kitapcik = i.Group.Name, cevaplar = i.AnswersList });
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string answersPath = GetFilePath();
            List<AnswerKeyModel> myAnswers = ev.GetAnswersKeys(answersPath);
            foreach (AnswerKeyModel a in myAnswers)
            {
                CevapAnahtari.Items.Add(new { KitapcikTuru = a.Group.Name, DogruCevaplar = a.AnswersList });
            }
        }
    }
}