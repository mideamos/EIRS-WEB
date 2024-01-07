using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IPaymentFrequencyRepository
    {
        usp_GetPaymentFrequencyList_Result REP_GetPaymentFrequencyDetails(Payment_Frequency pObjPaymentFrequency);
        IList<DropDownListResult> REP_GetPaymentFrequencyDropDownList(Payment_Frequency pObjPaymentFrequency);
        IList<usp_GetPaymentFrequencyList_Result> REP_GetPaymentFrequencyList(Payment_Frequency pObjPaymentFrequency);
        FuncResponse REP_InsertUpdatePaymentFrequency(Payment_Frequency pObjPaymentFrequency);
        FuncResponse REP_UpdateStatus(Payment_Frequency pObjPaymentFrequency);
    }
}