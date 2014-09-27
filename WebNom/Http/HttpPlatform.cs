using System;
using System.Net;
using System.Reflection;
using System.Threading;
using MuffinFramework.Platforms;
using WebNom.Pages;

namespace WebNom.Http
{
    internal class HttpPlatform : Platform
    {
        public delegate void ReceiveCallback(HttpListenerContext context, ref bool handled);
        public delegate void ServerErrorCallback(HttpListenerContext context, Exception ex);

        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private bool _disposed;
        private HttpListener _listener;
        private Page _errorPage;

        public event ReceiveCallback Receive;

        protected virtual void OnReceive(HttpListenerContext context, ref bool handled)
        {
            ReceiveCallback handler = this.Receive;
            if (handler != null) handler(context, ref handled);
        }

        public event ServerErrorCallback ServerError;

        protected virtual void OnServerError(HttpListenerContext context, Exception ex)
        {
            ServerErrorCallback handler = this.ServerError;
            if (handler != null) handler(context, ex);
        }

        protected override void Enable()
        {
            this._listener = new HttpListener();
            this._listener.Start();

            ThreadPool.QueueUserWorkItem(this.Work);
        }

        internal void Activate(string host, int port, Page errorPage)
        {
            this._errorPage = errorPage;
            this._listener.Prefixes.Add(String.Format("http://{0}:{1}/", host, port));
        }

        private void Work(object state)
        {
            while (!this._disposed)
            {
                this._listener.BeginGetContext(ar =>
                {
                    this._resetEvent.Set();

                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        HttpListenerContext context = null;

                        try
                        {
                            context = this._listener.EndGetContext(ar);
                            this.OnConnection(context);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.OnServerError(context, ex);
                            }
                            catch (Exception ex2)
                            {
                                Console.WriteLine("Unhandled error while running an error handler: " + ex2);
                            }
                        }
                    });
                }, state);

                this._resetEvent.WaitOne();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._disposed = true;
                this._resetEvent.Set();
                this._listener.Stop();
            }

            base.Dispose(disposing);
        }

        private void OnConnection(HttpListenerContext context)
        {
            context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
            context.Response.Headers["X-Powered-By"] = "WebNom/" + Assembly.GetExecutingAssembly().GetName().Version;

            bool handled = false;
            this.OnReceive(context, ref handled);

            if (!handled)
                this._errorPage.Invoke(context);

            context.Response.Close();
        }
    }
}