using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class LGARepository : ILGARepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateLGA(LGA pObjLGA)
        {
            using (_db = new EIRSEntities())
            {
                LGA mObjInsertUpdateLGA; //LGA Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from lga in _db.LGAs
                                       where lga.LGAName == pObjLGA.LGAName && lga.LGAClassID == pObjLGA.LGAClassID && lga.LGAID != pObjLGA.LGAID
                                       select lga);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Local Government Area already exists";
                    return mObjFuncResponse;
                }

                //If Update Load LGA
                if (pObjLGA.LGAID != 0)
                {
                    mObjInsertUpdateLGA = (from lga in _db.LGAs
                                           where lga.LGAID == pObjLGA.LGAID
                                           select lga).FirstOrDefault();

                    if (mObjInsertUpdateLGA != null)
                    {
                        mObjInsertUpdateLGA.ModifiedBy = pObjLGA.ModifiedBy;
                        mObjInsertUpdateLGA.ModifiedDate = pObjLGA.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateLGA = new LGA();
                        mObjInsertUpdateLGA.CreatedBy = pObjLGA.CreatedBy;
                        mObjInsertUpdateLGA.CreatedDate = pObjLGA.CreatedDate;
                    }
                }
                else // Else Insert LGA
                {
                    mObjInsertUpdateLGA = new LGA();
                    mObjInsertUpdateLGA.CreatedBy = pObjLGA.CreatedBy;
                    mObjInsertUpdateLGA.CreatedDate = pObjLGA.CreatedDate;
                }

                mObjInsertUpdateLGA.LGAName = pObjLGA.LGAName;
                mObjInsertUpdateLGA.LGAClassID = pObjLGA.LGAClassID;
                mObjInsertUpdateLGA.Active = pObjLGA.Active;

                if (pObjLGA.LGAID == 0)
                {
                    _db.LGAs.Add(mObjInsertUpdateLGA);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjLGA.LGAID == 0)
                        mObjFuncResponse.Message = "Local Government Area Added Successfully";
                    else
                        mObjFuncResponse.Message = "Local Government Area Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjLGA.LGAID == 0)
                        mObjFuncResponse.Message = "Local Government Area Addition Failed";
                    else
                        mObjFuncResponse.Message = "Local Government Area Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetLGAList_Result> REP_GetLGAList(LGA pObjLGA)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLGAList(pObjLGA.LGAName, pObjLGA.LGAID, pObjLGA.LGAClassID, pObjLGA.LGAIds, pObjLGA.intStatus, pObjLGA.IncludeLGAIds, pObjLGA.ExcludeLGAIds).ToList();
            }
        }

        public usp_GetLGAList_Result REP_GetLGADetails(LGA pObjLGA)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLGAList(pObjLGA.LGAName, pObjLGA.LGAID, pObjLGA.LGAClassID, pObjLGA.LGAIds, pObjLGA.intStatus, pObjLGA.IncludeLGAIds, pObjLGA.ExcludeLGAIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetLGADropDownList(LGA pObjLGA)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from LGA in _db.usp_GetLGAList(pObjLGA.LGAName, pObjLGA.LGAID, pObjLGA.LGAClassID, pObjLGA.LGAIds, pObjLGA.intStatus, pObjLGA.IncludeLGAIds, pObjLGA.ExcludeLGAIds)
                               select new DropDownListResult()
                               {
                                   id = LGA.LGAID.GetValueOrDefault(),
                                   text = LGA.LGAName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetLGAClassDropDownList()
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from lclass in _db.LGAClasses
                               select new DropDownListResult()
                               {
                                   id = lclass.LGAClassID,
                                   text = lclass.LGAClassName
                               });

                return vResult.ToList();
            }
        }

        public FuncResponse REP_UpdateStatus(LGA pObjLGA)
        {
            using (_db = new EIRSEntities())
            {
                LGA mObjInsertUpdateLGA; //LGA Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load LGA
                if (pObjLGA.LGAID != 0)
                {
                    mObjInsertUpdateLGA = (from lga in _db.LGAs
                                           where lga.LGAID == pObjLGA.LGAID
                                           select lga).FirstOrDefault();

                    if (mObjInsertUpdateLGA != null)
                    {
                        mObjInsertUpdateLGA.Active = !mObjInsertUpdateLGA.Active;
                        mObjInsertUpdateLGA.ModifiedBy = pObjLGA.ModifiedBy;
                        mObjInsertUpdateLGA.ModifiedDate = pObjLGA.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Local Government Area Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetLGAList(pObjLGA.LGAName, 0, pObjLGA.LGAClassID, pObjLGA.LGAIds, pObjLGA.intStatus, pObjLGA.IncludeLGAIds, pObjLGA.ExcludeLGAIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Local Government Area Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
