using MahApps.Metro.Controls;
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

namespace Expense_Manager__
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : MetroWindow
    {
        public MainPage()
        {
            InitializeComponent();
        }
        #region UserName

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string userNameContent = UserName.Text;
            //try
            //{
            //    string[] userNameParts = userNameContent.Split('@','.');

            //    if (userNameParts[1] == "gmail")
            //    {
            //        MessageBox.Show("Welcome Gmail user");
            //    }
            //    if (userNameParts[1] == "outlook")
            //    {
            //        MessageBox.Show("Welcome outlook user");
            //    }
            //    if (userNameParts[1] == "yahoo")
            //    {
            //        MessageBox.Show("Welcome yahoo user");
            //    }
            //}
            //catch
            //{

            //}
        }


        #endregion

        #region PasswordBox

        private void TogglePassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AccountPassword.Visibility == System.Windows.Visibility.Visible)
            {
                OpenP.Visibility = System.Windows.Visibility.Visible;
                AccountPassword.Visibility = System.Windows.Visibility.Collapsed;
                DummyPassword.Visibility = System.Windows.Visibility.Visible;
                DummyPassword.Text = AccountPassword.Password;

            }
            else
            {
                OpenP.Visibility = System.Windows.Visibility.Collapsed;
                AccountPassword.Visibility = System.Windows.Visibility.Visible;
                DummyPassword.Visibility = System.Windows.Visibility.Collapsed;
                AccountPassword.Password = DummyPassword.Text;
            }
        }

        private void DummyPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DummyPassword.Text.Length == 10)
            {
                WarningP.Visibility = System.Windows.Visibility.Visible;
            }

            if (DummyPassword.Text.Length < 10)
            {
                if (WarningP.Visibility == System.Windows.Visibility.Visible)
                {
                    WarningP.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void forgetP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Change your Password");
        }

        private void AccountPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {

            if (AccountPassword.Password.Length == 10)
            {
                WarningP.Visibility = System.Windows.Visibility.Visible;
            }

            if (AccountPassword.Password.Length < 10)
            {
                if (WarningP.Visibility == System.Windows.Visibility.Visible)
                {
                    WarningP.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void AccountPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            AccountPassword.Foreground = System.Windows.Media.Brushes.White;
        }

        private void AccountPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            AccountPassword.Foreground = System.Windows.Media.Brushes.LightGray;
        }

        private void forgetP_MouseEnter(object sender, MouseEventArgs e)
        {
            forgetP.Foreground = System.Windows.Media.Brushes.LightGray;
        }
        private void forgetP_MouseLeave(object sender, MouseEventArgs e)
        {
            forgetP.Foreground = System.Windows.Media.Brushes.White;
        }
        #endregion

        #region Login
        private void login_Click(object sender, RoutedEventArgs e)
        {
            //if (UserName.Text == "hemanth" && AccountPassword.Password == "monk" || DummyPassword.Text == "monk")
            //{
            //    MainWindow mw = new MainWindow();
            //    mw.Show();
            //    this.Close();
            //    //System.Diagnostics.Process.Start("http://www.google.com");
            //}
            //else if (UserName.Text != "hemanth" && AccountPassword.Password != "monk" || DummyPassword.Text != "monk")
            //{
            //    WarningP.Content = "username or password is incorrect";
            //    WarningP.Visibility = System.Windows.Visibility.Visible;
            //}
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
        #endregion
    }
}
