using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLSpecial
    {
        ISpecialRepository _SpecialRepository;
        IUtility _utility;

        public BLSpecial()
        {
            _SpecialRepository = new SpecialRepository();
            _utility = new Utility();
        }

        public IList<usp_GetSpecialList_Result> BL_GetSpecialList(Special pObjSpecial)
        {
            return _SpecialRepository.REP_GetSpecialList(pObjSpecial);
        }

        public IList<vw_Special> BL_GetSpecialList()
        {
            return _SpecialRepository.REP_GetSpecialList();
        }

        public IList<DropDownListResult> BL_GetSpecialDropDownList(Special pObjSpecial)
        {
            return _SpecialRepository.REP_GetSpecialDropDownList(pObjSpecial);
        }

        public IList<DropDownListResult> BL_GetSpecialDropDownList(string pStrSpecialName, int pIntTaxOfficeID = 0)
        {
            return _SpecialRepository.REP_GetSpecialDropDownList(pStrSpecialName, pIntTaxOfficeID);
        }

        public FuncResponse<Special> BL_InsertUpdateSpecial(Special pObjSpecial)
        {
            FuncResponse<Special> mObjFuncResponse = _SpecialRepository.REP_InsertUpdateSpecial(pObjSpecial);

            if (pObjSpecial.SpecialID == 0 && mObjFuncResponse.Success && GlobalDefaultValues.SendNotification)
            {
                //Send Notification
                EmailDetails mObjEmailDetails = new EmailDetails()
                {
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    TaxPayerTypeName = "Special",
                    TaxPayerID = mObjFuncResponse.AdditionalData.SpecialID,
                    TaxPayerName = mObjFuncResponse.AdditionalData.SpecialTaxPayerName,
                    TaxPayerRIN = mObjFuncResponse.AdditionalData.SpecialRIN,
                    TaxPayerMobileNumber = mObjFuncResponse.AdditionalData.ContactNumber,
                    TaxPayerEmail = mObjFuncResponse.AdditionalData.ContactEmail,
                    ContactAddress = "NA",
                    TaxPayerTIN = mObjFuncResponse.AdditionalData.TIN,
                    LoggedInUserID = pObjSpecial.CreatedBy,
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

        public usp_GetSpecialList_Result BL_GetSpecialDetails(Special pObjSpecial)
        {
            return _SpecialRepository.REP_GetSpecialDetails(pObjSpecial);
        }

        public FuncResponse BL_UpdateStatus(Special pObjSpecial)
        {
            return _SpecialRepository.REP_UpdateStatus(pObjSpecial);
        }

        public IList<usp_GetSpecialAddressInformation_Result> BL_GetAddressInformation(Special pObjSpecial)
        {
            return _SpecialRepository.REP_GetAddressInformation(pObjSpecial);
        }

        public FuncResponse BL_InsertAddressInformation(MAP_Special_AddressInformation pObjAddressInformation)
        {
            return _SpecialRepository.REP_InsertAddressInformation(pObjAddressInformation);
        }

        public FuncResponse<IList<usp_GetSpecialAddressInformation_Result>> BL_RemoveAddressInformation(MAP_Special_AddressInformation pObjAddressInformation)
        {
            return _SpecialRepository.REP_RemoveAddressInformation(pObjAddressInformation);
        }

        public IList<usp_SearchSpecialForRDMLoad_Result> BL_SearchSpecialDetails(Special pObjSpecial)
        {
            return _SpecialRepository.REP_SearchSpecialDetails(pObjSpecial);
        }

        public IDictionary<string, object> BL_SearchSpecial(Special pObjSpecial)
        {
            return _SpecialRepository.REP_SearchSpecial(pObjSpecial);
        }

        public FuncResponse BL_UpdateTaxOfficer(Special pObjSpecial)
        {
            return _SpecialRepository.REP_UpdateTaxOfficer(pObjSpecial);
        }
    }
}
