using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using EIRS.BOL;
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
        public virtual DbSet<GISFileHolder> GISFileHolder { get; set; }
        public virtual DbSet<RevenueStreamResult> RevenueStreamResults { get; set; }
        public virtual DbSet<usp_RPT_All_TaxOffices_Performance_ByMonth_Result> uspRPTAllTaxOfficesPerformanceByMonths { get; set; }
        public virtual DbSet<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamResult> uspRPTTaxOfficePerformanceByAllRevenueStreams { get; set; }
        public virtual DbSet<RevenueStreamResultDrillDown> RevenueStreamResultDrillDowns { get; set; }
        public virtual DbSet<usp_RPT_All_TaxOffices_Performance_byRevenueStreamdrilldown> AllTaxOfficesPerformancebyRevenueStreamdrilldowns { get; set; }
        public virtual DbSet<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown> TaxOfficesPerformancebyAllRevenueStreamdrilldowns { get; set; }

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
        //    public  List<RevenueStreamResult> GetRevenueStreamForAllPurposeAsync(int revenueStreamID, int year, int month, int taxOfficeID)
        //    {
        //        //    var parameters = new[]
        //        //    {
        //        //    new SqlParameter("@RevenueStreamID", revenueStreamID),
        //        //    new SqlParameter("@Year", year),
        //        //    new SqlParameter("@Month", month),
        //        //    new SqlParameter("@TaxOfficeID", taxOfficeID)
        //        //};

        //        var parameters = new[]
        //{
        //    new SqlParameter("@RevenueStreamID", SqlDbType.Int) { Value = revenueStreamID },
        //    new SqlParameter("@Year", SqlDbType.Int) { Value = year },
        //    new SqlParameter("@Month", SqlDbType.Int) { Value = month },
        //    new SqlParameter("@TaxOfficeID", SqlDbType.Int) { Value = taxOfficeID }
        //};
        //        return RevenueStreamResults.FromSqlRaw("[dbo].[usp_RPT_RevenueStreamForAllPurpose] @RevenueStreamID, @Year, @Month, @TaxOfficeID", parameters).ToList();
        //    }
        public virtual ObjectResult<RevenueStreamResult> usp_GetPAYERevenueStreamBillByTaxOffice(Nullable<int> RevenueStreamID, Nullable<int> Year, Nullable<int> Month, Nullable<int> TaxOfficeID)
        {
            var TaxOfficeIDParameter = TaxOfficeID.HasValue ?
                new ObjectParameter("TaxOfficeID", TaxOfficeID) :
                new ObjectParameter("TaxOfficeID", typeof(int));
            var RevenueStreamIDParameter = RevenueStreamID.HasValue ?
                new ObjectParameter("RevenueStreamID", RevenueStreamID) :
                new ObjectParameter("RevenueStreamID", typeof(int));
            var YearParameter = Year.HasValue ?
                new ObjectParameter("Year", Year) :
                new ObjectParameter("Year", typeof(int));

            var MonthParameter = Month.HasValue ?
                new ObjectParameter("Month", Month) :
                new ObjectParameter("Month", typeof(int));



            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RevenueStreamResult>("usp_GetPAYERevenueStreamBillByTaxOffice", TaxOfficeIDParameter, RevenueStreamIDParameter, YearParameter, MonthParameter);
        }
        public virtual IQueryable<RevenueStreamResult> usp_RPT_RevenueStreamForAllPurpose(int revenueStreamID, int year, int month, int taxOfficeID)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@RevenueStreamID", revenueStreamID));
            parameters.Add(new SqlParameter("@Year", year));
            parameters.Add(new SqlParameter("@Month", month));
            parameters.Add(new SqlParameter("@TaxOfficeID", taxOfficeID));

            var r = RevenueStreamResults.FromSqlRaw("usp_RPT_RevenueStreamForAllPurpose @RevenueStreamID, @Year, @Month, @TaxOfficeID", parameters: parameters.ToArray());
            return r;
        }
        
        public virtual IQueryable<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamResult> usp_RPT_TaxOffice_Performance_ByAllRevenueStream(int year, int month, int taxOfficeID)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Year", year));
            parameters.Add(new SqlParameter("@Month", month));
            parameters.Add(new SqlParameter("@TaxOfficeID", taxOfficeID));

            var r = uspRPTTaxOfficePerformanceByAllRevenueStreams.FromSqlRaw("usp_RPT_TaxOffice_Performance_ByAllRevenueStream @Year, @Month, @TaxOfficeID", parameters: parameters.ToArray());
            return r;
        }
        
        public virtual IQueryable<usp_RPT_All_TaxOffices_Performance_ByMonth_Result> usp_RPT_All_TaxOffices_Performance_ByMonth(int year, int month)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Year", year));
            parameters.Add(new SqlParameter("@Month", month));

            var r = uspRPTAllTaxOfficesPerformanceByMonths.FromSqlRaw("usp_RPT_All_TaxOffices_Performance_ByMonth @Year, @Month ", parameters: parameters.ToArray());
            return r;
        }

        public virtual IQueryable<RevenueStreamResultDrillDown> usp_RPT_TaxOffice_Performance_byRevenueStreamdrilldown(int? revenueStreamID, int? year, int? month, int? taxOfficeID)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@RevenueStreamID", revenueStreamID));
            parameters.Add(new SqlParameter("@Year", year));
            parameters.Add(new SqlParameter("@Month", month));
            parameters.Add(new SqlParameter("@TaxOfficeID", taxOfficeID));

            var r = RevenueStreamResultDrillDowns.FromSqlRaw("usp_RPT_TaxOffice_Performance_byRevenueStreamdrilldown @RevenueStreamID, @Year, @Month, @TaxOfficeID", parameters: parameters.ToArray());
            return r;
        }

        public virtual IQueryable<usp_RPT_All_TaxOffices_Performance_byRevenueStreamdrilldown> usp_RPT_All_TaxOffices_Performance_byRevenueStreamdrilldown(int revenueStreamID, int year, int month)
        {
            return AllTaxOfficesPerformancebyRevenueStreamdrilldowns.FromSqlRaw(
                "EXEC usp_RPT_All_TaxOffices_Performance_byRevenueStreamdrilldown @RevenueStreamID, @Year, @Month",
                new SqlParameter("@RevenueStreamID", revenueStreamID),
                new SqlParameter("@Year", year),
                new SqlParameter("@Month", month)
            );
        }

        public virtual IQueryable<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown> usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown(int year, int month, int taxOfficeID, int revenueStreamID)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Year", year));
            parameters.Add(new SqlParameter("@Month", month));
            parameters.Add(new SqlParameter("@TaxOfficeID", taxOfficeID));
            parameters.Add(new SqlParameter("@RevenueStreamID", revenueStreamID));

            var r = TaxOfficesPerformancebyAllRevenueStreamdrilldowns.FromSqlRaw("usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown @Year, @Month, @TaxOfficeID, @RevenueStreamID", parameters: parameters.ToArray());
            return r;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RevenueStreamResult>().HasNoKey();
            modelBuilder.Entity<RevenueStreamResultDrillDown>().HasNoKey();
            modelBuilder.Entity<usp_RPT_All_TaxOffices_Performance_ByMonth_Result>().HasNoKey();
            modelBuilder.Entity<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown>().HasNoKey();
            modelBuilder.Entity<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamResult>().HasNoKey();

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
