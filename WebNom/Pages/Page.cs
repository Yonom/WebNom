using System;
using System.Net;
using System.Text.RegularExpressions;
using MuffinFramework.Muffins;
using WebNom.Http;

namespace WebNom.Pages
{
    public abstract class Page : Muffin
    {
        private HttpPlatform _httpPlatform;
        protected abstract string Path { get; }

        protected override void Enable()
        {
            this._httpPlatform = this.PlatformLoader.Get<HttpPlatform>();
            this._httpPlatform.Receive += this._httpPlatform_Receive;
        }

        private void _httpPlatform_Receive(HttpListenerContext context, ref bool handled)
        {
            if (!handled && this.CanHandle(context.Request.Url))
            {
                handled = true;
                this.Invoke(context);
            }
        }

        public void Invoke(HttpListenerContext context)
        {
            this.Run(context, new InputReader(context.Request), new OutputWriter(context.Response));
        }

        protected virtual bool CanHandle(Uri url)
        {
            return new Regex("^(" + this.Path + ")$").IsMatch(url.AbsolutePath);
        }

        protected abstract void Run(HttpListenerContext context, InputReader input, OutputWriter output);
    }
}