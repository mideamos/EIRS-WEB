using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLIndividual
    {
        IIndividualRepository _IndividualRepository;
        IUtility _utility;
        public BLIndividual()
        {
            _IndividualRepository = new IndividualRepository();
            _utility = new Utility();
        }

        public IList<usp_GetIndividualList_Result> BL_GetIndividualList(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_GetIndividualList(pObjIndividual);
        }

        public IList<vw_Individual> BL_GetIndividualList()
        {
            return _IndividualRepository.REP_GetIndividualList();
        }

        public FuncResponse<Individual> BL_InsertUpdateIndividual(Individual pObjIndividual, bool pblnSendNotification = true, bool pblnSkipNoValidation = false)
        {
            FuncResponse<Individual> mObjFuncResponse = _IndividualRepository.REP_InsertUpdateIndividual(pObjIndividual, pblnSkipNoValidation);

            if(pObjIndividual.IndividualID == 0 && mObjFuncResponse.Success && GlobalDefaultValues.SendNotification && pblnSendNotification)
            {
                //Send Notification
                EmailDetails mObjEmailDetails = new EmailDetails()
                {
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    TaxPayerTypeName = "Individual",
                    TaxPayerID = mObjFuncResponse.AdditionalData.IndividualID,
                    TaxPayerName = mObjFuncResponse.AdditionalData.FirstName + " " + mObjFuncResponse.AdditionalData.LastName,
                    TaxPayerRIN = mObjFuncResponse.AdditionalData.IndividualRIN,
                    TaxPayerMobileNumber = mObjFuncResponse.AdditionalData.MobileNumber1,
                    TaxPayerEmail = mObjFuncResponse.AdditionalData.EmailAddress1,
                    ContactAddress = mObjFuncResponse.AdditionalData.ContactAddress,
                    TaxPayerTIN = mObjFuncResponse.AdditionalData.TIN,
                    LoggedInUserID = pObjIndividual.CreatedBy,
                };

                if (!string.IsNullOrWhiteSpace(mObjFuncResponse.AdditionalData.EmailAddress1))
                {
                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                }

                //if (!string.IsNullOrWhiteSpace(mObjFuncResponse.AdditionalData.MobileNumber1))
                //{
                //   _utility.BL_TaxPayerCreated(mObjEmailDetails);
                //}
            }

            return mObjFuncResponse;
        }

        public usp_GetIndividualList_Result BL_GetIndividualDetails(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_GetIndividualDetails(pObjIndividual);
        }

        public FuncResponse BL_UpdateStatus(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_UpdateStatus(pObjIndividual);
        }

        public IList<usp_GetIndividualAddressInformation_Result> BL_GetAddressInformation(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_GetAddressInformation(pObjIndividual);
        }

        public FuncResponse BL_InsertAddressInformation(MAP_Individual_AddressInformation pObjAddressInformation)
        {
            return _IndividualRepository.REP_InsertAddressInformation(pObjAddressInformation);
        }

        public FuncResponse<IList<usp_GetIndividualAddressInformation_Result>> BL_RemoveAddressInformation(MAP_Individual_AddressInformation pObjAddressInformation)
        {
            return _IndividualRepository.REP_RemoveAddressInformation(pObjAddressInformation);
        }

        public IList<DropDownListResult> BL_GetIndividualDropDownList(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_GetIndividualDropDownList(pObjIndividual);
        }

        public IList<DropDownListResult> BL_GetIndividualDropDownList(string pStrIndividualName, int pIntTaxOfficeID = 0)
        {
            return _IndividualRepository.REP_GetIndividualDropDownList(pStrIndividualName,pIntTaxOfficeID);
        }

        public FuncResponse<Individual> BL_CheckIndividualLoginDetails(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_CheckIndividualLoginDetails(pObjIndividual);
        }

        public FuncResponse BL_ChangePassword(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_ChangePassword(pObjIndividual);
        }

        public FuncResponse BL_UpdatePassword(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_UpdatePassword(pObjIndividual);
        }

        public void BL_UpdateRegisterationStatus(Individual pObjIndividual)
        {
            _IndividualRepository.REP_UpdateRegisterationStatus(pObjIndividual);
        }

        public void REP_UpdateOTPCode(Individual pObjIndividual)
        {
            _IndividualRepository.REP_UpdateOTPCode(pObjIndividual);
        }

        public FuncResponse REP_CheckOTPCode(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_CheckOTPCode(pObjIndividual);
        }

        public IList<usp_SearchIndividualForRDMLoad_Result> BL_SearchIndividualDetails(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_SearchIndividualDetails(pObjIndividual);
        }

        public IDictionary<string, object> BL_SearchIndividual(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_SearchIndividual(pObjIndividual);
        }

        public FuncResponse BL_UpdateTaxOfficer(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_UpdateTaxOfficer(pObjIndividual);
        }

        public IDictionary<string, object> BL_SearchIndividualForSideMenu(Individual pObjIndividual)
        {
            return _IndividualRepository.REP_SearchIndividualForSideMenu(pObjIndividual);
        }
    }
}
