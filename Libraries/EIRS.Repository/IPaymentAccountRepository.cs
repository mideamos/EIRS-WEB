using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;

namespace EIRS.Repository
{
    public interface IPaymentAccountRepository
    {
        FuncResponse<Payment_Account> REP_InsertUpdatePaymentAccount(Payment_Account pObjPaymentAccount);
        FuncResponse REP_UpdatePaymentAccountFromRDM(Payment_Account pObjPaymentAccount);

        IList<vw_PaymentAccount> REP_PaymentAccountList();

        IList<usp_GetPaymentAccountList_Result> REP_GetPaymentAccountList(Payment_Account pObjPaymentAccount);

        usp_GetPaymentAccountList_Result REP_GetPaymentAccountDetails(Payment_Account pObjPaymentAccount);

        FuncResponse REP_InsertPaymentOperation(MAP_PaymentAccount_Operation pObjPaymentAccount);

        decimal REP_GetWalletBalance(int pIntTaxPayerTypeID, int pIntTaxPayerID);

        IList<vw_PaymentAccountOperation> REP_GetPaymentAccountOperationList();
        IDictionary<string, object> REP_SearchPaymentAccount(Payment_Account pObjPaymentAccount);
    }
}