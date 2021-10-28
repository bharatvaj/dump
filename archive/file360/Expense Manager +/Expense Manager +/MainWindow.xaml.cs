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

namespace Expense_Manager__
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        List<Expenses> list;
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            timeDisplay.Text = DateTime.Now.ToLongTimeString();
        }


        private void settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
        }

        private void expenseData_Loaded(object sender, RoutedEventArgs e)
        {
            var expenses = new List<Expenses>();

            try
            {
                using (StreamReader reader = new StreamReader("pack://application:,,,Expenses.txt"))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        expenses.Add(new Expenses(line));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Cannot Read File");
                expenses.Add(new Expenses("Tour,123"));
            }
            this.list = expenses;
            var expenseData = sender as DataGrid;
            expenseData.ItemsSource = expenses;
        }

        private void expenseData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var expenseData = sender as DataGrid;
            var total = expenseData.SelectedItems;
            var selected = expenseData.SelectedItems;
            List<int> amount = new List<int>();
            foreach (var item in selected)
            {
                var expense = item as Expenses;
                amount.Add(expense.AMOUNT);
            }
            totalExpense.Content = string.Join("+ ", amount);
            }
        private void ExpenseTableToggle(object sender, RoutedEventArgs e)
        {
            if(ExpenseTableMenu.Visibility == System.Windows.Visibility.Visible)
            ExpenseTableMenu.Visibility = System.Windows.Visibility.Collapsed;
            else
                ExpenseTableMenu.Visibility = System.Windows.Visibility.Visible;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

        }
        }
    }

