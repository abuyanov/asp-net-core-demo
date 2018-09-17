using System;
using Atalasoft.Imaging.Codec;
using Atalasoft.Imaging.Codec.Office;
using Atalasoft.Imaging.Codec.Pdf;
using Atalasoft.Imaging.WebControls.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebCaptureWithAppend.Npm
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            RegisteredDecoders.Decoders.Add(new PdfDecoder());
            RegisteredDecoders.Decoders.Add(new OfficeDecoder());
            // setup options for form content. It's needed to Web Capture Service for files upload.
            // in this case we simply set maximum values, however you can set more strict values for your server.
            services.Configure<FormOptions>(opt =>
            {
                opt.BufferBodyLengthLimit = long.MaxValue;
                opt.ValueLengthLimit = int.MaxValue;
                opt.MultipartBodyLengthLimit = long.MaxValue;
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

            app.UseDefaultFiles()
               .UseStaticFiles();

            app.Map("/wcs", wcsApp => wcsApp.RunWebCaptureMiddleware())
               .Map("/wdv", wdvApp => wdvApp.RunWebDocumentViewerMiddleware());
        }
    }
}