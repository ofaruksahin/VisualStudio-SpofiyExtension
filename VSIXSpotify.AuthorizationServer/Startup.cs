using DionysosFX.Host;
using DionysosFX.Module.WebApi;
using DionysosFX.Swan;
using DionysosFX.Swan.Modules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VSIXSpotify.AuthorizationServer
{
    public class Startup : IStartup
    {
        IHostBuilder _hostBuilder;

        public Startup(IHostBuilder hostBuilder)
        {
            _hostBuilder = hostBuilder;
        }

        public void Configure()
        {
            _hostBuilder.AddPrefix("http://*:7000");
            _hostBuilder.AddWebApiModule(new WebApiModuleOptions(ResponseType.Json));
        }

        public void Build()
        {
            _hostBuilder.BuildContainer();
            _hostBuilder.UseWebApiModule();
            using (var cts = new CancellationTokenSource())
            {
                Task.WaitAll(RunWebServer(cts.Token));
            }
        }

        private IWebServer CreateWebServer()
        {
            IWebServer webServer = new WebServer(_hostBuilder);
            webServer.StateChanged += (sender, e) => Console.WriteLine($"Server New State {e.NewState}");
            return webServer;
        }

        private async Task RunWebServer(CancellationToken cancellationToken)
        {
            using var server = CreateWebServer();
            await server.RunAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
