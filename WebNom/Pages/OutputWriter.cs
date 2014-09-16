using System.Linq;
using System.Net;
using System.Text;

namespace WebNom.Pages
{
    public class OutputWriter
    {
        private readonly HttpListenerResponse _response;

        internal OutputWriter(HttpListenerResponse response)
        {
            this._response = response;
        }

        public void AddHeader(string name, string value)
        {
            this._response.AddHeader(name, value);
        }

        public void Write(byte[] bytes)
        {
            this._response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        public void Write(string text)
        {
            if (text == null) return;

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            this.Write(bytes);
        }

        public void WriteLine(string text)
        {
            this.Write(text + '\n');
        }
    }
}