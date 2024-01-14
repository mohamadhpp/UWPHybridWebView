using System;
using Windows.UI.Xaml.Controls;

namespace UWPVueHybridApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            webview.HybridWebViewMessageReceived += OnHybridWebViewMessageReceived;
        }

        private async void OnHybridWebViewMessageReceived(object sender, string message)
        {
            ContentDialog MessageDialog = new ContentDialog
            {
                Title = "Message",
                Content = message,
                CloseButtonText = "OK"
            };

            await MessageDialog.ShowAsync();

            string[] names = { "'John'" };

            webview.ExecuteJsScriptAsync("SayMyName", names);
            webview.ExecuteJsScriptAsync("JsMethod", null);
        }
    }
}