using System;
using System.Data;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EIRS.BOL.Models
{
    public partial class EIRSContext : DbContext
    {
        public EIRSContext()
        {
        }
        
        public EIRSContext(DbContextOptions<EIRSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TaxOffice> TaxOffice { get; set; }
        public virtual DbSet<Zone> Zone { get; set; }
        public virtual DbSet<ZoneLga> ZoneLga { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=92.205.57.77;Initial Catalog=EIRS;user id=Admin;password=K5?wh7l4##");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxOffice>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.TaxOfficeId).ValueGeneratedOnAdd();

                entity.Property(e => e.TaxOfficeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZoneCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.HasKey(e => e.ZoneCode);

                entity.Property(e => e.ZoneCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ZoneId).ValueGeneratedOnAdd();

                entity.Property(e => e.ZoneName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ZoneLga>(entity =>
            {
                entity.ToTable("ZoneLGA");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LgaName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZoneCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
