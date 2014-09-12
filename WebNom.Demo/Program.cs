using System.Threading;

namespace WebNom.Demo
{
    internal class Program
    {
        private static void Main()
        {
            var client = new WebNomClient();
            client.DetectDebug(); // Run on port 8080 if we are debugging
            client.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}