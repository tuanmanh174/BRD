using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.DataAccess
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            //this.ChangeTracker.LazyLoadingEnabled = false;
        }

        //public Context()
        //{

        //    this.ChangeTracker.LazyLoadingEnabled = false;

        //}



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseOracle(@"User Id=hr;Password=hr;Data Source=orcl;");
            }
                
        }


        public DbSet<Countries> COUNTRIES { get; set; }

        public DbSet<ACCOUNT> ACCOUNT { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
                modelBuilder.HasDefaultSchema("orcl");

        }

       
    }
}
