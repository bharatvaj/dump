// Copyright (c) Microsoft. All rights reserved.

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System;

namespace File360
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        public MainPage()
        {
            this.InitializeComponent();

            // This is a static public property that allows downstream pages to get a handle to the MainPage instance
            // in order to call methods that are in this class.
            Current = this;

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SuspensionManager.RegisterFrame(ScenarioFrame, "scenarioFrame");
            if (ScenarioFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the ScenarioList
                if (!ScenarioFrame.Navigate(typeof(MainGrid)))
                {
                    throw new Exception("Failed to create scenario list");
                }
            }
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            if (ScenarioFrame.CanGoBack)
            {
                // Clear the status block when navigating
                NotifyUser(String.Empty, NotifyType.StatusMessage);

                ScenarioFrame.GoBack();

                //Indicate the back button press is handled so the app does not exit
            }
        }


        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            if (StatusBlock != null)
            {
                switch (type)
                {
                    case NotifyType.StatusMessage:
                        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                        break;
                    case NotifyType.ErrorMessage:
                        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                        break;
                }
                StatusBlock.Text = strMessage;

                // Collapse the StatusBlock if it has no text to conserve real estate.
                if (StatusBlock.Text != String.Empty)
                {
                    StatusBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    StatusBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
        }

        private void ScenarioFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }

    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };

}
