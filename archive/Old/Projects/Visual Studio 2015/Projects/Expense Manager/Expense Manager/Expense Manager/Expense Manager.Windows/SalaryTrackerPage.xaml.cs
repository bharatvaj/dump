using SQLiteWinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Expense_Manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SalaryTrackerPage : Page
    {

        bool IS_SALARY_ENTERED = false;
        bool IS_NAME_ENTERED = false;
        private bool IS_IN_UPDATE_MODE = false;
        App app = (App)Application.Current;

        ObservableCollection<Transaction> salary = new ObservableCollection<Transaction>();

        IList<Transaction> selectedItems;
        
        public SalaryTrackerPage()
        {
            this.InitializeComponent();
            listView.ItemsSource = salary;
            LoadSalaryData();
        }

        private async void LoadSalaryData()
        {
            

            AddSalary(4500, "Mr. Employee One", DateTime.Now, null);
            AddSalary(5550, "Mr. Employee Two", DateTime.Now, null);
            AddSalary(7500, "Mr. Employee Three", DateTime.Now, null);
            AddSalary(4500, "Mr. Employee One", DateTime.Now, null);
            AddSalary(5550, "Mr. Employee Two", DateTime.Now, null);
            AddSalary(7500, "Mr. Employee Three", DateTime.Now, null);
            AddSalary(4500, "Mr. Employee One", DateTime.Now, null);
            AddSalary(5550, "Mr. Employee Two", DateTime.Now, null);
            AddSalary(7500, "Mr. Employee Three", DateTime.Now, null);
            AddSalary(4500, "Mr. Employee One", DateTime.Now, null);
            AddSalary(5550, "Mr. Employee Two", DateTime.Now, null);
            AddSalary(7500, "Mr. Employee Three", DateTime.Now, null);
            AddSalary(4500, "Mr. Employee One", DateTime.Now, null);
            AddSalary(5550, "Mr. Employee Two", DateTime.Now, null);
            AddSalary(7500, "Mr. Employee Three", DateTime.Now, null);
            AddSalary(4500, "Mr. Employee One", DateTime.Now, null);
            AddSalary(5550, "Mr. Employee Two", DateTime.Now, null);
            AddSalary(7500, "Mr. Employee Three", DateTime.Now, null);
            AddSalary(4500, "Mr. Employee One", DateTime.Now, null);
            AddSalary(5550, "Mr. Employee Two", DateTime.Now, null);
            AddSalary(7500, "Mr. Employee Three", DateTime.Now, null);
            AddSalary(4500, "Mr. Employee One", DateTime.Now, null);
            AddSalary(5550, "Mr. Employee Two", DateTime.Now, null);
            AddSalary(7500, "Mr. Employee Three", DateTime.Now, null);
        }


        private void AddSalary(int amt, string recv_name, DateTime date,ImageBrush recv_img)
        {
            salary.Add(new Transaction(amt,"Salary",'s',date,recv_name,recv_img,String.Empty));
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenControlMenu(listView.SelectedItems.Count > 0?true:false);
        }


        private void OpenControlMenu(bool open)
        {
            if(open)((Storyboard)Resources["controlGridOpen"]).Begin();
            else ((Storyboard)Resources["controlGridOpenRev"]).Begin();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            salary.RemoveAll<Transaction>(x => listView.SelectedItems.Contains(x));
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            IS_IN_UPDATE_MODE = true;
            selectedItems = (IList<Transaction>)listView.SelectedItems;
            //var b = (ListViewItem)e.OriginalSource;
            ((Storyboard)Resources["openAdd"]).Begin();
            salaryTextBox.Text = selectedItems[0].AMOUNT.ToString();
            nameTextBox.Text = selectedItems[0].RECIEVER_PERS;
            datePicker.Date = selectedItems[0].DATE_OBJ;



        }
        

        private void OpenAdd(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openAdd"]).Begin();
        }

        private void CloseAdd(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openAddRev"]).Begin();
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            if (IS_IN_UPDATE_MODE)
            {
                foreach (var item in selectedItems)
                {
                    item.AMOUNT = Int32.Parse(salaryTextBox.Text);
                }
            }
            else
            {
                AddSalary(Int32.Parse(salaryTextBox.Text), nameTextBox.Text, datePicker.Date.Date, null);
            }
            ClearFields();
            toggleButton.IsChecked = false;
        }

        private void RectangleTapped(object sender, TappedRoutedEventArgs e)
        {
            toggleButton.IsChecked = false;
            filterToggle.IsChecked = false;
        }

        private void salaryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            IS_SALARY_ENTERED =  String.IsNullOrWhiteSpace(salaryTextBox.Text) ? false : true;
            CheckOKCondition();
        }

        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            IS_NAME_ENTERED = String.IsNullOrWhiteSpace(nameTextBox.Text) ? false : true;
            CheckOKCondition();
        }

        private void ClearFields()
        {
            salaryTextBox.Text = nameTextBox.Text = String.Empty;
            IS_NAME_ENTERED = IS_SALARY_ENTERED = false;
            okButton.IsEnabled = false;
        }

        private void CheckOKCondition()
        {
            okButton.IsEnabled = IS_NAME_ENTERED && IS_SALARY_ENTERED ? true : false;
            
        }

        private void SortChecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openSort"]).Begin();
        }
        

        private void SortUnchecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openSortRev"]).Begin();
        }
        private void SortButton(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Content.ToString())
            {
                case "Name":
                    break;
                case "Date":
                    break;
                case "Amount":
                    break;
                default:
                    break;
            }
        }
    }
}
