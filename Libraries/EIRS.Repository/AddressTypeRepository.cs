using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class AddressTypeRepository : IAddressTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateAddressType(Address_Types pObjAddressType)
        {
            using (_db = new EIRSEntities())
            {
                Address_Types mObjInsertUpdateAddressType; //Address Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from AddressType in _db.Address_Types
                                       where AddressType.AddressTypeName == pObjAddressType.AddressTypeName && AddressType.AddressTypeID != pObjAddressType.AddressTypeID
                                       select AddressType);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Address Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Address Type
                if (pObjAddressType.AddressTypeID != 0)
                {
                    mObjInsertUpdateAddressType = (from AddressType in _db.Address_Types
                                                 where AddressType.AddressTypeID == pObjAddressType.AddressTypeID
                                                 select AddressType).FirstOrDefault();

                    if (mObjInsertUpdateAddressType != null)
                    {
                        mObjInsertUpdateAddressType.ModifiedBy = pObjAddressType.ModifiedBy;
                        mObjInsertUpdateAddressType.ModifiedDate = pObjAddressType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAddressType = new Address_Types();
                        mObjInsertUpdateAddressType.CreatedBy = pObjAddressType.CreatedBy;
                        mObjInsertUpdateAddressType.CreatedDate = pObjAddressType.CreatedDate;
                    }
                }
                else // Else Insert Address Type
                {
                    mObjInsertUpdateAddressType = new Address_Types();
                    mObjInsertUpdateAddressType.CreatedBy = pObjAddressType.CreatedBy;
                    mObjInsertUpdateAddressType.CreatedDate = pObjAddressType.CreatedDate;
                }

                mObjInsertUpdateAddressType.AddressTypeName = pObjAddressType.AddressTypeName;
                mObjInsertUpdateAddressType.Active = pObjAddressType.Active;

                if (pObjAddressType.AddressTypeID == 0)
                {
                    _db.Address_Types.Add(mObjInsertUpdateAddressType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAddressType.AddressTypeID == 0)
                        mObjFuncResponse.Message = "Address Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Address Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAddressType.AddressTypeID == 0)
                        mObjFuncResponse.Message = "Address Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Address Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAddressTypeList_Result> REP_GetAddressTypeList(Address_Types pObjAddressType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAddressTypeList(pObjAddressType.AddressTypeName, pObjAddressType.AddressTypeID, pObjAddressType.AddressTypeIds, pObjAddressType.intStatus, pObjAddressType.IncludeAddressTypeIds, pObjAddressType.ExcludeAddressTypeIds).ToList();
            }
        }

        public usp_GetAddressTypeList_Result REP_GetAddressTypeDetails(Address_Types pObjAddressType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAddressTypeList(pObjAddressType.AddressTypeName, pObjAddressType.AddressTypeID, pObjAddressType.AddressTypeIds, pObjAddressType.intStatus, pObjAddressType.IncludeAddressTypeIds, pObjAddressType.ExcludeAddressTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAddressTypeDropDownList(Address_Types pObjAddressType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from AddressType in _db.usp_GetAddressTypeList(pObjAddressType.AddressTypeName, pObjAddressType.AddressTypeID, pObjAddressType.AddressTypeIds, pObjAddressType.intStatus, pObjAddressType.IncludeAddressTypeIds, pObjAddressType.ExcludeAddressTypeIds)
                               select new DropDownListResult()
                               {
                                   id = AddressType.AddressTypeID.GetValueOrDefault(),
                                   text = AddressType.AddressTypeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Address_Types pObjAddressType)
        {
            using (_db = new EIRSEntities())
            {
                Address_Types mObjInsertUpdateAddressType; //Address Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load AddressType
                if (pObjAddressType.AddressTypeID != 0)
                {
                    mObjInsertUpdateAddressType = (from AddressType in _db.Address_Types
                                                 where AddressType.AddressTypeID == pObjAddressType.AddressTypeID
                                                 select AddressType).FirstOrDefault();

                    if (mObjInsertUpdateAddressType != null)
                    {
                        mObjInsertUpdateAddressType.Active = !mObjInsertUpdateAddressType.Active;
                        mObjInsertUpdateAddressType.ModifiedBy = pObjAddressType.ModifiedBy;
                        mObjInsertUpdateAddressType.ModifiedDate = pObjAddressType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Address Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetAddressTypeList(pObjAddressType.AddressTypeName, 0, pObjAddressType.AddressTypeIds, pObjAddressType.intStatus, pObjAddressType.IncludeAddressTypeIds, pObjAddressType.ExcludeAddressTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Address Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
