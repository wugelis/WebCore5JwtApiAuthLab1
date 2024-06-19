using EasyArchitect.OutsideApiControllerBase;
using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using EasyArchitect.OutsideManaged.Configuration;
using EasyArchitect.OutsideManaged.JWTAuthMiddlewares;
using EasyArchitectCore.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebCore5ApiLab1.Configuration;

namespace WebCore5ApiLab1
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
            services.AddCors();
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddAuthentication(configure =>
            {
                configure.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                configure.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                //options.LoginPath = new PathString(EasyArchitectCore.Core.ConfigurationManager.AppSettings["LoginPage"]);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
                options.Cookie.HttpOnly = true;
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToReturnUrl = async (context) =>
                    {
                        context.HttpContext.Response.Cookies.Delete(UserInfo.LOGIN_USER_INFO);
                    }
                };
            });

            services.AddSwaggerGen(c =>
            {
                foreach (FieldInfo field in typeof(ApiVersionInfo).GetFields())
                {
                    c.SwaggerDoc(
                    field.Name,
                    new OpenApiInfo
                    {
                        Title = $"Outside 外部人員權限模組 API 版本={field.Name} 描述 V1",
                        Version = field.Name
                    });
                }

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            // 註冊 AppSettings Configuration 類型，可在類別中注入 IOptions<AppSettings>
            IConfigurationSection appSettingRoot = Configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingRoot);

            services.AddDbContext<ModelContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("OutsideDbContext"));
                //options.UseOracle(Configuration.GetConnectionString("OutsideDbContext"), oraOptions => oraOptions.UseOracleSQLCompatibility("11"));
            });

            services.AddScoped<ModelContext>();

            services.AddScoped<IUserService, UserService>(x => new UserService(
                new AppSettings() 
                { 
                    Secret = appSettingRoot.GetSection("Secret").Value,
                    TimeoutMinutes = Convert.ToInt32(appSettingRoot.GetSection("TimeoutMinutes").Value)
                }, x.GetRequiredService<ModelContext>()));

            services.AddScoped<IUriExtensions, UriExtensions>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    foreach (FieldInfo field in typeof(ApiVersionInfo).GetFields())
                    {
                        c.SwaggerEndpoint($"/swagger/{field.Name}/swagger.json", $"{field.Name}");
                    }
                }
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherForecast v1")
                );
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseJwtAuthenticate();

            app.UseEndpoints(endpoints =>
            {
                 endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

               endpoints.MapControllers();

            });
        }
    }
}
