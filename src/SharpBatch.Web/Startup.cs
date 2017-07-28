using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using SharpBatch.Tracking.Abstraction;
using SharpBatch.Tracking.Memory;
using SharpBatch.Web.Internals;


namespace SharpBatch.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSharpBatchTrackingDB(Configuration.GetConnectionString("DefaultConnection"));
            services.AddScoped<IReportProvider, ReportProvider>();

            //services.TryAddSingleton<ISharpBatchTracking, TrackingMemory>();
            //var embeddedProvider = new EmbeddedFileProvider(typeof(Model).GetTypeInfo().Assembly);
            services.AddMvc();
                //.AddRazorOptions(options => options.FileProviders.Add(embeddedProvider));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
