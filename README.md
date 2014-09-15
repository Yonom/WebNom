WebNom [![Build status](https://ci.appveyor.com/api/projects/status/96evkkavgnkovs70)](https://ci.appveyor.com/project/Yonom/webnom)
======

A [MuffinFramework](https://github.com/Yonom/MuffinFramework) wrapper around HttpListener.

## Getting started

To setup WebNom, create a new console application, download and refrence WebNom.dll and MuffinFramework.dll. Make sure to add a refrence to System.Net.Http as well.

```csharp
using System.Threading;

namespace <Your_Namespace_here>
{
    class Program
    {
        static void Main()
        {
            var client = new WebNomClient();
            client.DetectDebug(); // Run on port 8080 if we are debugging
            client.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
```
**Note:** The port will be set to 80 if you are running on "Release" configuration, and 8080 if you are running on "Debug".  
To run your program in release mode, you need to start visual studio with administrator rights.

## Pages tutorial

To create your first page, you can use the following template:

```csharp
using System.Net;
using WebNom.Pages;

namespace <Your_Namespace_here>
{
    internal class MainPage : Page
    {
        protected override string Path
        {
            get { return "/"; }
        }

        protected override void Run(HttpListenerContext context, InputReader input, OutputWriter output)
        {
            output.Write("Hello world!");
        }
    }
}
```

The **Path** function specifies what urls are handled by this command. This is provided as a regular expression. Here are some examples:

- "/": The root path on your server.
- "/test": The path /test
- "/|/home": Root or /home
- "/profiles/(.+)": Any path that begins with /profiles/
- "/settings(.php)?": /settings or /settings.php

The **HttpListenerContext** is used to get information about the request that was made.  
The **InputReader** helps you get the data provided with the request (POST data, for example)  
The **OutputWriter** helps you send text to the user.
