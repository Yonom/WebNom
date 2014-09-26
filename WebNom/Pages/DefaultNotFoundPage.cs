using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WebNom.Pages
{
    public class DefaultNotFoundPage : Page
    {
        protected override string Path
        {
            get { return null; }
        }

        protected override void Run(HttpListenerContext context, InputReader input, OutputWriter output)
        {
            context.Response.StatusCode = 404;
            var writer = new OutputWriter(context.Response);
            writer.Write(@"<!DOCTYPE HTML PUBLIC ""-//IETF//DTD HTML 2.0//EN"">
<html><head>
<title>404 Not Found</title>
</head><body>
<h1>Not Found</h1>
<p>The requested URL " + context.Request.Url.AbsolutePath + @" was not found on this server.</p>
</body></html>");
        }
    }
}
