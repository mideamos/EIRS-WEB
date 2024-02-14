using System;
using System.Configuration;
using EIRS.Admin.Models;
using EIRS.BOL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EIRS.Admin.Models
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

        public virtual DbSet<TaxOffice> TaxOffices { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<ZoneLga> ZoneLgas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string con = ConfigurationManager.ConnectionStrings["DbEntities"].ConnectionString;
                optionsBuilder.UseSqlServer(con);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxOffice>(entity =>
                {
                    entity.ToTable("Tax_Offices");

                    entity.Property(e => e.TaxOfficeId).HasColumnName("TaxOfficeID");

                    entity.Property(e => e.AddressTypeId).HasColumnName("AddressTypeID");

                    entity.Property(e => e.BuildingId).HasColumnName("BuildingID");

                    entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                    entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                    entity.Property(e => e.TaxOfficeName)
                            .HasMaxLength(250)
                            .IsUnicode(false);
                });
            modelBuilder.Entity<Zone>(entity =>
            {
                entity.HasKey(e => e.ZoneCode);

                entity.ToTable("Zone");

                entity.Property(e => e.ZoneCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                                .HasColumnType("datetime")
                                .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBY).HasColumnName("ModifiedBY");

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
