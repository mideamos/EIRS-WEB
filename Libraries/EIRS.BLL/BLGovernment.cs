using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLGovernment
    {
        IGovernmentRepository _GovernmentRepository;
        IUtility _utility;
        public BLGovernment()
        {
            _GovernmentRepository = new GovernmentRepository();
            _utility = new Utility();
        }

        public IList<usp_GetGovernmentList_Result> BL_GetGovernmentList(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_GetGovernmentList(pObjGovernment);
        }

        public IList<vw_Government> BL_GetGovernmentList()
        {
            return _GovernmentRepository.REP_GetGovernmentList();
        }

        public IList<DropDownListResult> BL_GetGovernmentDropDownList(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_GetGovernmentDropDownList(pObjGovernment);
        }

        public IList<DropDownListResult> BL_GetGovernmentDropDownList(string pStrGovernmentName, int pIntTaxOfficeID = 0)
        {
            return _GovernmentRepository.REP_GetGovernmentDropDownList(pStrGovernmentName, pIntTaxOfficeID);
        }

        public FuncResponse<Government> BL_InsertUpdateGovernment(Government pObjGovernment)
        {
            FuncResponse<Government> mObjFuncResponse = _GovernmentRepository.REP_InsertUpdateGovernment(pObjGovernment);

            if (pObjGovernment.GovernmentID == 0 && mObjFuncResponse.Success && GlobalDefaultValues.SendNotification)
            {
                //Send Notification
                EmailDetails mObjEmailDetails = new EmailDetails()
                {
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    TaxPayerTypeName = "Government",
                    TaxPayerID = mObjFuncResponse.AdditionalData.GovernmentID,
                    TaxPayerName = mObjFuncResponse.AdditionalData.GovernmentName,
                    TaxPayerRIN = mObjFuncResponse.AdditionalData.GovernmentRIN,
                    TaxPayerMobileNumber = mObjFuncResponse.AdditionalData.ContactNumber,
                    TaxPayerEmail = mObjFuncResponse.AdditionalData.ContactEmail,
                    ContactAddress = mObjFuncResponse.AdditionalData.ContactAddress,
                    TaxPayerTIN = mObjFuncResponse.AdditionalData.TIN,
                    LoggedInUserID = pObjGovernment.CreatedBy,
                };

                if (!string.IsNullOrWhiteSpace(mObjFuncResponse.AdditionalData.ContactEmail))
                {
                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                }

                //if (!string.IsNullOrWhiteSpace(mObjFuncResponse.AdditionalData.ContactNumber))
                //{
                //    _utility.BL_TaxPayerCreated(mObjEmailDetails);
                //}
            }

            return mObjFuncResponse;
        }

        public usp_GetGovernmentList_Result BL_GetGovernmentDetails(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_GetGovernmentDetails(pObjGovernment);
        }

        public FuncResponse BL_UpdateStatus(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_UpdateStatus(pObjGovernment);
        }

        public IList<usp_GetGovernmentAddressInformation_Result> BL_GetAddressInformation(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_GetAddressInformation(pObjGovernment);
        }

        public FuncResponse BL_InsertAddressInformation(MAP_Government_AddressInformation pObjAddressInformation)
        {
            return _GovernmentRepository.REP_InsertAddressInformation(pObjAddressInformation);
        }

        public FuncResponse<IList<usp_GetGovernmentAddressInformation_Result>> BL_RemoveAddressInformation(MAP_Government_AddressInformation pObjAddressInformation)
        {
            return _GovernmentRepository.REP_RemoveAddressInformation(pObjAddressInformation);
        }

        public FuncResponse<Government> BL_CheckGovernmentLoginDetails(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_CheckGovermentLoginDetails(pObjGovernment);
        }

        public FuncResponse BL_ChangePassword(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_ChangePassword(pObjGovernment);
        }

        public FuncResponse BL_UpdatePassword(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_UpdatePassword(pObjGovernment);
        }

        public void BL_UpdateRegisterationStatus(Government pObjGovernment)
        {
            _GovernmentRepository.REP_UpdateRegisterationStatus(pObjGovernment);
        }

        public void REP_UpdateOTPCode(Government pObjGovernment)
        {
            _GovernmentRepository.REP_UpdateOTPCode(pObjGovernment);
        }

        public FuncResponse REP_CheckOTPCode(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_CheckOTPCode(pObjGovernment);
        }

        public IList<usp_SearchGovernmentForRDMLoad_Result> BL_SearchGovernmentDetails(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_SearchGovernmentDetails(pObjGovernment);
        }

        public IDictionary<string, object> BL_SearchGovernment(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_SearchGovernment(pObjGovernment);
        }

        public FuncResponse BL_UpdateTaxOfficer(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_UpdateTaxOfficer(pObjGovernment);
        }

        public IDictionary<string, object> BL_SearchGovernmentForSideMenu(Government pObjGovernment)
        {
            return _GovernmentRepository.REP_SearchGovernmentForSideMenu(pObjGovernment);
        }
    }
}
