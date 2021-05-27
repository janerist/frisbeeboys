using Dapper;
using Frisbeeboys.Web.Controllers.Import.Services;
using Frisbeeboys.Web.Controllers.Scorecards.Services;
using Frisbeeboys.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Frisbeeboys.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews();
            if (_env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            
            // Database
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var pgHost = _configuration["PGHOST"];
            var pgPort = _configuration["PGPORT"];
            var pgDatabase = _configuration["PGDATABASE"];
            var pgUser = _configuration["PGUSER"];
            var pgPassword = _configuration["PGPASSWORD"];
            var cnnString =
                $"Server={pgHost};Port={pgPort};Database={pgDatabase};Username={pgUser};Password={pgPassword}";
            services.AddSingleton(new ScorecardDatabase(cnnString));

            // Import
            services.AddSingleton<ImportService>();
            services.AddSingleton<UDiscCsvParser>();
            
            // Scorecards
            services.AddSingleton<ScorecardService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UseSerilogRequestLogging();

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Scorecards}/{action=Index}/{id?}");
            });
        }
    }
}