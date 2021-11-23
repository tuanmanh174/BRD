using BRD.DataModel;
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

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(@"User Id=hr;Password=hr;Data Source=conn;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Account>(entity =>
            //{
            //    entity.HasKey(e => e.Deptno)
            //        .HasName("SYS_C00220618");

            //    entity.ToTable("DEPT", "SCOTT");

            //    entity.Property(e => e.Deptno).HasColumnName("DEPTNO");

            //    entity.Property(e => e.Dname)
            //        .HasColumnName("DNAME")
            //        .HasColumnType("varchar2")
            //        .HasMaxLength(14);

            //    entity.Property(e => e.Loc)
            //        .HasColumnName("LOC")
            //        .HasColumnType("varchar2")
            //        .HasMaxLength(13);
            //});

            //modelBuilder.Entity<COUNTRIES>(entity =>
            //{
            //    entity.HasKey(e => e.COUNTRY_ID)
            //        .HasName("AR");

            //    entity.ToTable("COUNTRIES");
            //});



        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Countries> COUNTRIES  { get; set; }
    }
}
