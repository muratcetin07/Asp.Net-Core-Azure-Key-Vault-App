using AppConfiguration;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCoreAzureKeyVaultApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            #region Load_All_Environment_Appsettings
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            #endregion

            var config = builder.Build();

            #region Get_Parameter_Definitios_From_AzureKeyvault
            if (env.IsDevelopment())
            {
                builder.AddAzureKeyVault($"https://{config[$"AzureKeyVaultUseClientApi:Vault"]}.vault.azure.net/",
                    config["AzureKeyVaultUseClientApi:ClientId"],
                    config["AzureKeyVaultUseClientApi:ClientSecret"]);
            }
            #endregion

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services, IHostingEnvironment env)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            //get appsettings parameters on application startup
            AppConfiguration.AppConfiguration.Configure(Configuration);

            services.AddScoped<DbContext, DataContext>();

            if (env.IsDevelopment() || env.IsStaging())
            {
                //get database sql connection string from azure with client api and secret on development and test enviroment 
                var conn = Configuration[AppConfiguration.AppConfiguration.AzureKeyVaultUseClientApi.GetConnectionSecret()];
                services.AddDbContext<DataContext>(options => options.UseSqlServer(conn));
            }
            else if (env.IsProduction())
            {
                //get database sql connection string from azure on production enviroment
                //you should give your access policy to your application production slot and  your azure key vault definiton on your azure portal
                services.AddDbContext<DataContext>(options => options.UseSqlServer(AppConfiguration.AppConfiguration.GetDbConnStringFromAzureKeyVault().Result));
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
