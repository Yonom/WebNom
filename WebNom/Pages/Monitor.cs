using System;
using System.Net;
using System.Text.RegularExpressions;
using MuffinFramework.Muffins;
using WebNom.Http;

namespace WebNom.Pages
{
    public abstract class Monitor : Muffin
    {
        private HttpPlatform _httpPlatform;

        protected override void Enable()
        {
            this._httpPlatform = this.PlatformLoader.Get<HttpPlatform>();
            this._httpPlatform.Receive += this._httpPlatform_Receive;
            this._httpPlatform.ServerError += _httpPlatform_ServerError;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._httpPlatform.Receive -= this._httpPlatform_Receive;
                this._httpPlatform.ServerError -= _httpPlatform_ServerError;
            }

            base.Dispose(disposing);
        }

        void _httpPlatform_ServerError(HttpListenerContext context, Exception ex)
        {
            this.OnServerError(context, ex);
        }

        private void _httpPlatform_Receive(HttpListenerContext context, ref bool handled)
        {
            this.OnRequest(context);
        }

        protected abstract void OnRequest(HttpListenerContext context);
        protected abstract void OnServerError(HttpListenerContext context, Exception ex);
    }
}