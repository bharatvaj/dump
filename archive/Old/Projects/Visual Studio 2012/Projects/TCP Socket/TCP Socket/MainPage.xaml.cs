using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TCP_Socket.Resources;
using System.Windows.Media;

namespace TCPSocket
{
    public partial class MainPage : PhoneApplicationPage
    {
        int ECHO_PORT = 7;  // The Echo protocol uses port 7 in this sample
        const int QOTD_PORT = 13; 
        public MainPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Handle the btnEcho_Click event by sending text to the echo server 
        /// and outputting the response
        /// </summary>
        private void btnEcho_Click(object sender, RoutedEventArgs e)
        {

            // Clear the log 
            ClearLog();

            // Make sure we can perform this action with valid data
            if (ValidateRemoteHost() && ValidateInput())
            {
                // Instantiate the SocketClient
                SocketClient client = new SocketClient();

                // Attempt to connect to the echo server
                Log(String.Format("Connecting to server '{0}' over port {1} (echo) ...", txtRemoteHost.Text, ECHO_PORT), true);
                string result = client.Connect(txtRemoteHost.Text, ECHO_PORT);
                Log(result, false);

                // Attempt to send our message to be echoed to the echo server
                Log(String.Format("Sending '{0}' to server ...", txtInput.Text), true);
                result = client.Send(txtInput.Text);
                Log(result, false);

                // Receive a response from the echo server
                Log("Requesting Receive ...", true);
                result = client.Receive();
                Log(result, false);

                // Close the socket connection explicitly
                client.Close();
            }

        }

        /// <summary>
        /// Handle the btnEcho_Click event by receiving text from the Quote of 
        /// the Day (QOTD) server and outputting the response 
        /// </summary>
        private void btnGetQuote_Click(object sender, RoutedEventArgs e)
        {
            // Clear the log 
            ClearLog();
            // Make sure we can perform this action with valid data
            if (ValidateRemoteHost())
            {
                // Instantiate the SocketClient object
                SocketClient client = new SocketClient();

                // Attempt connection to the Quote of the Day (QOTD) server
                Log(String.Format("Connecting to server '{0}' over port {1} (Quote of the Day) ...", txtRemoteHost.Text, QOTD_PORT), true);
                string result = client.Connect(txtRemoteHost.Text, QOTD_PORT);
                Log(result, false);

                // Note: The QOTD protocol is not expecting data to be sent to it.
                // So we omit a send call in this example.

                // Receive response from the QOTD server
                Log("Requesting Receive ...", true);
                result = client.Receive();
                Log(result, false);

                // Close the socket conenction explicitly
                client.Close();
            }
        }

        #region UI Validation
        /// <summary>
        /// Validates the txtInput TextBox
        /// </summary>
        /// <returns>True if the txtInput TextBox contains valid data, otherwise 
        /// False.
        ///</returns>
        private bool ValidateInput()
        {
            // txtInput must contain some text
            if (String.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("Please enter some text to echo");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the txtRemoteHost TextBox
        /// </summary>
        /// <returns>True if the txtRemoteHost contains valid data,
        /// otherwise False
        /// </returns>
        private bool ValidateRemoteHost()
        {
            // The txtRemoteHost must contain some text
            if (String.IsNullOrWhiteSpace(txtRemoteHost.Text))
            {
                MessageBox.Show("Please enter a host name");
                return false;
            }

            return true;
        }
        #endregion

        #region Logging
        /// <summary>
        /// Log text to the txtOutput TextBox
        /// </summary>
        /// <param name="message">The message to write to the txtOutput TextBox</param>
        /// <param name="isOutgoing">True if the message is an outgoing (client to server)
        /// message, False otherwise.
        /// </param>
        /// <remarks>We differentiate between a message from the client and server 
        /// by prepending each line  with ">>" and "<<" respectively.</remarks>
        private void Log(string message, bool isOutgoing)
        {
            string direction = (isOutgoing) ? "Client: " : "Server: ";
            txtOutput.Text += Environment.NewLine + direction + message;
        }

        /// <summary>
        /// Clears the txtOutput TextBox
        /// </summary>
        private void ClearLog()
        {
            txtOutput.Text = String.Empty;
        }
        #endregion

        private void logRect_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (logGrid.Height == 110)
                logDrag.Begin();
            else
                logDown.Begin();
        }

        private void AppBar_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (logGrid.Height >= 110 && logGrid.Height <= 658)
                logGrid.Height += -1*e.DeltaManipulation.Translation.Y;
            if (logGrid.Height < 110)
                logGrid.Height = 110;
            if (logGrid.Height > 658)
                logGrid.Height = 658;
        }

        private void AppBar_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (logGrid.Height > 400)
                logDrag.Begin();
            if (logGrid.Height < 400)
                logDown.Begin();
        }

        private void Up_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            logDown.Begin();
        }
    }
}