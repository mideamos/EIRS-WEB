using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IPaymentOptionRepository
    {
        usp_GetPaymentOptionList_Result REP_GetPaymentOptionDetails(Payment_Options pObjPaymentOption);
        IList<DropDownListResult> REP_GetPaymentOptionDropDownList(Payment_Options pObjPaymentOption);
        IList<usp_GetPaymentOptionList_Result> REP_GetPaymentOptionList(Payment_Options pObjPaymentOption);
        FuncResponse REP_InsertUpdatePaymentOption(Payment_Options pObjPaymentOption);
        FuncResponse REP_UpdateStatus(Payment_Options pObjPaymentOption);
    }
}