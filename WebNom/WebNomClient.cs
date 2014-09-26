using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Reflection;
using MuffinFramework;
using MuffinFramework.Muffins;
using WebNom.Http;
using WebNom.Pages;

namespace WebNom
{
    public class WebNomClient : MuffinClient
    {
        public WebNomClient()
        {
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetCallingAssembly()));

            this.Host = "*";
            this.Port = 80;
            this.NotFoundErrorPage = typeof(DefaultNotFoundPage);
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public Type NotFoundErrorPage { get; set; }

        public override void Start()
        {
            base.Start();

            var methodInfo = typeof(MuffinLoader).GetMethod("Get").MakeGenericMethod(this.NotFoundErrorPage);
            var errorPage = (Page)methodInfo.Invoke(this.MuffinLoader, null);
            this.PlatformLoader.Get<HttpPlatform>().Activate(this.Host, this.Port, errorPage);
        }

        [Conditional("DEBUG")]
        public void DetectDebug()
        {
            this.Host = "localhost";
            this.Port = 8080;
        }
    }
}