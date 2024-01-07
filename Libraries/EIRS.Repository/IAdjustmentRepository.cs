using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public interface IAdjustmentRepository
    {
        List<AdjustmentResponse> GetAdjustmentResponse();
        List<AdjustmentResponse> GetLateChargeResponse();
        List<AdjustmentResponse> GetAdjustmentServiceResponse();
        List<AdjustmentResponse> GetLateChargeServiceResponse();
        List<long> GetListOfItemId(int assId);
        List<long?> GetListOfServiceItemId(int assId);
    }

    public class AdjustmentResponse
    {
        public decimal? TotalAmount { get; set; }
        public long? AAIID { get; set; }
        public long? SBIID { get; set; }
    }
}
