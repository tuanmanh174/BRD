using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BRD.DataAccess.Model
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        { }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder
                //            .UseLoggerFactory(ConsoleLoggerFactory)
                //            .UseOracle("connection string to test db.");
            }
        }

        private int GetSequence(string sequenceName)
        {
            long result = -1;

            var connection = Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT nextval('\"{sequenceName}\"');";
                var obj = cmd.ExecuteScalar();
                result = (long)obj;
            }

            return (int)result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed
            //modelBuilder.Seed();
        }
    }
}
