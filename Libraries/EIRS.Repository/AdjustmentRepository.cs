using EIRS.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public class AdjustmentRepository : IAdjustmentRepository
    {
        EIRSEntities _db;
        public List<AdjustmentResponse> GetAdjustmentResponse()
        {
            using (_db = new EIRSEntities())
            {
                var ret = _db.MAP_Assessment_Adjustment.ToList();
                List<AdjustmentResponse> result = ret.GroupBy(l => l.AAIID)
                                                                                 .Select(cl => new AdjustmentResponse
                                                                                 {
                                                                                     AAIID = cl.First().AAIID,
                                                                                     TotalAmount = cl.Sum(c => c.Amount),
                                                                                 }).ToList();
                return result;
            }
        }

        public List<long> GetListOfItemId(int assId)
        {
            var ret = new List<long>();
            using (_db = new EIRSEntities())
            {
                var assItems = from a in _db.MAP_Assessment_AssessmentItem
                               join b in _db.MAP_Assessment_AssessmentRule
                               on a.AARID equals b.AARID
                               where b.AssessmentID == assId
                               select a.AAIID;
                ret.AddRange(assItems);
                return ret;
            }
        }    
        public List<AdjustmentResponse> GetAdjustmentServiceResponse()
        {
            using (_db = new EIRSEntities())
            {
                var ret = _db.MAP_ServiceBill_Adjustment.ToList();
                List<AdjustmentResponse> result = ret.GroupBy(l => l.SBSIID)
                                                                                 .Select(cl => new AdjustmentResponse
                                                                                 {
                                                                                     SBIID = cl.First().SBSIID,
                                                                                     TotalAmount = cl.Sum(c => c.Amount),
                                                                                 }).ToList();
                return result;
            }
        }

        public List<long?> GetListOfServiceItemId(int assId)
        {
            var ret = new List<long?>();
            using (_db = new EIRSEntities())
            {
                var assItems = from a in _db.MAP_ServiceBill_MDAServiceItem
                               join b in _db.MAP_ServiceBill_MDAService
                               on a.SBSID equals b.SBSID
                               where b.ServiceBillID == assId
                               select a.SBSID;
                ret.AddRange(assItems);
                return ret;
            }
        }

        public List<AdjustmentResponse> GetLateChargeResponse()
        {
            using (_db = new EIRSEntities())
            {
                var ret = _db.MAP_Assessment_LateCharge.ToList();
                List<AdjustmentResponse> result = ret.GroupBy(l => l.AAIID)
                                                                                 .Select(cl => new AdjustmentResponse
                                                                                 {
                                                                                     AAIID = cl.First().AAIID,
                                                                                     TotalAmount = cl.Sum(c => c.TotalAmount),
                                                                                 }).ToList();
                return result;
            }
        }

        public List<AdjustmentResponse> GetLateChargeServiceResponse()
        {
            using (_db = new EIRSEntities())
            {
                var ret = _db.MAP_ServiceBill_LateCharge.ToList();
                List<AdjustmentResponse> result = ret.GroupBy(l => l.SBSIID)
                                                                                 .Select(cl => new AdjustmentResponse
                                                                                 {
                                                                                     SBIID = cl.First().SBSIID,
                                                                                     TotalAmount = cl.Sum(c => c.TotalAmount),
                                                                                 }).ToList();
                return result;
            }
        }
    }
}
