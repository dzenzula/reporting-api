using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Proxies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Net.Http.Headers;
using ScalesMWebAPI.Services;
using ScalesMWebAPI.Models;

namespace ScalesMWebAPI
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "TestAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// dotnet ef dbcontext scaffold "Server=KRR-TST-PAHWL02;Database=KRR-PA-MON-SCALES;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models --ContexDir ContextDB
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                
                options.AddPolicy(name: AllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder
                                      .WithOrigins("http://localhost:8080", "https://krr-app-palbp01.europe.mittalco.com", "https://krr-tst-padev02.europe.mittalco.com")
                                      .SetIsOriginAllowed(origin => true)
                                      .WithExposedHeaders()
                                      .AllowAnyMethod()
                                      .WithHeaders("Access-Control-Allow-Origin")
                                      .SetPreflightMaxAge(TimeSpan.FromSeconds(86400)
                                      )
                                          //.WithHeaders("Vary: Origin", "Origin", "X-Requested-With", "Content-Type", "Accept", "Access-Control-Allow-Origin", "Access-Control-Allow-Credentials")
                                          //.WithMethods("GET", "POST", "OPTIONS", "PUT", "DELETE")
                                          ;
                                  });
               
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<KRRPAMONSCALESContext>(
                options =>
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"])
                );
            //services.AddTransient();
            
            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "reporting-api", Version = "v1" });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.AddServer(new OpenApiServer { Url = "http://localhost:63169", Description = "Developer server" });
                //c.AddServer(new OpenApiServer { Url = "https://krr-tst-padev02/ScalesMWebAPI", Description = "Test server" });
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
                /*****************************************************************************************/
                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

               
                /*****************************************************************************************/

            });

            /*****************************************************************************************/
            // Set the comments path for the Swagger JSON and UI.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }
            /*
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<KRRPAMONSCALESContext>();
                context.Database.Migrate();
            }
            */
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("./v1/swagger.json", "reporting-api V1");
                    c.DefaultModelsExpandDepth(-1);

                });



           
            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);
            app.UseAuthentication();
            //app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                await next();
            });
            //app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(AllowSpecificOrigins);
            });
        }

     
    }
}
