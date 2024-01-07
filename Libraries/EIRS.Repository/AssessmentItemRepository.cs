using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class AssessmentItemRepository : IAssessmentItemRepository
    {
        EIRSEntities _db;
        
        public FuncResponse REP_InsertUpdateAssessmentItem(Assessment_Items pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Items mObjInsertUpdateAssessmentItem; //Assessment Item Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate Name
                var vDuplicateCheck = (from aicat in _db.Assessment_Items
                                       where aicat.AssessmentItemName == pObjAssessmentItem.AssessmentItemName && aicat.AssessmentItemID != pObjAssessmentItem.AssessmentItemID
                                       select aicat);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Assessment Item Name already exists";
                    return mObjFuncResponse;
                }

                ////Check if Duplicate 
                //var vDuplicateCheck2 = (from aicat in _db.Assessment_Items
                //                       where aicat.AssetTypeID == pObjAssessmentItem.AssetTypeID 
                //                       && aicat.AssessmentGroupID == pObjAssessmentItem.AssessmentGroupID
                //                       && aicat.AssessmentSubGroupID == pObjAssessmentItem.AssessmentSubGroupID
                //                       && aicat.RevenueStreamID == pObjAssessmentItem.RevenueStreamID
                //                       && aicat.RevenueSubStreamID == pObjAssessmentItem.RevenueSubStreamID
                //                       && aicat.AssessmentItemCategoryID == pObjAssessmentItem.AssessmentItemCategoryID
                //                       && aicat.AssessmentItemSubCategoryID == pObjAssessmentItem.AssessmentItemSubCategoryID
                //                       && aicat.AgencyID == pObjAssessmentItem.AgencyID 
                //                       && aicat.AssessmentItemID != pObjAssessmentItem.AssessmentItemID
                //                       select aicat);

                //if (vDuplicateCheck2.Count() > 0)
                //{
                //    mObjFuncResponse.Success = false;
                //    mObjFuncResponse.Message = "Assessment Item already exists";
                //    return mObjFuncResponse;
                //}

                //If Update Load Assessment Item
                if (pObjAssessmentItem.AssessmentItemID != 0)
                {
                    mObjInsertUpdateAssessmentItem = (from aicat in _db.Assessment_Items
                                                              where aicat.AssessmentItemID == pObjAssessmentItem.AssessmentItemID
                                                              select aicat).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentItem != null)
                    {
                        mObjInsertUpdateAssessmentItem.ModifiedBy = pObjAssessmentItem.ModifiedBy;
                        mObjInsertUpdateAssessmentItem.ModifiedDate = pObjAssessmentItem.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessmentItem = new Assessment_Items();
                        mObjInsertUpdateAssessmentItem.CreatedBy = pObjAssessmentItem.CreatedBy;
                        mObjInsertUpdateAssessmentItem.CreatedDate = pObjAssessmentItem.CreatedDate;
                    }
                }
                else // Else Insert Assessment Item
                {
                    mObjInsertUpdateAssessmentItem = new Assessment_Items();
                    mObjInsertUpdateAssessmentItem.CreatedBy = pObjAssessmentItem.CreatedBy;
                    mObjInsertUpdateAssessmentItem.CreatedDate = pObjAssessmentItem.CreatedDate;
                }

                mObjInsertUpdateAssessmentItem.AssetTypeID = pObjAssessmentItem.AssetTypeID;
                mObjInsertUpdateAssessmentItem.AssessmentGroupID = pObjAssessmentItem.AssessmentGroupID;
                mObjInsertUpdateAssessmentItem.AssessmentSubGroupID = pObjAssessmentItem.AssessmentSubGroupID;
                mObjInsertUpdateAssessmentItem.RevenueStreamID = pObjAssessmentItem.RevenueStreamID;
                mObjInsertUpdateAssessmentItem.RevenueSubStreamID = pObjAssessmentItem.RevenueSubStreamID;
                mObjInsertUpdateAssessmentItem.AssessmentItemCategoryID = pObjAssessmentItem.AssessmentItemCategoryID;
                mObjInsertUpdateAssessmentItem.AssessmentItemSubCategoryID = pObjAssessmentItem.AssessmentItemSubCategoryID;
                mObjInsertUpdateAssessmentItem.AgencyID = pObjAssessmentItem.AgencyID;
                mObjInsertUpdateAssessmentItem.AssessmentItemName = pObjAssessmentItem.AssessmentItemName;
                mObjInsertUpdateAssessmentItem.ComputationID = pObjAssessmentItem.ComputationID;
                mObjInsertUpdateAssessmentItem.TaxBaseAmount = pObjAssessmentItem.TaxBaseAmount;
                mObjInsertUpdateAssessmentItem.Percentage = pObjAssessmentItem.Percentage;
                mObjInsertUpdateAssessmentItem.TaxAmount = pObjAssessmentItem.TaxAmount;
                mObjInsertUpdateAssessmentItem.Active = pObjAssessmentItem.Active;

                if (pObjAssessmentItem.AssessmentItemID == 0)
                {
                    _db.Assessment_Items.Add(mObjInsertUpdateAssessmentItem);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAssessmentItem.AssessmentItemID == 0)
                        mObjFuncResponse.Message = "Assessment Item Added Successfully";
                    else
                        mObjFuncResponse.Message = "Assessment Item Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessmentItem.AssessmentItemID == 0)
                        mObjFuncResponse.Message = "Assessment Item Addition Failed";
                    else
                        mObjFuncResponse.Message = "Assessment Item Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAssessmentItemList_Result> REP_GetAssessmentItemList(Assessment_Items pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentItemList(pObjAssessmentItem.AssessmentItemID, pObjAssessmentItem.AssessmentItemReferenceNo,pObjAssessmentItem.intStatus).ToList();
            }
        }

        public usp_GetAssessmentItemList_Result REP_GetAssessmentItemDetails(Assessment_Items pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentItemList(pObjAssessmentItem.AssessmentItemID, pObjAssessmentItem.AssessmentItemReferenceNo, pObjAssessmentItem.intStatus).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAssessmentItemDropDownList(Assessment_Items pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from aicat in _db.usp_GetAssessmentItemList(pObjAssessmentItem.AssessmentItemID, pObjAssessmentItem.AssessmentItemReferenceNo, pObjAssessmentItem.intStatus)
                               select new DropDownListResult()
                               {
                                   id = aicat.AssessmentItemID.GetValueOrDefault(),
                                   text = aicat.AssessmentItemName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Assessment_Items pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Items mObjInsertUpdateAssessmentItem; //Assessment Item Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load AssessmentItem
                if (pObjAssessmentItem.AssessmentItemID != 0)
                {
                    mObjInsertUpdateAssessmentItem = (from aicat in _db.Assessment_Items
                                                              where aicat.AssessmentItemID == pObjAssessmentItem.AssessmentItemID
                                                              select aicat).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentItem != null)
                    {
                        mObjInsertUpdateAssessmentItem.Active = !mObjInsertUpdateAssessmentItem.Active;
                        mObjInsertUpdateAssessmentItem.ModifiedBy = pObjAssessmentItem.ModifiedBy;
                        mObjInsertUpdateAssessmentItem.ModifiedDate = pObjAssessmentItem.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Assessment Item Updated Successfully";
                            //mObjFuncResponse.AdditionalData = _db.usp_GetAssessmentItemList(0, pObjAssessmentItem.AssessmentItemReferenceNo, pObjAssessmentItem.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Assessment Item Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_SearchAssessmentItemForRDMLoad_Result> REP_SearchAssessmentItemDetails(Assessment_Items pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchAssessmentItemForRDMLoad(pObjAssessmentItem.AssessmentItemReferenceNo, pObjAssessmentItem.AssetTypeName, pObjAssessmentItem.AssessmentGroupName, pObjAssessmentItem.AssessmentSubGroupName, pObjAssessmentItem.RevenueStreamName, pObjAssessmentItem.RevenueSubStreamName, pObjAssessmentItem.AssessmentItemCategoryName, pObjAssessmentItem.AssessmentItemSubCategoryName, pObjAssessmentItem.AgencyName, pObjAssessmentItem.AssessmentItemName, pObjAssessmentItem.ComputationName, pObjAssessmentItem.StrTaxAmount, pObjAssessmentItem.StrPercentage, pObjAssessmentItem.StrTaxBaseAmount, pObjAssessmentItem.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchAssessmentItem(Assessment_Items pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["AssessmentItemList"] = _db.usp_SearchAssessmentItem(pObjAssessmentItem.WhereCondition, pObjAssessmentItem.OrderBy, pObjAssessmentItem.OrderByDirection, pObjAssessmentItem.PageNumber, pObjAssessmentItem.PageSize, pObjAssessmentItem.MainFilter,
                                                                        pObjAssessmentItem.AssessmentItemReferenceNo, pObjAssessmentItem.AssetTypeName, pObjAssessmentItem.AssessmentGroupName, pObjAssessmentItem.AssessmentSubGroupName, pObjAssessmentItem.RevenueStreamName, pObjAssessmentItem.RevenueSubStreamName, pObjAssessmentItem.AssessmentItemCategoryName, pObjAssessmentItem.AssessmentItemSubCategoryName, pObjAssessmentItem.AgencyName, pObjAssessmentItem.AssessmentItemName, pObjAssessmentItem.ComputationName, pObjAssessmentItem.StrTaxAmount, pObjAssessmentItem.StrPercentage, pObjAssessmentItem.StrTaxBaseAmount, pObjAssessmentItem.ActiveText).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(AssessmentItemID) FROM Assessment_Items").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(AssessmentItemID) ");
                sbFilteredCountQuery.Append(" FROM Assessment_Items aitem ");
                sbFilteredCountQuery.Append(" INNER JOIN Asset_Types atype ON aitem.AssetTypeID = atype.AssetTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Assessment_Group agrp ON aitem.AssessmentGroupID = agrp.AssessmentGroupID ");
                sbFilteredCountQuery.Append(" INNER JOIN Assessment_SubGroup asgrp ON aitem.AssessmentSubGroupID = asgrp.AssessmentSubGroupID ");
                sbFilteredCountQuery.Append(" INNER JOIN Revenue_Stream rstrm ON aitem.RevenueStreamID = rstrm.RevenueStreamID ");
                sbFilteredCountQuery.Append(" INNER JOIN Revenue_SubStream rsstrm ON aitem.RevenueSubStreamID = rsstrm.RevenueSubStreamID ");
                sbFilteredCountQuery.Append(" INNER JOIN Assessment_Item_Category aicat  ON aitem.AssessmentItemCategoryID = aicat.AssessmentItemCategoryID ");
                sbFilteredCountQuery.Append(" INNER JOIN Assessment_Item_SubCategory aiscat ON aitem.AssessmentItemSubCategoryID = aiscat.AssessmentItemSubCategoryID ");
                sbFilteredCountQuery.Append(" INNER JOIN Agencies agncy ON aitem.AgencyID = agncy.AgencyID ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_Computation comp ON aitem.ComputationID = comp.ComputationID  WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjAssessmentItem.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjAssessmentItem.MainFilter),
                    new SqlParameter("@AssessmentItemReferenceNo",pObjAssessmentItem.AssessmentItemReferenceNo),
                    new SqlParameter("@AssetTypeName",pObjAssessmentItem.AssetTypeName),
                    new SqlParameter("@AssessmentGroupName ",pObjAssessmentItem.AssessmentGroupName ),
                    new SqlParameter("@AssessmentSubGroupName",pObjAssessmentItem.AssessmentSubGroupName),
                    new SqlParameter("@RevenueStreamName",pObjAssessmentItem.RevenueStreamName),
                    new SqlParameter("@RevenueSubStreamName ",pObjAssessmentItem.RevenueSubStreamName ),
                    new SqlParameter("@AssessmentItemCategoryName ",pObjAssessmentItem.AssessmentItemCategoryName ),
                    new SqlParameter("@AssessmentItemSubCategoryName ",pObjAssessmentItem.AssessmentItemSubCategoryName ),
                    new SqlParameter("@AgencyName",pObjAssessmentItem.AgencyName),
                    new SqlParameter("@AssessmentItemName",pObjAssessmentItem.AssessmentItemName),
                    new SqlParameter("@ComputationName",pObjAssessmentItem.ComputationName),
                    new SqlParameter("@TaxAmount",pObjAssessmentItem.StrTaxAmount),
                    new SqlParameter("@Percentage",pObjAssessmentItem.StrPercentage),
                    new SqlParameter("@TaxBaseAmount",pObjAssessmentItem.StrTaxBaseAmount),
                    new SqlParameter("@ActiveText",pObjAssessmentItem.ActiveText)
                };

                //Get Filtered Count
                //int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntTotalCount;

                return dcData;
            }
        }
    }
}
