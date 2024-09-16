﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EIRS.BOL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ERASDWEntities : DbContext
    {
        public ERASDWEntities()
            : base("name=ERASDWEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PAYEInput> PAYEInputs { get; set; }
        public virtual DbSet<PAYEOutput> PAYEOutputs { get; set; }
        public virtual DbSet<vw_PAYEInputList> vw_PAYEInputList { get; set; }
        public virtual DbSet<vw_PAYEOutput> vw_PAYEOutput { get; set; }
        public virtual DbSet<vw_DAInputList> vw_DAInputList { get; set; }
        public virtual DbSet<vw_DAOutputList> vw_DAOutputList { get; set; }
        public virtual DbSet<DAInput> DAInputs { get; set; }
        public virtual DbSet<DAOutput> DAOutputs { get; set; }
    
        public virtual ObjectResult<usp_GetPAYEInputList_Result> usp_GetPAYEInputList(Nullable<int> pAYEInputID, string employer_RIN)
        {
            var pAYEInputIDParameter = pAYEInputID.HasValue ?
                new ObjectParameter("PAYEInputID", pAYEInputID) :
                new ObjectParameter("PAYEInputID", typeof(int));
    
            var employer_RINParameter = employer_RIN != null ?
                new ObjectParameter("Employer_RIN", employer_RIN) :
                new ObjectParameter("Employer_RIN", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetPAYEInputList_Result>("usp_GetPAYEInputList", pAYEInputIDParameter, employer_RINParameter);
        }
    
        public virtual ObjectResult<usp_GETPAYEOutput_Result> usp_GETPAYEOutput(Nullable<int> payeOutputID, string employer_RIN)
        {
            var payeOutputIDParameter = payeOutputID.HasValue ?
                new ObjectParameter("PayeOutputID", payeOutputID) :
                new ObjectParameter("PayeOutputID", typeof(int));
    
            var employer_RINParameter = employer_RIN != null ?
                new ObjectParameter("Employer_RIN", employer_RIN) :
                new ObjectParameter("Employer_RIN", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GETPAYEOutput_Result>("usp_GETPAYEOutput", payeOutputIDParameter, employer_RINParameter);
        }
    
        public virtual ObjectResult<usp_GetDAInputList_Result> usp_GetDAInputList(Nullable<int> dAInputID, string taxpayer_RIN)
        {
            var dAInputIDParameter = dAInputID.HasValue ?
                new ObjectParameter("DAInputID", dAInputID) :
                new ObjectParameter("DAInputID", typeof(int));
    
            var taxpayer_RINParameter = taxpayer_RIN != null ?
                new ObjectParameter("Taxpayer_RIN", taxpayer_RIN) :
                new ObjectParameter("Taxpayer_RIN", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetDAInputList_Result>("usp_GetDAInputList", dAInputIDParameter, taxpayer_RINParameter);
        }
    
        public virtual ObjectResult<usp_GetDAOutputList_Result> usp_GetDAOutputList(Nullable<int> dAOutputID, string taxpayer_RIN)
        {
            var dAOutputIDParameter = dAOutputID.HasValue ?
                new ObjectParameter("DAOutputID", dAOutputID) :
                new ObjectParameter("DAOutputID", typeof(int));
    
            var taxpayer_RINParameter = taxpayer_RIN != null ?
                new ObjectParameter("Taxpayer_RIN", taxpayer_RIN) :
                new ObjectParameter("Taxpayer_RIN", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetDAOutputList_Result>("usp_GetDAOutputList", dAOutputIDParameter, taxpayer_RINParameter);
        }
    
        public virtual ObjectResult<usp_RPT_PAYEOutputAggregationList_Result> usp_RPT_PAYEOutputAggregationList(string employerRIN, Nullable<int> taxYear, Nullable<int> taxOfficeID)
        {
            var employerRINParameter = employerRIN != null ?
                new ObjectParameter("EmployerRIN", employerRIN) :
                new ObjectParameter("EmployerRIN", typeof(string));
    
            var taxYearParameter = taxYear.HasValue ?
                new ObjectParameter("TaxYear", taxYear) :
                new ObjectParameter("TaxYear", typeof(int));
    
            var taxOfficeIDParameter = taxOfficeID.HasValue ?
                new ObjectParameter("TaxOfficeID", taxOfficeID) :
                new ObjectParameter("TaxOfficeID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_RPT_PAYEOutputAggregationList_Result>("usp_RPT_PAYEOutputAggregationList", employerRINParameter, taxYearParameter, taxOfficeIDParameter);
        }
    
        public virtual ObjectResult<usp_RPT_PAYEOutputAggregationSummary_Result> usp_RPT_PAYEOutputAggregationSummary(Nullable<int> taxYear, Nullable<int> taxOfficeID)
        {
            var taxYearParameter = taxYear.HasValue ?
                new ObjectParameter("TaxYear", taxYear) :
                new ObjectParameter("TaxYear", typeof(int));
    
            var taxOfficeIDParameter = taxOfficeID.HasValue ?
                new ObjectParameter("TaxOfficeID", taxOfficeID) :
                new ObjectParameter("TaxOfficeID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_RPT_PAYEOutputAggregationSummary_Result>("usp_RPT_PAYEOutputAggregationSummary", taxYearParameter, taxOfficeIDParameter);
        }
    }
}
