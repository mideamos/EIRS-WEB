using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
  
    public class EM_IGRClassificationRepository : IEM_IGRClassificationRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateIGRClassification(EM_IGRClassification pObjIGRClassification)
        {
            using (_db = new EIRSEntities())
            {
                EM_IGRClassification mObjInsertUpdateIGRClassification; //IGR Classification Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from cls in _db.EM_IGRClassification
                                       where cls.RevenueHeadID == pObjIGRClassification.RevenueHeadID && cls.TaxMonth == pObjIGRClassification.TaxMonth && cls.IGRClassificationID != pObjIGRClassification.IGRClassificationID
                                       select cls);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "IGR Classification already exists";
                    return mObjFuncResponse;
                }

                //If Update Load IGR Classification
                if (pObjIGRClassification.IGRClassificationID != 0)
                {
                    mObjInsertUpdateIGRClassification = (from cls in _db.EM_IGRClassification
                                                         where cls.IGRClassificationID == pObjIGRClassification.IGRClassificationID
                                                         select cls).FirstOrDefault();

                    if (mObjInsertUpdateIGRClassification != null)
                    {
                        mObjInsertUpdateIGRClassification.ModifiedBy = pObjIGRClassification.ModifiedBy;
                        mObjInsertUpdateIGRClassification.ModifiedDate = pObjIGRClassification.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateIGRClassification = new EM_IGRClassification();
                        mObjInsertUpdateIGRClassification.CreatedBy = pObjIGRClassification.CreatedBy;
                        mObjInsertUpdateIGRClassification.CreatedDate = pObjIGRClassification.CreatedDate;
                    }
                }
                else // Else Insert IGR Classification
                {
                    mObjInsertUpdateIGRClassification = new EM_IGRClassification();
                    mObjInsertUpdateIGRClassification.CreatedBy = pObjIGRClassification.CreatedBy;
                    mObjInsertUpdateIGRClassification.CreatedDate = pObjIGRClassification.CreatedDate;
                }

                mObjInsertUpdateIGRClassification.RevenueHeadID = pObjIGRClassification.RevenueHeadID;
                mObjInsertUpdateIGRClassification.TaxMonth = pObjIGRClassification.TaxMonth;
                mObjInsertUpdateIGRClassification.Active = pObjIGRClassification.Active;

                if (pObjIGRClassification.IGRClassificationID == 0)
                {
                    _db.EM_IGRClassification.Add(mObjInsertUpdateIGRClassification);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjIGRClassification.IGRClassificationID == 0)
                        mObjFuncResponse.Message = "IGR Classification Added Successfully";
                    else
                        mObjFuncResponse.Message = "IGR Classification Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjIGRClassification.IGRClassificationID == 0)
                        mObjFuncResponse.Message = "IGR Classification Addition Failed";
                    else
                        mObjFuncResponse.Message = "IGR Classification Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_EM_GetIGRClassificationList_Result> REP_GetIGRClassificationList(EM_IGRClassification pObjIGRClassification)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetIGRClassificationList(pObjIGRClassification.RevenueHeadID, pObjIGRClassification.CategoryID, pObjIGRClassification.IGRClassificationID, pObjIGRClassification.intStatus).ToList();

            }
        }

        public usp_EM_GetIGRClassificationList_Result REP_GetIGRClassificationDetails(EM_IGRClassification pObjIGRClassification)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetIGRClassificationList(pObjIGRClassification.RevenueHeadID, pObjIGRClassification.CategoryID, pObjIGRClassification.IGRClassificationID, pObjIGRClassification.intStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(EM_IGRClassification pObjIGRClassification)
        {
            using (_db = new EIRSEntities())
            {
                EM_IGRClassification mObjInsertUpdateIGRClassification; //IGR Classification Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load IGRClassification
                if (pObjIGRClassification.IGRClassificationID != 0)
                {
                    mObjInsertUpdateIGRClassification = (from cls in _db.EM_IGRClassification
                                                         where cls.IGRClassificationID == pObjIGRClassification.IGRClassificationID
                                                         select cls).FirstOrDefault();

                    if (mObjInsertUpdateIGRClassification != null)
                    {
                        mObjInsertUpdateIGRClassification.Active = !mObjInsertUpdateIGRClassification.Active;
                        mObjInsertUpdateIGRClassification.ModifiedBy = pObjIGRClassification.ModifiedBy;
                        mObjInsertUpdateIGRClassification.ModifiedDate = pObjIGRClassification.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "IGR Classification Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_EM_GetIGRClassificationList(pObjIGRClassification.RevenueHeadID, pObjIGRClassification.CategoryID, pObjIGRClassification.IGRClassificationID, pObjIGRClassification.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "IGR Classification Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_EM_GetClassificationDataSourceList_Result> REP_GetClassificationDataSource(long plngIGRClassificationID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetClassificationDataSourceList(plngIGRClassificationID).ToList();
            }
        }

        public IList<vw_EM_BankStatement> REP_GetBankStatementList(long plngIGRClassificationID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from cent in _db.EM_MAP_IGRClassification_Entry
                               join bs in _db.vw_EM_BankStatement on cent.DSEID equals bs.BSID
                               where cent.DataSourceID == 5 && cent.IGRClassificationID == plngIGRClassificationID
                               select bs);

                return vResult.ToList();
            }
        }

        public IList<vw_EM_PD_Main_Authorized> REP_GetPDMainAuthorizedList(long plngIGRClassificationID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from cent in _db.EM_MAP_IGRClassification_Entry
                               join pdma in _db.vw_EM_PD_Main_Authorized on cent.DSEID equals pdma.PDMAID
                               where cent.DataSourceID == 1 && cent.IGRClassificationID == plngIGRClassificationID
                               select pdma);

                return vResult.ToList();
            }
        }

        public IList<vw_EM_PD_Main_Pending> REP_GetPDMainPendingList(long plngIGRClassificationID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from cent in _db.EM_MAP_IGRClassification_Entry
                               join pdmd in _db.vw_EM_PD_Main_Pending on cent.DSEID equals pdmd.PDMPID
                               where cent.DataSourceID == 2 && cent.IGRClassificationID == plngIGRClassificationID
                               select pdmd);

                return vResult.ToList();
            }
        }

        public IList<vw_EM_PD_MVA_Authorized> REP_GetPDMVAAuthorizedList(long plngIGRClassificationID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from cent in _db.EM_MAP_IGRClassification_Entry
                               join pdma in _db.vw_EM_PD_MVA_Authorized on cent.DSEID equals pdma.PDMVAID
                               where cent.DataSourceID == 3 && cent.IGRClassificationID == plngIGRClassificationID
                               select pdma);

                return vResult.ToList();
            }
        }

        public IList<vw_EM_PD_MVA_Pending> REP_GetPDMVAPendingList(long plngIGRClassificationID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from cent in _db.EM_MAP_IGRClassification_Entry
                               join pdmd in _db.vw_EM_PD_MVA_Pending on cent.DSEID equals pdmd.PDMVPID
                               where cent.DataSourceID == 4 && cent.IGRClassificationID == plngIGRClassificationID
                               select pdmd);

                return vResult.ToList();
            }
        }

        public FuncResponse REP_InsertClassificationEntry(EM_MAP_IGRClassification_Entry pObjClassificationEntry)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from cent in _db.EM_MAP_IGRClassification_Entry
                               where cent.IGRClassificationID == pObjClassificationEntry.IGRClassificationID && cent.DSEID == pObjClassificationEntry.DSEID && cent.DataSourceID == pObjClassificationEntry.DataSourceID
                               select cent);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Already Exists";
                    return mObjResponse;
                }

                _db.EM_MAP_IGRClassification_Entry.Add(pObjClassificationEntry);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }
    }
}
