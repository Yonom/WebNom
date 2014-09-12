using System.Net;
using WebNom.Pages;

namespace WebNom.Demo
{
    internal class Index : Page
    {
        protected override string Path
        {
            get { return "/(.*)"; }
        }

        protected override void Run(HttpListenerContext context, InputReader input, OutputWriter output)
        {
            output.Write("HAI!");
        }
    }
}