using DionysosFX.Host;

namespace VSIXSpotify.AuthorizationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateStartup();
        }

        private static void CreateStartup()
        {
            Host host = new Host();
            var startup = host.CreateStartup<Startup>();
            startup.Configure();
            startup.Build();
        }
    }
}
