using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EIRS.Admin.Models
{
    public class EirsDbContext :DbContext
    {
        public EirsDbContext()
        {
        }

        public EirsDbContext(DbContextOptions<EirsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AssessmentAndItemRollOver> AssessmentAndItemRollOver { get; set; }
        public virtual DbSet<AssessmentRuleRollover> AssessmentRuleRollover { get; set; }
        public virtual DbSet<TempAssHolder> TempAssHolder { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string con = ConfigurationManager.ConnectionStrings["DbEntities"].ConnectionString;

                optionsBuilder.UseSqlServer(con);
            }
        }

    }
}