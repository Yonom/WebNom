WebNom [![Build status](https://ci.appveyor.com/api/projects/status/96evkkavgnkovs70)](https://ci.appveyor.com/project/Yonom/webnom) ![NuGet version](https://img.shields.io/nuget/v/WebNom.svg)
======

A [MuffinFramework](https://github.com/Yonom/MuffinFramework) wrapper around HttpListener.

## Getting started

To setup WebNom, create a new console application and install WebNom's [NuGet](https://www.nuget.org/packages/WebNom) package.

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

# Monitors tutorial
Monitors can be used to track any request or error that occures on the server. They can be defined in a way that is similar to pages:

```csharp
using WebNom.Pages;

namespace <Your_Namespace_here>
{
    internal class Logger : Monitor
    {
        protected override void OnRequest(HttpListenerContext context)
        {
            // TODO: Log visits
        }

        protected override void OnServerError(HttpListenerContext context, Exception ex)
        {
            // TODO: Log the errors
        }
    }
}
```
