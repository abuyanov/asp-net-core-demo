using Atalasoft.Imaging.Codec;
using Atalasoft.Imaging.Codec.Pdf;
using Atalasoft.Imaging.WebControls.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebDocumentViewerCors.Npm
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                    // This address can be found in Properties/launchSettings.json.
                    // Comment this line, to disable CORS support for this origin, so your browser
                    // will show an error in its console about it.
                    .WithOrigins("http://localhost:37510")
                    // For this demo we allow any header, but you can setup your own and specific headers
                    .AllowAnyHeader() 
                    // For this demo we allow any method, however you can use more strict conditions
                    .AllowAnyMethod()) 
                .UseDefaultFiles()
                .UseStaticFiles();

            RegisteredDecoders.Decoders.Add(new PdfDecoder());

            app.Map("/wdv", wdvApp => wdvApp.RunWebDocumentViewerMiddleware());
        }
    }
}
