using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.Requesters;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CMSUI.CreateForms
{
    /// <summary>
    /// Interaction logic for CreateActiveTermWindow.xaml
    /// </summary>
    /// 

    public partial class CreateActiveTermWindow
    {
        IActiveTermRequester CallingWindow;
        List<YearModel> Years;

        List<TermModel> Terms;
        YearModel model;

        //List<StackPanel> spList = new List<StackPanel>();  // DynamicValid  için

        bool update;
        ActiveTermModel activeTerm = new ActiveTermModel();

        public CreateActiveTermWindow(IActiveTermRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();

            update = false;
            createActiveTermBtn.Content = "Create";//gerek var mı?
        }

        public CreateActiveTermWindow(IActiveTermRequester caller, ActiveTermModel model)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();

            update = true;
            createActiveTermBtn.Content = "Update";
            titleText.Text = "Update The Active Term";
            title.Title = "UPDATE ACTIVE TERM";
            activeTerm = model;


            // TODO Foreach'sız nasıl olurdu
            foreach (var year in Years)
            {
                if (year.Id == activeTerm.Year.Id)
                {
                    yearsCombobox.SelectedItem = year;
                }
            }
            termsCombobox.SelectedItem = activeTerm.Term;

            //


        }

        private void LoadListsData()
        {
            Years = GlobalConfig.Connection.GetYear_ALL();
            yearsCombobox.ItemsSource = Years;

            //Terms = GlobalConfig.Connection.GetTerm_ALL();
            //termsCombobox.ItemsSource = Terms;
        }

        private void CreateActiveTermBtn_Click(object sender, RoutedEventArgs e)
        {

            if (ValidForm())
            {
                activeTerm.Year = (YearModel)yearsCombobox.SelectedItem;
                activeTerm.Term = (TermModel)termsCombobox.SelectedItem;

                if (!update)
                {
                    GlobalConfig.Connection.CreateActiveTerm(activeTerm);
                }
                else
                {
                    GlobalConfig.Connection.UpdateActiveTerms(activeTerm);
                }
                CallingWindow.ActiveTermComplete(activeTerm);
                this.Close();


            }

        }

        private bool ValidForm()
        {

            if (yearsCombobox.SelectedItem == null || termsCombobox.SelectedItem == null)
            {
                if (yearsCombobox.SelectedItem == null)
                {
                    errorYear.Visibility = Visibility.Visible;
                }
                if (termsCombobox.SelectedItem == null)
                {
                    errorTerm.Visibility = Visibility.Visible;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        //private bool DynamicValidForm()//DİNAMİK BİR ŞEKİLDE HATA VEREN KOD (KULLANMIYORUZ)
        //{
        //    int i = 0;
        //    List<int> rows = new List<int>();
        //    List<String> text = new List<String>();

        //    StackPanel sp;

        //    TextBlock tb;
        //    PackIconMaterial icon;
        //    foreach (UIElement Child in myGrid.Children)
        //    {
        //        if(Child is ComboBox)
        //        {
        //            if (((ComboBox)Child).SelectedItem == null)
        //            {
        //                rows.Add(Grid.GetRow(Child));
        //                text.Add(((ComboBox)Child).Name);
        //                i++;

        //            }
        //        }
        //    }
        //    foreach (var item in spList)
        //    {
        //        item.Children.Clear();
        //    }

        //    if (i > 0) { 
        //        i = 0;
        //    foreach (var row in rows)
        //    {
        //        sp = new StackPanel();
        //        sp.Name = "errorSp";
        //        sp.Orientation = Orientation.Horizontal;

        //        spList.Add(sp);

        //        tb = new TextBlock();
        //        tb.Text = "You need to choose a " + text[i]  ;
        //        tb.FontSize=20;
        //        tb.VerticalAlignment = VerticalAlignment.Center;

        //        icon = new PackIconMaterial();
        //        icon.Kind = PackIconMaterialKind.AlertCircle;
        //        icon.VerticalAlignment = VerticalAlignment.Center;
        //        icon.Width = 20;
        //        icon.Height = 20;
        //        icon.Margin = new Thickness(5);
        //        icon.Foreground = new SolidColorBrush(Colors.Red);

        //        sp.Children.Add(icon);
        //        sp.Children.Add(tb);
        //        Grid.SetRow(sp, row);
        //        Grid.SetColumn(sp,3);
        //        Grid.SetColumnSpan(sp, 3);
        //        myGrid.Children.Add(sp);
        //            i++;
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }

        //}

        private void CancelActiveTermBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void YearsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (yearsCombobox.SelectedItem == null)
            {
                termsCombobox.IsHitTestVisible = false;
                errorYear.Visibility = Visibility.Visible;
            }
            else
            {
                model = (YearModel)yearsCombobox.SelectedItem;
                Terms = GlobalConfig.Connection.GetTerm_ValidByYearId(model.Id);

                if (model.Id == activeTerm.Year.Id)
                {
                    Terms.Add(activeTerm.Term);
                }

                termsCombobox.ItemsSource = Terms;
                if (!Terms.Any())
                {
                    termsCombobox.IsHitTestVisible = false;
                }
                else
                {
                    termsCombobox.IsHitTestVisible = true;
                }

                errorYear.Visibility = Visibility.Hidden;
            }
        }

        private void TermsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (termsCombobox.SelectedItem == null)
            {
                errorTerm.Visibility = Visibility.Visible;
            }
            else
            {
                errorTerm.Visibility = Visibility.Hidden;
            }
        }
    }
}
