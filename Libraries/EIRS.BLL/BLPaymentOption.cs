using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLPaymentOption
    {
        IPaymentOptionRepository _PaymentOptionRepository;

        public BLPaymentOption()
        {
            _PaymentOptionRepository = new PaymentOptionRepository();
        }

        public IList<usp_GetPaymentOptionList_Result> BL_GetPaymentOptionList(Payment_Options pObjPaymentOption)
        {
            return _PaymentOptionRepository.REP_GetPaymentOptionList(pObjPaymentOption);
        }

        public FuncResponse BL_InsertUpdatePaymentOption(Payment_Options pObjPaymentOption)
        {
            return _PaymentOptionRepository.REP_InsertUpdatePaymentOption(pObjPaymentOption);
        }

        public usp_GetPaymentOptionList_Result BL_GetPaymentOptionDetails(Payment_Options pObjPaymentOption)
        {
            return _PaymentOptionRepository.REP_GetPaymentOptionDetails(pObjPaymentOption);
        }

        public IList<DropDownListResult> BL_GetPaymentOptionDropDownList(Payment_Options pObjPaymentOption)
        {
            return _PaymentOptionRepository.REP_GetPaymentOptionDropDownList(pObjPaymentOption);
        }

        public FuncResponse BL_UpdateStatus(Payment_Options pObjPaymentOption)
        {
            return _PaymentOptionRepository.REP_UpdateStatus(pObjPaymentOption);
        }
    }
}
