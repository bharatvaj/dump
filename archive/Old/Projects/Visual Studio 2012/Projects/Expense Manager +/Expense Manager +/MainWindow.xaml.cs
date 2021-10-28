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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.IO;
using System.Windows.Threading;
using Microsoft.Win32;
using Expense_Manager__;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;

namespace Expense_Manager__
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //double totalAmtEarned;
        //double totalAmtSpended;

        TranslateTransform pT = new TranslateTransform();
        double width;
        double height;
        #region Storyboard
        Storyboard searchBoxAnimation;
        Storyboard searchBoxAnimationR;
        List<Expenses> expenses;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            #region Storyboard
            searchBoxAnimation = ((Storyboard)GetWindow(this).TryFindResource("searchBoxAnimation"));
            searchBoxAnimationR = ((Storyboard)GetWindow(this).TryFindResource("searchBoxAnimation_Copy1"));
            #endregion

        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawerLayout.InitializeDrawerLayout();
        }
        #region PopupMover
        /// <summary>
        /// Moves the Popup with respect to Position of Mouse.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PopupMover(object sender, MouseEventArgs e)
        {
            
        }
        #endregion
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
        }

        private void expenseData_Loaded(object sender, RoutedEventArgs e)
        {
            expenses = new List<Expenses>();
            selectedExpense.RenderTransform = pT;
            this.expenseData.MouseEnter += expenseData_MouseEnter;
            this.expenseData.MouseMove += expenseData_MouseMove;
            this.expenseData.MouseLeave += expenseData_MouseLeave;

            try
            {
                using (StreamReader reader = new StreamReader("pack:application:,,,\\Expenses.txt"))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        expenses.Add(new Expenses(line));
                        MessageBox.Show("File is Read.,, Hooray!!!");
                    }
                }
            }
            catch //(Exception ex)
            {
                //MessageBox.Show("Cannot Read File" + ex);
                expenses.Add(new Expenses("Tour,123,1,23/07/2015"));
                expenses.Add(new Expenses("You Done It, 10000000,0,23/07/2015"));
                expenses.Add(new Expenses("Cigarette,150,1,23/07/2015"));
                expenses.Add(new Expenses("Drinks,250,1,23/07/2015"));
                //using (StreamReader reader = new StreamReader("C://Expenses.txt"))
                //while (true)
                //{
                //    string line = reader.ReadLine();
                //    if (line == null)
                //    {
                //        break;
                //    }
                //    expenses.Add(new Expenses(line));
                //}
            }
            var expenseData = (DataGrid)sender;
            expenseData.ItemsSource = expenses;
        }

        void expenseData_MouseLeave(object sender, MouseEventArgs e)
        {
            selectedExpense.Visibility = System.Windows.Visibility.Collapsed;
        }

        void expenseData_MouseEnter(object sender, MouseEventArgs e)
        {
            selectedExpense.Visibility = System.Windows.Visibility.Visible;
            width = expenseData.ActualWidth;
            height = expenseData.ActualHeight;
        }

        private void expenseData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var expenseData = sender as DataGrid;
            var selected = expenseData.SelectedItems;
            List<int> amount = new List<int>();
            int total = 0;
            foreach (var item in selected)
            {
                var expense = item as Expenses;
                amount.Add(expense.AMOUNT);
            }
            foreach (var Integer in amount)
            {
                total = total + Integer;
                selectedExpense.Content = total;
            }
        }
        int i;
        void expenseData_MouseMove(object sender, MouseEventArgs e)
        {
            selectedExpense.Visibility = System.Windows.Visibility.Visible;
            pieChart.ProgressRing = i++;
            pT.X = e.GetPosition(MainFragment).X - width;
            pT.Y = e.GetPosition(expenseData).Y - height;
        }
        //private void ExpenseTableToggle(object sender, RoutedEventArgs e)
        //{
        //    if(ExpenseTableMenu.Visibility == System.Windows.Visibility.Visible)
        //    ExpenseTableMenu.Visibility = System.Windows.Visibility.Collapsed;
        //    else
        //        ExpenseTableMenu.Visibility = System.Windows.Visibility.Visible;
        //}

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd.CheckFileExists)
            {
                var b = ofd.OpenFile();
                MessageBox.Show(b.Length.ToString());
            }
        }

        private void header_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            DrawerLayout.OpenDrawer();
        }
        #region SearchBox
        private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            searchBoxAnimation.Begin();
        }
        #endregion

        private void searchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            searchBoxAnimationR.Begin();
        }


        //new list "check" is created now and search filtered objects will added to it later
        List<Expenses> check = new List<Expenses>();

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((TextBox)sender).Text;
            // returns original list if search box is empty.
            if (txt == String.Empty)
            {
                expenseData.ItemsSource = expenses;
                return;
            }


            List<string> name = new List<string>();
            foreach (var item in expenses)
            {
                name.Add(((Expenses)item).NAME);
            }


            foreach (string item in name)
            {
                Regex regex = new Regex(txt);
                Match match = regex.Match(item);
                if (match.Success)
                {
                    foreach (var exp in expenses)
                    {
                        if (exp.NAME.Contains(txt))
                        {
                            check.Add(exp);
                        }
                    }
                }
            }
            expenseData.ItemsSource = null;
            expenseData.ItemsSource = check;
        }

        private bool Contains(string source, string toCheck, StringComparison comp)
        {
            int compareResult = String.Compare(source, toCheck, StringComparison.OrdinalIgnoreCase);
            return compareResult >= 0;
        }

        private void expenseData_CurrentCellChanged(object sender, EventArgs e)
        {
            //var currentCell = (DataGrid)sender;
            //edit.Content = currentCell.CurrentCell.Column.GetCellContent(1);
        }

        private void AddTransaction(object sender, RoutedEventArgs e)
        {

        }

        private void HideMoneyPopup(object sender, RoutedEventArgs e)
        {
            

        }

        private void ShowMoneyPopup(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            tb.Content = "Hide Money Indicator";
            selectedExpense.Visibility = System.Windows.Visibility.Visible;
        }

        private void UnShowMoneyPopup(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            tb.Content = "Show Money Indicator";
            selectedExpense.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
    }

