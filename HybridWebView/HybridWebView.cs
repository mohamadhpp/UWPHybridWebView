using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace HybridWebView
{
    public sealed class HybridWebView : WebView2
    {
        private string _appPath;


        #region App Configuration

        public string AppName { get; set; }

        /// <summary>
        /// This address must be inside the Assets folder;
        /// </summary>
        public string AppPath
        {
            get
            {
                return AppPath;
            }
            set
            {
                _appPath = Path.Combine("Assets", value);
            }
        }

        public string FileName { get; set; } = "index.html";

        #endregion


        #region Sensors Access

        public bool AllSensorsAccess { get; set; } = false;

        public bool CameraAccess { get; set; } = false;

        public bool ClipboardReadAccess { get; set; } = false;

        public bool MicrophoneAccess { get; set; } = false;

        public bool NotificationsAccess { get; set; } = false;

        public bool UnknownPermissionAccess { get; set; } = false;

        #endregion


        #region Events

        public event EventHandler<string> HybridWebViewMessageReceived;

        #endregion


        #region Handlers

        private void HandlePermissionRequested(CoreWebView2 sender, CoreWebView2PermissionRequestedEventArgs args)
        {
            if (AllSensorsAccess)
            {
                args.State = CoreWebView2PermissionState.Allow;
                return;
            }

            if (args.PermissionKind == CoreWebView2PermissionKind.Camera && CameraAccess)
            {
                args.State = CoreWebView2PermissionState.Allow;
                return;
            }

            if (args.PermissionKind == CoreWebView2PermissionKind.ClipboardRead && ClipboardReadAccess)
            {
                args.State = CoreWebView2PermissionState.Allow;
                return;
            }

            if (args.PermissionKind == CoreWebView2PermissionKind.Microphone && MicrophoneAccess)
            {
                args.State = CoreWebView2PermissionState.Allow;
                return;
            }

            if (args.PermissionKind == CoreWebView2PermissionKind.Notifications && NotificationsAccess)
            {
                args.State = CoreWebView2PermissionState.Allow;
                return;
            }

            if (args.PermissionKind == CoreWebView2PermissionKind.UnknownPermission && UnknownPermissionAccess)
            {
                args.State = CoreWebView2PermissionState.Allow;
                return;
            }
        }

        private void HandleWebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string message = args.TryGetWebMessageAsString();

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            HybridWebViewMessageReceived?.Invoke(this, message);
        }

        #endregion


        #region Initilize

        public HybridWebView()
        {
            Init();
        }

        private async void Init()
        {
            await EnsureCoreWebView2Async();
            CoreWebView2.SetVirtualHostNameToFolderMapping(AppName, _appPath, CoreWebView2HostResourceAccessKind.Allow);

            CoreWebView2.WebMessageReceived += HandleWebMessageReceived;
            CoreWebView2.PermissionRequested += HandlePermissionRequested;

            Source = new Uri($"https://{AppName}/{FileName}");

            CoreWebView2.Settings.AreDevToolsEnabled = true;
            CoreWebView2.Settings.IsWebMessageEnabled = true;
        }

        #endregion


        public async void ExecuteJsScriptAsync(string methodName, [ReadOnlyArray] string[] args)
        {
            string script = methodName;

            if (args == null)
            {
                script += "()";
            }
            else
            {
                script += $"({string.Join(",", args)})";
            }

            await ExecuteScriptAsync(script);
        }
    }
}