using BRD.DataAccess;
using BRD.Repository;
using BRD.Service;
using Fluent.Infrastructure.FluentModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BRD
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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Place Info Service API",
                    Version = "v2",
                    Description = "Sample service for Learner",
                });
            });

            services.AddHttpContextAccessor();
            services.AddEntityFrameworkOracle()
                .AddDbContext<Context>(builder => builder
                .UseOracle(
                    Configuration["Data:ConnectionStrings:DefaultConnection"],
                    sqlOptions => sqlOptions.CommandTimeout(60).UseRelationalNulls(true).MinBatchSize(2))
                .EnableDetailedErrors(false).EnableSensitiveDataLogging(false)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Scoped);
            OracleConfiguration.StatementCacheSize = 300;
            OracleConfiguration.FetchSize = 300000;
            OracleConfiguration.TraceFileLocation = @"C:\app\oracle\product\19c\db_home1\network\trace";
            OracleConfiguration.TraceLevel = 7;
            OracleConfiguration.TnsAdmin = @"C:\app\oracle\product\19c\db_home1\network\admin";
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //thêm các service business
            services.AddScoped<ICountryRepository, CountryService>();
            services.AddScoped<IAccountRepository, AccountService>();

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());


            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "PlaceInfo Services"));



        }
    }
}
