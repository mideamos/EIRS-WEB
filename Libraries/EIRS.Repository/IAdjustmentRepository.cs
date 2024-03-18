using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EIRS.Repository.AdjustmentRepository;

namespace EIRS.Repository
{
    public interface IAdjustmentRepository
    {
        List<AdjustmentResponse> GetAdjustmentResponse();
        List<AdjustmentResponse> GetAdjustmentResponse(List<long> aaiid);
        List<AdjustmentResponse> GetLateChargeResponse();
        List<AdjustmentResponse> GetLateChargeResponse(List<long> aaiid);
        List<AdjustmentResponse> GetAdjustmentServiceResponse();
        List<AdjustmentResponse> GetLateChargeServiceResponse();
        List<long> GetListOfItemId(int assId);
        List<AiidHolder> GetListOfItemId(List<long> assId);
        List<long?> GetListOfServiceItemId(int assId);
        List<long?> GetListOfServiceItemId(List<long> assId);
    }

    public class AdjustmentResponse
    {
        public decimal? TotalAmount { get; set; }
        public long? AAIID { get; set; }
        public long? SBIID { get; set; }
    }
}
