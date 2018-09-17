using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebDocumentViewerCors.Bower
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
#if !DEBUG
                // I want to use specific host and port number,
                // when we launch this application built in
                // the 'Release' configuration.
                .UseUrls($"http://localhost:5000")
#endif
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
