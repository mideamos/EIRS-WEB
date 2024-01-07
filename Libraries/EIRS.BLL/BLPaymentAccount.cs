using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLPaymentAccount
    {
        IPaymentAccountRepository _PaymentAccountRepository;

        public BLPaymentAccount()
        {
            _PaymentAccountRepository = new PaymentAccountRepository();
        }

        public FuncResponse<Payment_Account> BL_InsertUpdatePaymentAccount(Payment_Account pObjPaymentAccount)
        {
            FuncResponse<Payment_Account> mObjFuncResponse = _PaymentAccountRepository.REP_InsertUpdatePaymentAccount(pObjPaymentAccount);

            if (pObjPaymentAccount.PaymentAccountID == 0 && mObjFuncResponse.Success && GlobalDefaultValues.SendNotification)
            {
                string mStrTaxPayerRIN = "", mStrTaxPayerName = "", mStrTaxPayerType = "", mStrTaxPayerMobile = "", mStrTaxPayerEmail = "";

                if (pObjPaymentAccount.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = pObjPaymentAccount.TaxPayerID.GetValueOrDefault() });

                    if (mObjIndividualData != null)
                    {
                        mStrTaxPayerRIN = mObjIndividualData.IndividualRIN;
                        mStrTaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName;
                        mStrTaxPayerType = mObjIndividualData.TaxPayerTypeName;
                        mStrTaxPayerEmail = mObjIndividualData.EmailAddress1;
                        mStrTaxPayerMobile = mObjIndividualData.MobileNumber1;
                    }

                }
                else if (pObjPaymentAccount.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = pObjPaymentAccount.TaxPayerID.GetValueOrDefault() });

                    if (mObjCompanyData != null)
                    {
                        mStrTaxPayerRIN = mObjCompanyData.CompanyRIN;
                        mStrTaxPayerName = mObjCompanyData.CompanyName;
                        mStrTaxPayerType = mObjCompanyData.TaxPayerTypeName;
                        mStrTaxPayerEmail = mObjCompanyData.EmailAddress1;
                        mStrTaxPayerMobile = mObjCompanyData.MobileNumber1;
                    }
                }
                else if (pObjPaymentAccount.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = pObjPaymentAccount.TaxPayerID.GetValueOrDefault() });

                    if (mObjGovernmentData != null)
                    {
                        mStrTaxPayerRIN = mObjGovernmentData.GovernmentRIN;
                        mStrTaxPayerName = mObjGovernmentData.GovernmentName;
                        mStrTaxPayerType = mObjGovernmentData.TaxPayerTypeName;
                        mStrTaxPayerEmail = mObjGovernmentData.ContactEmail;
                        mStrTaxPayerMobile = mObjGovernmentData.ContactNumber;
                    }
                }
                else if (pObjPaymentAccount.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = pObjPaymentAccount.TaxPayerID.GetValueOrDefault() });

                    if (mObjSpecialData != null)
                    {
                        mStrTaxPayerRIN = mObjSpecialData.SpecialRIN;
                        mStrTaxPayerName = mObjSpecialData.SpecialTaxPayerName;
                        mStrTaxPayerType = mObjSpecialData.TaxPayerTypeName;
                        mStrTaxPayerEmail = mObjSpecialData.ContactEmail;
                        mStrTaxPayerMobile = mObjSpecialData.ContactNumber;
                    }
                }

                //Send Notification
                EmailDetails mObjEmailDetails = new EmailDetails()
                {
                    TaxPayerName = mStrTaxPayerName,
                    TaxPayerRIN = mStrTaxPayerRIN,
                    TaxPayerTypeName = mStrTaxPayerType,
                    TaxPayerID = pObjPaymentAccount.TaxPayerID.GetValueOrDefault(),
                    TaxPayerTypeID = pObjPaymentAccount.TaxPayerTypeID.GetValueOrDefault(),
                    ReceivedAmount = CommUtil.GetFormatedCurrency(pObjPaymentAccount.Amount),
                    PoARefNo = mObjFuncResponse.AdditionalData.PaymentRefNo,
                    LoggedInUserID = pObjPaymentAccount.CreatedBy,
                    TaxPayerEmail = mStrTaxPayerMobile,
                    TaxPayerMobileNumber = mStrTaxPayerMobile
                };

                if (!string.IsNullOrWhiteSpace(mStrTaxPayerEmail))
                {
                    BLEmailHandler.BL_PaymentonAccount(mObjEmailDetails);
                }

                if (!string.IsNullOrWhiteSpace(mStrTaxPayerMobile))
                {
                    BLSMSHandler.BL_PaymentonAccount(mObjEmailDetails);
                }
            }

            return mObjFuncResponse;
        }

        public FuncResponse BL_UpdatePaymentAccountFromRDM(Payment_Account pObjPaymentAccount)
        {
            return _PaymentAccountRepository.REP_UpdatePaymentAccountFromRDM(pObjPaymentAccount);
        }

        public IList<vw_PaymentAccount> BL_PaymentAccountList()
        {
            return _PaymentAccountRepository.REP_PaymentAccountList();
        }

        public IList<usp_GetPaymentAccountList_Result> BL_GetPaymentAccountList(Payment_Account pObjPaymentAccount)
        {
            return _PaymentAccountRepository.REP_GetPaymentAccountList(pObjPaymentAccount);
        }

        public usp_GetPaymentAccountList_Result BL_GetPaymentAccountDetails(Payment_Account pObjPaymentAccount)
        {
            return _PaymentAccountRepository.REP_GetPaymentAccountDetails(pObjPaymentAccount);
        }


        public FuncResponse BL_InsertPaymentOperation(MAP_PaymentAccount_Operation pObjPaymentAccount)
        {
            return _PaymentAccountRepository.REP_InsertPaymentOperation(pObjPaymentAccount);
        }

        public decimal BL_GetWalletBalance(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _PaymentAccountRepository.REP_GetWalletBalance(pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<vw_PaymentAccountOperation> BL_GetPaymentAccountOperationList()
        {
            return _PaymentAccountRepository.REP_GetPaymentAccountOperationList();
        }

        public IDictionary<string, object> BL_SearchPaymentAccount(Payment_Account pObjPaymentAccount)
        {
            return _PaymentAccountRepository.REP_SearchPaymentAccount(pObjPaymentAccount);
        }
    }
}
