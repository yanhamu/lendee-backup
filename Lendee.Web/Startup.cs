using Autofac;
using Lendee.Web.Configurations;
using Lendee.Web.Configurations.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Lendee.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddHttpContextAccessor();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.RegisterUserDependencies(configuration);
            services
                .AddControllersWithViews()
                .AddViewLocalization();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<LendeeModule>();
            builder.RegisterModule<LendeeWebModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            NLog.Extensions.Logging.ConfigSettingLayoutRenderer.DefaultConfiguration = configuration;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
