using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace WebNom.Pages
{
    public class InputReader
    {
        private readonly Lazy<NameValueCollection> _get;
        private readonly Lazy<string> _post;
        private readonly HttpListenerRequest _request;

        internal InputReader(HttpListenerRequest request)
        {
            this._request = request;

            this._post = new Lazy<string>(() => { return new StreamReader(this._request.InputStream).ReadToEnd(); });

            this._get = new Lazy<NameValueCollection>(() =>
            {
                string input = this._request.Url.Query;
                return HttpUtility.ParseQueryString(input);
            });
        }

        public string Post
        {
            get { return this._post.Value; }
        }

        public NameValueCollection Get
        {
            get { return this._get.Value; }
        }
    }
}