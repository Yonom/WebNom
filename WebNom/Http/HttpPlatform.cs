using System;
using System.Net;
using System.Threading;
using MuffinFramework.Platforms;
using WebNom.Pages;

namespace WebNom.Http
{
    internal class HttpPlatform : Platform
    {
        public delegate void ReceiveCallback(HttpListenerContext context, ref bool handled);

        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private HttpListener _listener;

        public event ReceiveCallback Receive;

        protected virtual void OnReceive(HttpListenerContext context, ref bool handled)
        {
            ReceiveCallback handler = this.Receive;
            if (handler != null) handler(context, ref handled);
        }

        protected override void Enable()
        {
            this._listener = new HttpListener();
            this._listener.Start();

            ThreadPool.QueueUserWorkItem(this.Work);
        }

        internal void Activate(int port)
        {
            this._listener.Prefixes.Add(String.Format("http://localhost:{0}/", port));
        }

        private void Work(object state)
        {
            while (true)
            {
                this._listener.BeginGetContext(this.OnConnection, state);
                this._resetEvent.WaitOne();
            }
        }

        private void OnConnection(IAsyncResult ar)
        {
            this._resetEvent.Set();

            HttpListenerContext context = this._listener.EndGetContext(ar);
            bool handled = false;
            this.OnReceive(context, ref handled);

            if (!handled)
            {
                context.Response.StatusCode = 404;
                var writer = new OutputWriter(context.Response);
                writer.Write(@"<!DOCTYPE HTML PUBLIC ""-//IETF//DTD HTML 2.0//EN"">
<html><head>
<title>404 Not Found</title>
</head><body>
<h1>Not Found</h1>
<p>The requested URL was not found on this server.</p>
</body></html>");
            }

            context.Response.Close();
        }
    }
}