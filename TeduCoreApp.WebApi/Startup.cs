using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using TeduCoreApp.Application.AutoMapper;
using TeduCoreApp.Application.Implementation;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Data.EF;
using TeduCoreApp.Infrastructure.Interfaces;

namespace TeduCoreApp.WebApi
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
            services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("AppDbConnection"),
                       b => b.MigrationsAssembly("TeduCore.Data.EF")));

            services.AddCors(o => o.AddPolicy("TeduCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddSingleton(AutoMapperConfig.RegisterMappings().CreateMapper());

            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));

            services.AddTransient<IProductCategoryService, ProductCategoryService>();

            services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                     options.JsonSerializerOptions.PropertyNamingPolicy = null;
                 });

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TEDU Project",
                    Description = "TEDU API Swagger surface",
                    Contact = new OpenApiContact
                    {
                        Name = "ToanBN",
                        Email = "tedu.international@gmail.com",
                        Url = new Uri("http://www.tedu.com.vn")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/teduinternational/teducoreapp")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == Environments.Development)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCors("TeduCorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Project API v1.1");
            });

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    "default",
                   "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}