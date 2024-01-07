using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public class ClaimBusinessRepository : IClaimBusinessRepository
    {
        ERASEntities _db;

        public FuncResponse REP_InsertUpdateBusiness(MST_Business pObjBusiness)
        {
            using (_db = new ERASEntities())
            {
                MST_Business mObjInsertUpdateBusiness; //Business Insert Update Object

                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Business
                if (pObjBusiness.BusinessID != 0)
                {
                    mObjInsertUpdateBusiness = (from Business in _db.MST_Business
                                                where Business.BusinessID == pObjBusiness.BusinessID
                                                select Business).FirstOrDefault();

                    if (mObjInsertUpdateBusiness == null)
                    {
                        mObjInsertUpdateBusiness = new MST_Business();
                    }
                }
                else // Else Insert Business
                {
                    mObjInsertUpdateBusiness = new MST_Business();
                }

                mObjInsertUpdateBusiness.BusinessName = pObjBusiness.BusinessName;
                mObjInsertUpdateBusiness.BusinessAddress = pObjBusiness.BusinessAddress;
                mObjInsertUpdateBusiness.BusinessSector = pObjBusiness.BusinessSector;
                mObjInsertUpdateBusiness.BusinessSubSector = pObjBusiness.BusinessSubSector;
                mObjInsertUpdateBusiness.BusinessType = pObjBusiness.BusinessType;
                mObjInsertUpdateBusiness.BusinessCategory = pObjBusiness.BusinessCategory;
                mObjInsertUpdateBusiness.AssetType = pObjBusiness.AssetType;
                mObjInsertUpdateBusiness.BusinessStructure = pObjBusiness.BusinessStructure;
                mObjInsertUpdateBusiness.BusinessOperation = pObjBusiness.BusinessOperation;
                mObjInsertUpdateBusiness.Size = pObjBusiness.Size;
                mObjInsertUpdateBusiness.LGA = pObjBusiness.LGA;
                mObjInsertUpdateBusiness.TIN = pObjBusiness.TIN;
                mObjInsertUpdateBusiness.TaxOffice = pObjBusiness.TaxOffice;
                mObjInsertUpdateBusiness.ContactName = pObjBusiness.ContactName;
                mObjInsertUpdateBusiness.Phone = pObjBusiness.Phone;
                mObjInsertUpdateBusiness.EmailAddress = pObjBusiness.EmailAddress;
                mObjInsertUpdateBusiness.Source = pObjBusiness.Source;
                mObjInsertUpdateBusiness.BuildingTag = pObjBusiness.BuildingTag;
                mObjInsertUpdateBusiness.Latitude = pObjBusiness.Latitude;
                mObjInsertUpdateBusiness.Longitude = pObjBusiness.Longitude;
                //mObjInsertUpdateBusiness.Claimed = pObjBusiness.Claimed;
                mObjInsertUpdateBusiness.Active = pObjBusiness.Active;

                if (pObjBusiness.BusinessID == 0)
                {
                    _db.MST_Business.Add(mObjInsertUpdateBusiness);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBusiness.BusinessID == 0)
                        mObjFuncResponse.Message = "Business Added Successfully";
                    else
                        mObjFuncResponse.Message = "Business Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    if (pObjBusiness.BusinessID == 0)
                        mObjFuncResponse.Message = "Business Addition Failed";
                    else
                        mObjFuncResponse.Message = "Business Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetClaimBusinessList_Result> REP_GetBusinessList(MST_Business pObjBusiness)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetClaimBusinessList(pObjBusiness.BusinessID, pObjBusiness.BusinessName, pObjBusiness.intClaimed, pObjBusiness.intStatus).ToList();
            }
        }

        public usp_GetClaimBusinessList_Result REP_GetBusinessDetails(MST_Business pObjBusiness)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetClaimBusinessList(pObjBusiness.BusinessID, pObjBusiness.BusinessName, pObjBusiness.intClaimed, pObjBusiness.intStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(MST_Business pObjBusiness)
        {
            using (_db = new ERASEntities())
            {
                MST_Business mObjInsertUpdateBusiness; //Business Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Business
                if (pObjBusiness.BusinessID != 0)
                {
                    mObjInsertUpdateBusiness = (from bcomp in _db.MST_Business
                                                where bcomp.BusinessID == pObjBusiness.BusinessID
                                                select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateBusiness != null)
                    {
                        mObjInsertUpdateBusiness.Claimed = !mObjInsertUpdateBusiness.Claimed;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Updated Successfully";
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateBusiness(MST_Business pObjBusiness)
        {
            using (_db = new ERASEntities())
            {
                MST_Business mObjInsertUpdateBusiness; //Business Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Business Completion

                if (pObjBusiness.BusinessID != 0)
                {
                    mObjInsertUpdateBusiness = (from Business in _db.MST_Business
                                                where Business.BusinessID == pObjBusiness.BusinessID
                                                select Business).FirstOrDefault();

                    mObjInsertUpdateBusiness.Phone = pObjBusiness.Phone;
                    mObjInsertUpdateBusiness.ContactName = pObjBusiness.ContactName;

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        if (pObjBusiness.BusinessID == 0)
                            mObjFuncResponse.Message = "Business Added Successfully";
                        else
                            mObjFuncResponse.Message = "Business Updated Successfully";
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;

                        if (pObjBusiness.BusinessID == 0)
                            mObjFuncResponse.Message = "Business Addition Failed";
                        else
                            mObjFuncResponse.Message = "Business Updation Failed";
                    }
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "No Business Found";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdatePhoneVerified(MST_Business pObjBusiness)
        {
            using (_db = new ERASEntities())
            {
                MST_Business mObjInsertUpdateBusiness; //Business Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Business
                if (pObjBusiness.BusinessID != 0)
                {
                    mObjInsertUpdateBusiness = (from bcomp in _db.MST_Business
                                                where bcomp.BusinessID == pObjBusiness.BusinessID
                                                select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateBusiness != null)
                    {
                        mObjInsertUpdateBusiness.PhoneVerified = true;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Updated Successfully";
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
