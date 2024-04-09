using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EIRS.Web.GISModels
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

        public virtual DbSet<Gistesting> Gistesting { get; set; }
        public virtual DbSet<GISFileAssessmentItem> GISFileAssessmentItem { get; set; }
        public virtual DbSet<GISFileInvoiceItem> GISFileInvoiceItem { get; set; }
        public virtual DbSet<GISFileInvoice> GISFileInvoice { get; set; }
        public virtual DbSet<GISFileAssessment> GISFileAssessment { get; set; }
        public virtual DbSet<GISFileAsset> GISFileAsset { get; set; }
        public virtual DbSet<GISFileParty> GISFileParty { get; set; }
        public DbSet<usp_GetAssessmentForPendingOrDeclined_Result> usp_GetAssessmentForPendingOrDeclined_Results { get; set; }
        public virtual DbSet<GISFileHolder> GISFileHolder { get; set; }
        public IList<usp_GetAssessmentForPendingOrDeclined_Result> usp_GetAssessmentForPendingOrDeclined()
        {
            return this.Set<usp_GetAssessmentForPendingOrDeclined_Result>().FromSqlRaw("EXECUTE usp_GetAssessmentForPendingOrDeclined").ToList();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //  string cs = ConfigurationManager.ConnectionStrings["EIRSEntities"].ConnectionString;
                //optionsBuilder.UseSqlServer(cs);
                string con = ConfigurationManager.ConnectionStrings["DbEntities"].ConnectionString;

                optionsBuilder.UseSqlServer(con);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gistesting>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GISTesting");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
