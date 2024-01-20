# **UWP HybridWebView**
This repo .NET UWP Hybrid Web View control, which enables hosting arbitrary HTML/JS/CSS content in a WebView and enables communication between the code in the WebView (JavaScript) and the code that hosts the WebView (C#/.NET).

**It is possible to convert web applications to Windows application mode and even use it in kiosk mode.
Access to Windows functions is also preserved in place.**

**You can use this for Vue, React and Angular projects.**

_*Nuget:*_ [See Nuget Page](https://www.nuget.org/packages/UWPHybridWebView/1.0.0)

 - [Usage](#Usage)
 - [Methods](#Methods)
 - [Events](#Events)

## **Support Information**

This project is for UWP and can be used in it. It is based on .NET 8.
## **Documents**

After adding the Hybrid WebView component as below, the necessary properties for the component need to be set. \
Some properties are mandatory and some are optional.

Pay attention that the output of your web application must be placed inside the Assets folder or its sub-folders.

All the properties of the component are as follows.

```
AppName: It is just a name for a web application. //Require
AppPath: The path of the main folder of the web application. //Require - Default: Assets
FileName: The main file name of the web application. //Default: index.html

AllSensorsAccess: //Default: False
CameraAccess: //Default: False
ClipboardReadAccess: //Default: False
MicrophoneAccess: //Default: False
NotificationsAccess: //Default: False
UnknownPermissionAccess: //Default: False

```
## **Usage**

```
<Page
    ...
    xmlns:hwv="using:HybridWebView">

    <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
        <hwv:HybridWebView x:Name="webview" 
                           AppName="VueApp"
                           AppPath="Raw/hybrid_root"
                           AllSensorsAccess="True"/>
    </Grid>
</Page>

```
## **Methods**

You can use this method for call a method from Js.

```
ExecuteJsScriptAsync(string methodName, string[] args)
```
Example:
```
string[] names = { "'John'" };

webview.ExecuteJsScriptAsync("SayMyName", names);
webview.ExecuteJsScriptAsync("JsMethod", null);
```

Pay attention that the function on the Js side must also be defined as below.

```
window.{MethodName} = function ()
{
    ...
}
```
Example:
```
window.JsMethod = function ()
{
    alert("Js Method");
}

window.SayMyName = function (name)
{
    alert(name);
}
```

## **Events**
To receive messages sent from the JS side, the event must be used.

```
public MainPage()
{
    this.InitializeComponent();

    webview.HybridWebViewMessageReceived += OnHybridWebViewMessageReceived;
}


private async void OnHybridWebViewMessageReceived(object sender, string message)
{
    ...
}
```

To send a message from the Js side to the C# side, you must proceed as follows.

```
//You can write any message
let message = "Any Message";

if (window.chrome && window.chrome.webview)
{
    window.chrome.webview.postMessage(message);
}
else if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.webwindowinterop)
{
    window.webkit.messageHandlers.webwindowinterop.postMessage(message);
}
else
{
    window.hybridWebViewHost.sendMessage(message);
}
```
