using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLPaymentFrequency
    {
        IPaymentFrequencyRepository _PaymentFrequencyRepository;

        public BLPaymentFrequency()
        {
            _PaymentFrequencyRepository = new PaymentFrequencyRepository();
        }

        public IList<usp_GetPaymentFrequencyList_Result> BL_GetPaymentFrequencyList(Payment_Frequency pObjPaymentFrequency)
        {
            return _PaymentFrequencyRepository.REP_GetPaymentFrequencyList(pObjPaymentFrequency);
        }

        public FuncResponse BL_InsertUpdatePaymentFrequency(Payment_Frequency pObjPaymentFrequency)
        {
            return _PaymentFrequencyRepository.REP_InsertUpdatePaymentFrequency(pObjPaymentFrequency);
        }

        public usp_GetPaymentFrequencyList_Result BL_GetPaymentFrequencyDetails(Payment_Frequency pObjPaymentFrequency)
        {
            return _PaymentFrequencyRepository.REP_GetPaymentFrequencyDetails(pObjPaymentFrequency);
        }

        public IList<DropDownListResult> BL_GetPaymentFrequencyDropDownList(Payment_Frequency pObjPaymentFrequency)
        {
            return _PaymentFrequencyRepository.REP_GetPaymentFrequencyDropDownList(pObjPaymentFrequency);
        }

        public FuncResponse BL_UpdateStatus(Payment_Frequency pObjPaymentFrequency)
        {
            return _PaymentFrequencyRepository.REP_UpdateStatus(pObjPaymentFrequency);
        }
    }
}
