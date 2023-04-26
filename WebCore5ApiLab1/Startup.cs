using EasyArchitect.OutsideApiControllerBase;
using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using EasyArchitect.OutsideManaged.Configuration;
using EasyArchitect.OutsideManaged.JWTAuthMiddlewares;
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
using Microsoft.OpenApi.Models;
using Mxic.FrameworkCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddControllers();
            services.AddAuthentication(configure =>
            {
                configure.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
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
                c.SwaggerDoc(
                    "v1", 
                    new OpenApiInfo 
                    { 
                        Title = "WeatherForecast", 
                        Version = "v1" 
                    });
            });

            // 註冊 AppSettings Configuration 類型，可在類別中注入 IOptions<AppSettings>
            IConfigurationSection appSettingRoot = Configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingRoot);

            services.AddDbContext<ModelContext>(options =>
            {
                options.UseOracle(Configuration.GetConnectionString("OutsideDbContext"), oraOptions => oraOptions.UseOracleSQLCompatibility("11"));
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherForecast v1")
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //app.UseMiddleware<JwtMiddleware>();
            app.UseAuthentication();
            app.UseJwtAuthenticate();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
