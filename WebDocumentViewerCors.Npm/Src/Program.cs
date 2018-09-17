using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebDocumentViewerCors.Npm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
#if !DEBUG
// I want to use specific host and port number,
// when we launch this application built in
// the 'Release' configuration.
                .UseUrls($"http://localhost:5000")
#endif
                .UseStartup<Startup>()
                .Build();

    }
}
