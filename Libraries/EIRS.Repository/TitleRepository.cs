using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class TitleRepository : ITitleRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateTitle(Title pObjTitle)
        {
            using (_db = new EIRSEntities())
            {
                Title mObjInsertUpdateTitle; //Title Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from tit in _db.Titles
                                       where tit.TitleName == pObjTitle.TitleName && tit.GenderID == pObjTitle.GenderID && tit.TitleID != pObjTitle.TitleID
                                       select tit);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Title already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Title
                if (pObjTitle.TitleID != 0)
                {
                    mObjInsertUpdateTitle = (from tit in _db.Titles
                                                 where tit.TitleID == pObjTitle.TitleID
                                                 select tit).FirstOrDefault();

                    if (mObjInsertUpdateTitle != null)
                    {
                        mObjInsertUpdateTitle.ModifiedBy = pObjTitle.ModifiedBy;
                        mObjInsertUpdateTitle.ModifiedDate = pObjTitle.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateTitle = new Title();
                        mObjInsertUpdateTitle.CreatedBy = pObjTitle.CreatedBy;
                        mObjInsertUpdateTitle.CreatedDate = pObjTitle.CreatedDate;
                    }
                }
                else // Else Insert Title
                {
                    mObjInsertUpdateTitle = new Title();
                    mObjInsertUpdateTitle.CreatedBy = pObjTitle.CreatedBy;
                    mObjInsertUpdateTitle.CreatedDate = pObjTitle.CreatedDate;
                }

                mObjInsertUpdateTitle.TitleName = pObjTitle.TitleName;
                mObjInsertUpdateTitle.GenderID = pObjTitle.GenderID;
                mObjInsertUpdateTitle.Active = pObjTitle.Active;

                if (pObjTitle.TitleID == 0)
                {
                    _db.Titles.Add(mObjInsertUpdateTitle);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjTitle.TitleID == 0)
                        mObjFuncResponse.Message = "Title Added Successfully";
                    else
                        mObjFuncResponse.Message = "Title Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjTitle.TitleID == 0)
                        mObjFuncResponse.Message = "Title Addition Failed";
                    else
                        mObjFuncResponse.Message = "Title Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTitleList_Result> REP_GetTitleList(Title pObjTitle)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTitleList(pObjTitle.TitleName, pObjTitle.TitleID,pObjTitle.GenderID, pObjTitle.TitleIds, pObjTitle.intStatus, pObjTitle.IncludeTitleIds, pObjTitle.ExcludeTitleIds).ToList();
            }
        }

        public usp_GetTitleList_Result REP_GetTitleDetails(Title pObjTitle)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTitleList(pObjTitle.TitleName, pObjTitle.TitleID, pObjTitle.GenderID, pObjTitle.TitleIds, pObjTitle.intStatus, pObjTitle.IncludeTitleIds, pObjTitle.ExcludeTitleIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetTitleDropDownList(Title pObjTitle)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from tit in _db.usp_GetTitleList(pObjTitle.TitleName, pObjTitle.TitleID, pObjTitle.GenderID, pObjTitle.TitleIds, pObjTitle.intStatus, pObjTitle.IncludeTitleIds, pObjTitle.ExcludeTitleIds)
                               select new DropDownListResult()
                               {
                                   id = tit.TitleID.GetValueOrDefault(),
                                   text = tit.TitleName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Title pObjTitle)
        {
            using (_db = new EIRSEntities())
            {
                Title mObjInsertUpdateTitle; //Title Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Title
                if (pObjTitle.TitleID != 0)
                {
                    mObjInsertUpdateTitle = (from tit in _db.Titles
                                                 where tit.TitleID == pObjTitle.TitleID
                                                 select tit).FirstOrDefault();

                    if (mObjInsertUpdateTitle != null)
                    {
                        mObjInsertUpdateTitle.Active = !mObjInsertUpdateTitle.Active;
                        mObjInsertUpdateTitle.ModifiedBy = pObjTitle.ModifiedBy;
                        mObjInsertUpdateTitle.ModifiedDate = pObjTitle.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Title Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetTitleList(pObjTitle.TitleName, 0, pObjTitle.GenderID, pObjTitle.TitleIds, pObjTitle.intStatus, pObjTitle.IncludeTitleIds, pObjTitle.ExcludeTitleIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Title Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }


    }
}
