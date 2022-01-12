using ERP.DataAccess.Entity;
using ERP.DataAccess.EntityData;
using ERP.Models.Common;
using ERP.Models.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace ERP.UI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //SeriLogExtensions.ConfigureSeriLog("ERPUI");

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Logs")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Logs"));
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            string mySqlConnectionStr = Configuration.GetValue<string>("AppSettings:ErplanConnString");

            services.AddDbContextPool<ErpDbContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

            services.AddIdentity<ApplicationIdentityUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ErpDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            //services.AddAuthentication().AddFacebook(options =>
            //{
            //    options.AppId = "479144716347128";
            //    options.AppSecret = "8888cefba55e9cfa06a2b28f0495e533";
            //});

            //services.AddAuthentication().AddMicrosoftAccount(options =>
            //{
            //    options.ClientId = "479144716347128";
            //    options.ClientSecret = "8888cefba55e9cfa06a2b28f0495e533";
            //});

            //services.AddAuthentication().AddGoogle(options =>
            //{
            //    options.ClientId = "457450439478-477bgc7uvgfo9ck8bel6i0rkrjk0q266.apps.googleusercontent.com";
            //    options.ClientSecret = "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";

            //});

            services.AddControllers();
            services.AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddRazorPages();

            services.AddHttpContextAccessor();

            // registering dependency injection(application services).
            ApplicationServices.Register(ref services);

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.Configure<MailSettingsModel>(Configuration.GetSection("MailSettings"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandlingPath = "/Error" });
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
           
            app.UseSession();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthorization();

            //app.UseContextAccessor();
            //loggerFactory.AddSeriLog();
            //app.UseSeriLogMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Common}/{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

                endpoints.MapRazorPages().RequireAuthorization();
            });
        }
    }
}
