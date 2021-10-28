using SQLiteWinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
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

        ObservableCollection<TransType> expenseType = new ObservableCollection<TransType>();
        ObservableCollection<TransType> incomeType = new ObservableCollection<TransType>();

        StorageFolder installFolder = Package.Current.InstalledLocation; //gets the Expense Manager.Shared folder



        StorageFile expenseTypeFile; //holds the expense_categories.xml file
        StorageFile incomeTypeFile; //holds the income_categories.xml file


        StorageFile expenseFile; //holds the expense_data.xml file
        StorageFile incomeFile; //holds the income_data.xml file

        private bool IS_EXPENSE_TAB_SELECTED = false; //checks if expense tab is selected

        private bool IS_EXPENSE_TYPE_SELECTED = false; //used to enable or disable ok button
        private bool IS_INCOME_TYPE_SELECTED = false; //used to enable or disable ok button

        
        //VARIABLES FOR THE TYPES THAT HAVE BEEN SELECTED////////////////////////////////////
        private string SELECTED_INCOME_TYPE;
        private string SELECTED_EXPENSE_TYPE;

        private char SELECTED_INCOME_TYPE_IMAGE;
        private char SELECTED_EXPENSE_TYPE_IMAGE;

        private string SELECTED_EXPENSE_TYPE_DESCRIPTION;
        private string SELECTED_INCOME_TYPE_DESCRIPTION;
        ///////////////////////////////////////////////////////////////


        private Storyboard STORY_SYNC;
        private Storyboard STORY_REALIME;

        XmlDocument xmlDocE;
        XDocument xDocE;

        public MainPage()
        {
            this.InitializeComponent();

            ExpenseView.ItemsSource = expense;
            IncomeView.ItemsSource = income;
            Loaded += MainPage_Loaded;
            STORY_SYNC = (Storyboard)Resources["syncOn"];
            STORY_REALIME = (Storyboard)Resources["realTime"];
            SizeChanged += MainPage_SizeChanged;

            LoadDatabase();


            gridExpenseType.ItemsSource = expenseType;
            gridIncomeType.ItemsSource = incomeType;

            LoadTypes();



            LoadTransactions();

        }
        /// <summary>
        /// Use method to read and update type UI
        /// </summary>
        private async void LoadTypes()
        {
            XmlDocument xmlDocE = await XmlDocument.LoadFromFileAsync(await installFolder.GetFileAsync("expense_categories.xml"));
            XDocument xDocE = XDocument.Parse(xmlDocE.GetXml());
            XmlReader readerE = xDocE.CreateReader();

            XmlDocument xmlDocI = await XmlDocument.LoadFromFileAsync(await installFolder.GetFileAsync("income_categories.xml"));
            XDocument xDocI = XDocument.Parse(xmlDocI.GetXml());
            XmlReader readerI = xDocI.CreateReader();


            //load income types
            new Task( async() =>
            {
                string elementName = String.Empty;
                string typeName = String.Empty;
                char typeImage = 'm';
                string typeDescription = String.Empty;
                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    while (readerI.Read())
                    {
                        switch (readerI.NodeType)
                        {
                            case XmlNodeType.Element: // The node is an element.
                                elementName = readerI.Name;
                                break;

                            case XmlNodeType.Text: //Display the text in each element.

                                if (elementName == "name")
                                {
                                    typeName = readerI.Value;
                                }
                                else if (elementName == "description")
                                {
                                    typeDescription = readerI.Value;
                                }
                                else
                                {
                                    typeImage = readerI.Value.First();
                                }
                                break;
                            case XmlNodeType.EndElement:
                                if (readerI.Name == "income")
                                {
                                    incomeType.Add(new TransType(typeName, typeImage, typeDescription));
                                    typeImage = 'm';
                                }
                                break;
                            default:
                                //
                                break;
                        }
                    }
                });
            }).Start();


            new Task( async ()=>
            {
                string elementName = String.Empty;
                string typeName = String.Empty;
                char typeImage = 'm';
                string typeDescription = String.Empty;
                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    while (readerE.Read())
                    {
                        switch (readerE.NodeType)
                        {
                            case XmlNodeType.Element: // The node is an element.
                                elementName = readerE.Name;
                                break;

                            case XmlNodeType.Text: //Display the text in each element.

                                if (elementName == "name")
                                {
                                    typeName = readerE.Value;
                                }
                                else if (elementName == "description")
                                {
                                    typeDescription = readerE.Value;
                                }
                                else
                                {
                                    typeImage = readerE.Value.First();
                                }
                                break;
                            case XmlNodeType.EndElement:
                                if (readerE.Name == "expense")
                                {
                                    expenseType.Add(new TransType(typeName, typeImage, typeDescription));
                                    typeImage = 'm';
                                }
                                break;
                            default:
                                //
                                break;
                        }
                    }
                });
            }).Start();


        }

        private void LoadDatabase()
        {
            new Task( async() =>
            {
                //load type files
                expenseTypeFile = await installFolder.GetFileAsync("expense_categories.xml");
                incomeTypeFile = await installFolder.GetFileAsync("income_categories.xml");

                //load data files
                expenseFile = await installFolder.GetFileAsync("expense_data.xml");
                incomeFile = await installFolder.GetFileAsync("income_data.xml");




            }).RunSynchronously();
        }

        private async void LoadTransactions()
        {
            new Task(delegate ()
            {
                LoadIncome();
            }).Start();
            

            //new Task(delegate()
            //{
                LoadExpense();
            //}).Start();

            /*
            // Get the file from the install location  
            var file = await Package.Current.InstalledLocation.GetFileAsync("cities.db");

            // Create a new SQLite instance for the file 
            var db = new Database(file);

            // Open the database asynchronously
            await db.OpenAsync(SqliteOpenMode.OpenRead);

            // Prepare a SQL statement to be executed
            var statement = await db.PrepareStatementAsync(
              "SELECT rowid, CityName FROM Cities;");

            // Loop through all the results and add to the collection
            while (await statement.StepAsync())
                income.Add(new Transaction(statement.GetIntAt(0), statement.GetTextAt(1), statement.GetTextAt(2).First(), DateTime.Now, statement.GetTextAt(4), GetImageFromIBuffer(statement.GetBlobAt(5)), statement.GetTextAt(6)));
            ImageSource ids = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("http://image.source.de"));
            */
        }

        public ImageBrush GetImageFromIBuffer(IBuffer b)
        {
            BitmapImage image = new BitmapImage();
            image.SetSource(b.AsStream().AsRandomAccessStream());

            ImageBrush im = new ImageBrush();
            im.ImageSource = image;
            return im;
        }


        private async void LoadIncome()
        {
            XmlDocument xmlDocI = await XmlDocument.LoadFromFileAsync(await installFolder.GetFileAsync("income_data.xml"));
            XDocument xDocI = XDocument.Parse(xmlDocI.GetXml());
            XmlReader readerI = xDocI.CreateReader();


            new Task(() =>
            {
                string elementName = String.Empty;
                int amount = 0;
                string type = String.Empty;
                char typeImage = 'z';
                DateTime date = new DateTime();
                string description = String.Empty;

                    while (readerI.Read())
                    {
                        switch (readerI.NodeType)
                        {
                            case XmlNodeType.Element: // The node is an element.
                                elementName = readerI.Name;
                                break;

                            case XmlNodeType.Text: //Display the text in each element.

                            if (elementName == "amount")
                            {
                                amount = Int32.Parse(readerI.Value);
                            }
                            else if (elementName == "type")
                            {
                                type = readerI.Value;
                            }
                            else if (elementName == "image")
                            {
                                typeImage = readerI.Value.First();
                            }
                            else if (elementName == "date")
                            {
                                string[] dateS = readerI.Value.Split('-');
                                date = new DateTime(Int32.Parse(dateS[2]), Int32.Parse(dateS[1]), Int32.Parse(dateS[0]));
                            }
                            else if (elementName == "description")
                            {
                                description = readerI.Value;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (readerI.Name == "income")
                                AddIncome(amount,type,typeImage,date,description);
                            break;
                        default:
                            break;
                        }
                    }
            }).Start();
        }

        private async void LoadExpense()
        {
            xmlDocE = await XmlDocument.LoadFromFileAsync(await installFolder.GetFileAsync("expense_data.xml"));
            xDocE = XDocument.Parse(xmlDocE.GetXml());
            XmlReader readerE = xDocE.CreateReader();


            //new Task(() =>
            //{
                string elementName = String.Empty;
                int amount = 0;
                string type = String.Empty;
                char typeImage = 'z';
                DateTime date = new DateTime();
                string description = String.Empty;

                while (readerE.Read())
                {
                    switch (readerE.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            elementName = readerE.Name;
                            break;

                        case XmlNodeType.Text: //Display the text in each element.

                            if (elementName == "amount")
                            {
                                amount = Int32.Parse(readerE.Value);
                            }
                            else if (elementName == "type")
                            {
                                type = readerE.Value;
                            }
                            else if (elementName == "image")
                            {
                                typeImage = readerE.Value.First();
                            }
                            else if (elementName == "date")
                            {
                                string[] dateS = readerE.Value.Split('-');
                                date = new DateTime(Int32.Parse(dateS[2]), Int32.Parse(dateS[1]), Int32.Parse(dateS[0]));
                            }
                            else if (elementName == "description")
                            {
                                description = readerE.Value;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (readerE.Name == "income")
                                AddIncome(amount, type, typeImage, date, description);
                            break;
                        default:
                            break;
                    }
                }
            //}).Start();
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //SideBar.InitializeDrawerLayout();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SideBar.InitializeDrawerLayout();

            gridExpenseType.SelectionChanged += GridExpenseType_SelectionChanged;

            flipView.SelectionChanged += TransTypeSelectionChanged;
        }

        private void GridExpenseType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (TransType)gridExpenseType.SelectedItem;
            SELECTED_EXPENSE_TYPE = selectedItem.TYPE;
            SELECTED_EXPENSE_TYPE_IMAGE = selectedItem.TYPE_IMAGE;
            SELECTED_EXPENSE_TYPE_DESCRIPTION = selectedItem.TYPE_DESCRIPTION;
            IS_EXPENSE_TYPE_SELECTED = true;
        }


        private void CheckForOK()   //checks the required conditions for the okButton Button to be enabled
        {
            if(IS_EXPENSE_TAB_SELECTED)
            okButton.IsEnabled = expenseTextBox.Text != String.Empty && IS_EXPENSE_TYPE_SELECTED ? true : false; //checks condition for expense tab
            else okButton.IsEnabled = incomeTextBox.Text != String.Empty &&  IS_INCOME_TYPE_SELECTED? true : false; //checks condition for income tab
        }


        //following are the expense methods
        public void AddExpense(int amount,string TYPE,char typeImage,DateTime date,string reciever,string description)
        {
            expense.Add(new Transaction(amount,TYPE, typeImage, date, reciever,null,description));
        }

        //following are the income methods

        private async void AddIncome(int amount, string type, char typeImage, DateTime date, string description)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                income.Add(new Transaction(amount, type, typeImage, date, "SELF",null,description));
                
            });
        }


        /// <summary>
        /// returns a string with respect a character sent
        /// </summary>
        /// <param name="typeImage"></param>
        /// <returns></returns>
        private string GetTypeFromImg(char typeImage)
        {
            switch (typeImage)
            {
                case 'b':
                    return "Habitual Expense";
                case 'c':
                    return "Commute";
                case 'e':
                    return "Education";
                case 'f':
                    return "Food";
                case 'g':
                    return "Grocery";
                case 'p':
                    return "Personal";
                case 't':
                    return "Capital";
                default:
                    return "Others";
            }
        }




        private void OpenTans(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openTransaction"]).Begin();
        }

        //closes the transaction menu even when the user tapped outside the box... android lollipop effect :P
        private void RectangleTapped(object sender, TappedRoutedEventArgs e)
        {
            //no additional code is required... this inturn triggers the uncheched event of toggleButton
            toggleButton.IsChecked = false;
            leftToggleBtn.IsChecked = false;
            rightToggleBtn.IsChecked = false;
        }

        private async void OKClick(object sender, RoutedEventArgs e)
        {
            if (IS_EXPENSE_TAB_SELECTED) //check if flip page is on expense or in income page
            {
                TransType obj = (TransType)gridExpenseType.SelectedItem;

                //XmlDocument xmlDocI = await XmlDocument.LoadFromFileAsync(expenseFile);
                //XDocument xDocI = XDocument.Parse(xmlDocI.GetXml());
                //XmlWriter writerI = xDocI.CreateWriter();
                //writerI.WriteElementString("amount", expenseTextBox.Text);
                AddExpense(Int32.Parse(expenseTextBox.Text),obj.TYPE, obj.TYPE_IMAGE, DateTime.Now, string.Empty, expenseDescription.PlaceholderText);


                //var amt = xmlDocE.CreateElement("amount");
                //amt.InnerText = expenseTextBox.Text;

                //xmlDocE.DocumentElement.AppendChild(amt);

                //await xmlDocE.SaveToFileAsync(expenseFile);

            }

            else
            {
                TransType obj = (TransType)gridIncomeType.SelectedItem;
                AddIncome(Int32.Parse(incomeTextBox.Text),obj.TYPE, obj.TYPE_IMAGE, DateTime.Now, String.Empty);
            }

            ((Storyboard)Resources["openTransactionRev"]).Begin();
        }


        //open menu and navigate to dashboard
        private void OpenDashboard(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(typeof(Dashboard));
            DrawerOpen();
            UpdateIndicator(0);
        }


        //close menu and display income/expense list
        private void OpenList(object sender, RoutedEventArgs e)
        {
            if (SideBar.IsDrawerOpen)
                CloseDrawer();
            UpdateIndicator(1);
        }


        private void CloseDrawer()
        {
            SideBar.CloseDrawer();
            UpdateIndicator(1);
        }
        //open menu and navigate to calendar
        private void OpenCalendar(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(typeof(CalendarPage));
            DrawerOpen();
            UpdateIndicator(2);
        }

        //open menu and navigate to salarytracker page
        private void OpenSalaryList(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(typeof(SalaryTrackerPage));
            DrawerOpen();
            UpdateIndicator(3);
        }

        //open menu and navigate to settings
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(typeof(SettingsPage));
            DrawerOpen();
            UpdateIndicator(4);
        }

        //dont know why this is here
        private void OpenRapidTrans(object sender, HoldingRoutedEventArgs e)
        {
            e.Handled = true;
        }
        //check and open Drawer safely
        private void DrawerOpen()
        {
            if (!SideBar.IsDrawerOpen)
                SideBar.OpenDrawer();
        }

        //update the location of the menuindicator
        private void UpdateIndicator(int location)
        {
            menuIndicator.Margin = new Thickness(0, location * 75, 0, 0);
        }

        //TO-DO: Should have smooth transitions
        //STATUS:BUGGY
        //handle pointer wheel events for SideBar
        private void SideBarPointerWheel(object sender, PointerRoutedEventArgs e)
        {
            int mwY = e.GetCurrentPoint((UIElement)sender).Properties.MouseWheelDelta;//mousewheel delta- returns either positive or negative
            double mar = menuIndicator.Margin.Top; //get current margin of menuIndicator and assign to mar
            if (mwY > 0 && menuIndicator.Margin.Top>=75) //if menuIndicator pos is greater than or equal 75 only, we can decrement
            {
                menuIndicator.Margin = new Thickness(0, mar-75, 0, 0); 
            }
            else if(mwY <0 && menuIndicator.Margin.Top<300) //75*4 =300 , where 4 is the number 0 based count of menus
            {
                menuIndicator.Margin = new Thickness(0, mar+75, 0, 0);
            }
            if (menuIndicator.Margin.Top == 75) //cannot use mar value because, we changed menuIndicator value in previous steps.
            {
                if (SideBar.IsDrawerOpen)
                    CloseDrawer();
            }
            else
            {
                if(!SideBar.IsDrawerOpen)
                SideBar.OpenDrawer();
            }
        }

        private void CloseTrans(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openTransactionRev"]).Begin();
            incomeTextBox.Text = String.Empty;
            expenseTextBox.Text = String.Empty;
        }

        private void TransTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IS_EXPENSE_TAB_SELECTED = flipView.SelectedIndex == 1 ? true : false;
            if (IS_EXPENSE_TAB_SELECTED)//checking if current is expense tab
            {
                okButton.IsEnabled = expenseTextBox.Text != String.Empty ? true : false;//if contains text, enable okButton else disable...for expenseTab
            }
            else //else income tab
            {
                okButton.IsEnabled = incomeTextBox.Text != String.Empty ? true : false;//if contains text, enable okButton else disable...for incomeTab
            }
        }

        private void AmountTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            okButton.IsEnabled = txtBox.Text.Count() >= 1 ? true : false;//if contains text, enable okButton else disable...for expenseTab
        }
        

        private void LeftSortChecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openSortMenuLeft"]).Begin();
        }

        private void LeftSortUnchecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openSortMenuLeftRev"]).Begin();
        }

        private void RightSortChecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openSortMenuRight"]).Begin();
        }

        private void RightSortUnchecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)Resources["openSortMenuRightRev"]).Begin();
        }

        private void SortButtonIncome(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Content.ToString())
            {
                case "Name":
                    income.OrderBy(x => x.AMOUNT > 10000);
                    break;
                case "Date":
                    break;
                case "Amount":
                    break;
                default:
                    break;
            }
        }

        private void SortButtonExpense(object sender, RoutedEventArgs e)
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


        private void IncomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            LOpenControlMenu(IncomeView.SelectedItems.Count > 0 ? true : false);
        }


        private void LOpenControlMenu(bool open)
        {
            if (open) ((Storyboard)Resources["lControlGridOpen"]).Begin();
            else ((Storyboard)Resources["lControlGridOpenRev"]).Begin();
        }

        private void ExpenseSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ROpenControlMenu(ExpenseView.SelectedItems.Count > 0 ? true : false);
        }


        private void ROpenControlMenu(bool open)
        {
            if (open) ((Storyboard)Resources["rControlGridOpen"]).Begin();
            else ((Storyboard)Resources["rControlGridOpenRev"]).Begin();
        }

        private void LDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            income.RemoveAll<Transaction>(x => IncomeView.SelectedItems.Contains(x));
        }

        private void LEditButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void RDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            expense.RemoveAll<Transaction>(x => ExpenseView.SelectedItems.Contains(x));
        }

        private void REditButtonClick(object sender, RoutedEventArgs e)
        {

        }
        

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            STORY_SYNC.Begin();
            STORY_REALIME.Stop();
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            STORY_REALIME.Begin();
            STORY_SYNC.Stop();
        }
    }
}
