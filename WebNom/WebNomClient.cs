using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Reflection;
using MuffinFramework;
using WebNom.Http;

namespace WebNom
{
    public class WebNomClient : MuffinClient
    {
        public WebNomClient()
        {
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetCallingAssembly()));

            this.Host = "*";
            this.Port = 80;
        }
        
        public string Host { get; set; }
        public int Port { get; set; }

        public override void Start()
        {
            base.Start();

            this.PlatformLoader.Get<HttpPlatform>().Activate(this.Host, this.Port);
        }

        [Conditional("DEBUG")]
        public void DetectDebug()
        {
            this.Host = "localhost";
            this.Port = 8080;
        }
    }
}