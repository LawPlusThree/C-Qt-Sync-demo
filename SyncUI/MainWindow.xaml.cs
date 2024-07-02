using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace SyncUI
{
    public sealed partial class MainWindow : Window
    {
        private NamedPipeClient _namedPipeClient;

        public MainWindow(NamedPipeClient namedPipeClient)
        {
            this.InitializeComponent();
            _namedPipeClient = namedPipeClient;
        }

        private async void OnSendButtonClick(object sender, RoutedEventArgs e)
        {
            string message = "Hello, Server!";
            await _namedPipeClient.SendMessageAsync(message);
        }

        private async void OnReceiveButtonClick(object sender, RoutedEventArgs e)
        {
            string response = await _namedPipeClient.ReceiveMessageAsync();
            // Update UI with the response
            ResponseTextBlock.Text = response;
        }
    }
}
