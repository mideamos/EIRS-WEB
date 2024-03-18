using EIRS.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EIRS.Repository.AdjustmentRepository;

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
                                                    TotalAmount = cl.Sum(c => c.Amount)
                                                }).ToList();
                return result;
            }
        }
        public List<AdjustmentResponse> GetAdjustmentResponse(List<long> aaiid)
        {
            using (_db = new EIRSEntities())
            {
                var ret = _db.MAP_Assessment_Adjustment.Where(o => aaiid.Contains(o.AAIID.Value)).ToList();
                List<AdjustmentResponse> result = ret.GroupBy(l => l.AAIID)
                                                .Select(cl => new AdjustmentResponse
                                                {
                                                    AAIID = cl.First().AAIID,
                                                    TotalAmount = cl.Sum(c => c.Amount)
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
        public class AiidHolder
        {
            public long AAIID { get; set; }
            public long BillId { get; set; }
        }

        public List<AiidHolder> GetListOfItemId(List<long> assId)
        {
            using (_db = new EIRSEntities())
            {
                // var r = _db.MAP_Assessment_AssessmentItem.Where(o => assId.Contains(o.AAIID)).ToList();
                var assItems = (from a in _db.MAP_Assessment_AssessmentItem
                                join b in _db.MAP_Assessment_AssessmentRule.Where(o => assId.Contains(o.AssessmentID.Value))
                on a.AARID equals b.AARID
                                select new AiidHolder { AAIID = a.AAIID, BillId = b.AssessmentID.Value }).ToList();
                return assItems;
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
        public List<long?> GetListOfServiceItemId(List<long> assId)
        {
            var ret = new List<long?>();
            using (_db = new EIRSEntities())
            {
                //r = _db.MAP_ServiceBill_MDAServiceItem.Where(o => assId.Contains(o.SBSIID)).ToList();
                var assItems = (from a in _db.MAP_ServiceBill_MDAServiceItem
                                join b in _db.MAP_ServiceBill_MDAService.Where(o => assId.Contains(o.ServiceBillID.Value))
                                on a.SBSID equals b.SBSID
                                select a.SBSID).ToList();
                return assItems;
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
                       TotalAmount = cl.Sum(c => c.TotalAmount)
                   }).ToList();
                return result;
            }
        }
        public List<AdjustmentResponse> GetLateChargeResponse(List<long> aaiid)
        {
            using (_db = new EIRSEntities())
            {
                var ret = _db.MAP_Assessment_LateCharge.Where(o => aaiid.Contains(o.AAIID.Value)).ToList();

                List<AdjustmentResponse> result = ret.GroupBy(l => l.AAIID)
                   .Select(cl => new AdjustmentResponse
                   {
                       AAIID = cl.First().AAIID,
                       TotalAmount = cl.Sum(c => c.TotalAmount)
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
