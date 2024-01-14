<template>
  
  <div id="container">
    <button id="btn" @click="CSharpMethod">C# Method</button>
  </div>

</template>

<script>
export default 
{
    mounted()
    {
        window.JsMethod = function ()
        {
            alert("Js Method");
        }
        
        window.SayMyName = function (name)
        {
            alert(name);
        }
    },

    unmounted()
    {
      delete window.JsMethod;
      delete window.SayMyName;
    },

    methods:
    {
        CSharpMethod()
        {
            //You can write custom message
            let message = "C# Method Call";

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
        }
    }
}

</script>


<style scoped>

#container
{
  height: 500px;
  display: flex;
  align-items: center;
  justify-content: center;
}

#btn
{
  width: 150px;
  height: 75px;
  background-color: antiquewhite;

  margin: 0 auto;
}

</style>