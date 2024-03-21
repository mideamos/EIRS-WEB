using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class ModelFromController
    {
    }
    public class MyClass
    {
        public long AAIID { get; set; }
        public decimal SettlementAmount { get; set; }
        public decimal SeetlementAmount { get; set; }
        public decimal Assessed_amount { get; set; }
    }
    public class MyClassII
    {
        public decimal TotalAssessed { get; set; }
    }
    public class MyClassIII
    {
        public decimal settled_amount { get; set; }
    } 
    public class BalanceHolder
    {
        public decimal Balance { get; set; }
    }
}