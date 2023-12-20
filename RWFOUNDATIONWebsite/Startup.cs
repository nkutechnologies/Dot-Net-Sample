using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Newtonsoft.Json.Serialization;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.SignalR.Hubs;

namespace RWFOUNDATIONWebsite
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
            services.AddDbContext<RwDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
             .AddEntityFrameworkStores<RwDbContext>()
             .AddDefaultTokenProviders();
            services.AddSingleton<IConfiguration>(Configuration);

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "RWFOUNDATIONCookie";
                options.LoginPath = "/Authentication/Login";
                options.LogoutPath = "/Authentication/Logout";
                options.AccessDeniedPath = "/Authentication/AccessDenied";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });
            
            services.AddMailKit(config => config.UseMailKit(Configuration.GetSection("Email").Get<MailKitOptions>()));          
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1000000);
                options.Cookie.HttpOnly = true;
            });
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "1053610423191-6ilfn1cdmbnrh81lubs5jik27j304jsf.apps.googleusercontent.com";
                    options.ClientSecret = "KHPTxHfsuJV7JwKd9WRWyj_Z";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "706542100104240";
                    options.AppSecret = "71e7e199bfa340c8888cf13378deef4c";                   
                });
               
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();           
            services.AddRazorPages();
            services.AddSignalR();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();           
            services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver());
            services.AddControllersWithViews(x => x.SuppressAsyncSuffixInActionNames = false)
                .AddRazorRuntimeCompilation();
            services.AddControllersWithViews();

            services.AddTransient<JsonReturn>();
            services.AddTransient<GrocerykitService>();
            services.AddTransient<RoleService>();
            services.AddTransient<ItemService>();
            services.AddTransient<PackageService>();
            services.AddTransient<DonorService>();
            services.AddTransient<BeneficiarySaveAsDraftService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseRouting();
            app.UseCors();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DonorRequestHub>("/DonorRequestHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
