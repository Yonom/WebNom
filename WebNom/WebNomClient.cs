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

            this.Port = 80;
        }

        public int Port { get; set; }

        public override void Start()
        {
            base.Start();

            this.PlatformLoader.Get<HttpPlatform>().Activate(this.Port);
        }

        [Conditional("DEBUG")]
        public void DetectDebug()
        {
            this.Port = 8080;
        }
    }
}