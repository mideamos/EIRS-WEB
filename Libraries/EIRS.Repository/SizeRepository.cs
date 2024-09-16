using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class SizeRepository : ISizeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateSize(Size pObjSize)
        {
            using (_db = new EIRSEntities())
            {
                Size mObjInsertUpdateSize; //Size Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from Size in _db.Sizes
                                       where Size.SizeName == pObjSize.SizeName && Size.SizeID != pObjSize.SizeID
                                       select Size);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Size already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Size
                if (pObjSize.SizeID != 0)
                {
                    mObjInsertUpdateSize = (from Size in _db.Sizes
                                            where Size.SizeID == pObjSize.SizeID
                                            select Size).FirstOrDefault();

                    if (mObjInsertUpdateSize != null)
                    {
                        mObjInsertUpdateSize.ModifiedBy = pObjSize.ModifiedBy;
                        mObjInsertUpdateSize.ModifiedDate = pObjSize.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateSize = new Size();
                        mObjInsertUpdateSize.CreatedBy = pObjSize.CreatedBy;
                        mObjInsertUpdateSize.CreatedDate = pObjSize.CreatedDate;
                    }
                }
                else // Else Insert Size
                {
                    mObjInsertUpdateSize = new Size();
                    mObjInsertUpdateSize.CreatedBy = pObjSize.CreatedBy;
                    mObjInsertUpdateSize.CreatedDate = pObjSize.CreatedDate;
                }

                mObjInsertUpdateSize.SizeName = pObjSize.SizeName;
                mObjInsertUpdateSize.Active = pObjSize.Active;

                if (pObjSize.SizeID == 0)
                {
                    _db.Sizes.Add(mObjInsertUpdateSize);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjSize.SizeID == 0)
                        mObjFuncResponse.Message = "Size Added Successfully";
                    else
                        mObjFuncResponse.Message = "Size Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjSize.SizeID == 0)
                        mObjFuncResponse.Message = "Size Addition Failed";
                    else
                        mObjFuncResponse.Message = "Size Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetSizeList_Result> REP_GetSizeList(Size pObjSize)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSizeList(pObjSize.SizeName, pObjSize.SizeID, pObjSize.SizeIds, pObjSize.intStatus, pObjSize.IncludeSizeIds, pObjSize.ExcludeSizeIds).ToList();
            }
        }

        public usp_GetSizeList_Result REP_GetSizeDetails(Size pObjSize)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSizeList(pObjSize.SizeName, pObjSize.SizeID, pObjSize.SizeIds, pObjSize.intStatus, pObjSize.IncludeSizeIds, pObjSize.ExcludeSizeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetSizeDropDownList(Size pObjSize)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from Size in _db.usp_GetSizeList(pObjSize.SizeName, pObjSize.SizeID, pObjSize.SizeIds, pObjSize.intStatus, pObjSize.IncludeSizeIds, pObjSize.ExcludeSizeIds)
                               select new DropDownListResult()
                               {
                                   id = Size.SizeID.GetValueOrDefault(),
                                   text = Size.SizeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Size pObjSize)
        {
            using (_db = new EIRSEntities())
            {
                Size mObjInsertUpdateSize; //Size Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Size
                if (pObjSize.SizeID != 0)
                {
                    mObjInsertUpdateSize = (from Size in _db.Sizes
                                            where Size.SizeID == pObjSize.SizeID
                                            select Size).FirstOrDefault();

                    if (mObjInsertUpdateSize != null)
                    {
                        mObjInsertUpdateSize.Active = !mObjInsertUpdateSize.Active;
                        mObjInsertUpdateSize.ModifiedBy = pObjSize.ModifiedBy;
                        mObjInsertUpdateSize.ModifiedDate = pObjSize.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Size Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetSizeList(pObjSize.SizeName, 0, pObjSize.SizeIds, pObjSize.intStatus, pObjSize.IncludeSizeIds, pObjSize.ExcludeSizeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Size Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
