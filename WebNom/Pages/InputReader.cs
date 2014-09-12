using System.IO;
using System.Net;

namespace WebNom.Pages
{
    public class InputReader
    {
        private readonly HttpListenerRequest _request;

        internal InputReader(HttpListenerRequest request)
        {
            this._request = request;
        }

        public string ReadInput()
        {
            return new StreamReader(this._request.InputStream).ReadToEnd();
        }
    }
}