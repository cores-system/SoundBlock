using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace SoundBlock
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            if (!Directory.Exists("profiles/"))
                Directory.CreateDirectory("profiles/");

            foreach (var file in Directory.GetFiles("profiles/"))
            {
                Settings.app = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(file));
                if (Settings.app == null)
                    Settings.app = new Settings();

                break;
            }

            var script = CSharpScript.Create<float>(File.ReadAllText("wwwroot/code.cs"), globalsType: typeof(Globals));
            script.Compile();
            Normalizer.runner = script.CreateDelegate();

            new Thread(() => Normalizer.MonitorPeakValue()).Start();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            // 
            app.UseSignalR(routes => routes.MapHub<GraficHub>("/hubs/grafic"));
        }
    }
}
