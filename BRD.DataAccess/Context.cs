using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.DataAccess
{
    public class Context : DbContext
    {
        public Context()
        { }
        public Context(DbContextOptions<Context> options) : base(options)
        { }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseOracle(@"User Id=hr;Password=hr;Data Source=orcl;");
                //optionsBuilder.UseOracle(@"Data Source =  (DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = orcl)));User ID=hr;Password=hr;");
                //optionsBuilder.UseOracle(@"Data Source =  (DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = orcl)));User ID=hr;Password=hr;");
            }

        }


        public virtual DbSet<Countries> COUNTRIES { get; set; }

        public virtual DbSet<Accounts> accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("orcl");

            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasIndex(e => e.id);
                entity.Property(e => e.accountname).HasMaxLength(20);
            });

        }


    }
}
