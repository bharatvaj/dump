using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MainPage : Page
    {


        ObservableCollection<Transaction> expense = new ObservableCollection<Transaction>();
        ObservableCollection<Transaction> income = new ObservableCollection<Transaction>();

        public MainPage()
        {
            this.InitializeComponent();
            ExpenseView.ItemsSource = expense;
            IncomeView.ItemsSource = income;
            Loaded += MainPage_Loaded;
            SizeChanged += MainPage_SizeChanged;
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //SideBar.InitializeDrawerLayout();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SideBar.InitializeDrawerLayout();
            AddExpense(10000, 'f', new DateTime(1997, 05, 02));
            AddExpense(10420, 'f', new DateTime(1997, 05, 02));
            AddExpense(2000, 't', new DateTime(1997, 05, 02));
            AddExpense(50000, 't', new DateTime(1997, 05, 02));
            AddExpense(1750, 'p', new DateTime(1997, 05, 02));
            AddExpense(100500, 's', new DateTime(1997, 05, 02));
            AddExpense(10450, 'b', new DateTime(1997, 05, 02));
            AddExpense(107870, 'a', new DateTime(1997, 05, 02));
            AddExpense(1045, 't', new DateTime(1997, 05, 02));
            AddExpense(100500, 's', new DateTime(1997, 05, 02));
            AddExpense(10450, 't', new DateTime(1997, 05, 02));
            AddExpense(107870, 'a', new DateTime(1997, 05, 02));
            AddExpense(1045, 'c', new DateTime(1997, 05, 02));
            AddExpense(100500, 's', new DateTime(1997, 05, 02));
            AddExpense(10450, 't', new DateTime(1997, 05, 02));
            AddExpense(107870, 'c', new DateTime(1997, 05, 02));
            AddExpense(1045, 't', new DateTime(1997, 05, 02));




            AddIncome(10000, 'f', new DateTime(1997, 05, 02));
            AddIncome(10420, 'f', new DateTime(1997, 05, 02));
            AddIncome(2000, 't', new DateTime(1997, 05, 02));
            AddIncome(50000, 't', new DateTime(1997, 05, 02));
            AddIncome(1750, 'p', new DateTime(1997, 05, 02));
            AddIncome(100500, 's', new DateTime(1997, 05, 02));
            AddIncome(10450, 'b', new DateTime(1997, 05, 02));
            AddIncome(107870, 'a', new DateTime(1997, 05, 02));
            AddIncome(1045, 't', new DateTime(1997, 05, 02));
            AddIncome(100500, 's', new DateTime(1997, 05, 02));
            AddIncome(10450, 't', new DateTime(1997, 05, 02));
            AddIncome(107870, 'a', new DateTime(1997, 05, 02));
            AddIncome(1045, 'c', new DateTime(1997, 05, 02));
            AddIncome(100500, 's', new DateTime(1997, 05, 02));
            AddIncome(10450, 't', new DateTime(1997, 05, 02));
            AddIncome(107870, 'c', new DateTime(1997, 05, 02));
            AddIncome(1045, 't', new DateTime(1997, 05, 02));
        }

        public void AddExpense(int amount, char type, DateTime date)
        {
            //expense.Add(new Transaction(amount,"", type, date,'e'));
        }


        public void AddIncome(int amount, char type, DateTime date)
        {
            //income.Add(new Transaction(amount,"", type, date,'i'));
        }

        private void OpenDrawer(object sender, RoutedEventArgs e)
        {
            if (SideBar.IsDrawerOpen == false)
            {
                SideBar.OpenDrawer();
            }
            else
            {
                SideBar.CloseDrawer();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        
        private void StackPanel_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.Y >= 10)
            {

            }
        }

        private void OpenTans(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openTransaction"]).Begin();
        }

        private void ClodeTrans(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openTransactionRev"]).Begin();
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            //expense.Add(new Transaction(500, 't', DateTime.Today));
            //((Storyboard)Resources["openTransactionRev"]).Begin();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openmenu"]).Begin();
            MenuFrame.Navigate(typeof(SettingsPage));
        }

        private void CloseSettings(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["closemenu"]).Begin();
            //MenuFrame.Content = null;
        }

        private void OpenCalendar(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openmenu"]).Begin();
        }

        private void CloseCalendar(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["closemenu"]).Begin();
        }
    }
}
