using System;
using System.Linq;
using Atalasoft.Imaging.Codec;
using Atalasoft.Imaging.Codec.Pdf;
using Atalasoft.Imaging.WebControls.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace WebDocumentViewerBasic.Bower
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching(); //configures response caching support with default settings
            services.AddResponseCompression(options => //configures response compression
            {
                options.Providers.Add<GzipCompressionProvider>(); //adds gzip compression provider
                //comment this line to not compress images, that are already compressed by PngEncoder.
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/png" }); //adds mime-type "image/png" and default mime-types to the collection of mime-types that should be compressed
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression(). //adds response compression middleware
                UseResponseCaching().  //adds response caching middleware
                UseDefaultFiles().
                UseStaticFiles();

            RegisteredDecoders.Decoders.Add(new PdfDecoder());

            app.Map("/wdv", wdvApp =>
            {
                // adds a custom middleware to setup compression.
                // in this case we add it right before WebDocumentViewer middleware
                wdvApp.Use(async (context, next) =>
                {
                    await next();
                });

                // adds a custom middleware to setup caching header.
                // in this case we add it right before WebDocumentViewer middleware
                wdvApp.Use(async (context, next) =>
                {
                    context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                    {
                        // You can use any other parameters that you need
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };

                    await next();
                });

                wdvApp.RunWebDocumentViewerMiddleware(new DemoCallbacks(env.WebRootPath));
            });
        }
    }
}