using BRD.DataAccess.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using BRD.Repository;
using BRD.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BRD.Common.Infrastructure;

namespace BRD.Service.Infrastructure
{
    public class DbHelper
    {
        /// <summary>
        ///  Create and return an UnitOfWork instance
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IUnitOfWork CreateUnitOfWork(IConfiguration configuration, IHttpContextAccessor contextAccessor = null)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();
            options.UseNpgsql(configuration.GetConnectionString(Constants.Settings.DefaultConnection),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(Constants.Settings.BRDDataAccess);
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                    sqlOptions.CommandTimeout(3600);
                });
            options.ConfigureWarnings(
                warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning));

            var context = new AppDbContext(options.Options);
            return new UnitOfWork(context, contextAccessor);
        }

    }
}
