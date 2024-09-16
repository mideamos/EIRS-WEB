using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class MDAServiceItemRepository : IMDAServiceItemRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateMDAServiceItem(MDA_Service_Items pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {
                MDA_Service_Items mObjInsertUpdateMDAServiceItem; //MDA Service Item Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate Name
                var vDuplicateCheck = (from msitem in _db.MDA_Service_Items
                                       where msitem.MDAServiceItemName == pObjMDAServiceItem.MDAServiceItemName && msitem.MDAServiceItemID != pObjMDAServiceItem.MDAServiceItemID
                                       select msitem);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "MDA Service Item Name already exists";
                    return mObjFuncResponse;
                }

                //Check if Duplicate 
                var vDuplicateCheck2 = (from msitem in _db.MDA_Service_Items
                                        where msitem.RevenueStreamID == pObjMDAServiceItem.RevenueStreamID
                                        && msitem.RevenueSubStreamID == pObjMDAServiceItem.RevenueSubStreamID
                                        && msitem.AssessmentItemCategoryID == pObjMDAServiceItem.AssessmentItemCategoryID
                                        && msitem.AssessmentItemSubCategoryID == pObjMDAServiceItem.AssessmentItemSubCategoryID
                                        && msitem.AgencyID == pObjMDAServiceItem.AgencyID
                                        && msitem.MDAServiceItemID != pObjMDAServiceItem.MDAServiceItemID
                                        select msitem);

                if (vDuplicateCheck2.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "MDA Service Item already exists";
                    return mObjFuncResponse;
                }

                //If Update Load MDA Service Item
                if (pObjMDAServiceItem.MDAServiceItemID != 0)
                {
                    mObjInsertUpdateMDAServiceItem = (from msitem in _db.MDA_Service_Items
                                                      where msitem.MDAServiceItemID == pObjMDAServiceItem.MDAServiceItemID
                                                      select msitem).FirstOrDefault();

                    if (mObjInsertUpdateMDAServiceItem != null)
                    {
                        mObjInsertUpdateMDAServiceItem.ModifiedBy = pObjMDAServiceItem.ModifiedBy;
                        mObjInsertUpdateMDAServiceItem.ModifiedDate = pObjMDAServiceItem.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateMDAServiceItem = new MDA_Service_Items();
                        mObjInsertUpdateMDAServiceItem.CreatedBy = pObjMDAServiceItem.CreatedBy;
                        mObjInsertUpdateMDAServiceItem.CreatedDate = pObjMDAServiceItem.CreatedDate;
                    }
                }
                else // Else Insert MDA Service Item
                {
                    mObjInsertUpdateMDAServiceItem = new MDA_Service_Items();
                    mObjInsertUpdateMDAServiceItem.CreatedBy = pObjMDAServiceItem.CreatedBy;
                    mObjInsertUpdateMDAServiceItem.CreatedDate = pObjMDAServiceItem.CreatedDate;
                }

                mObjInsertUpdateMDAServiceItem.RevenueStreamID = pObjMDAServiceItem.RevenueStreamID;
                mObjInsertUpdateMDAServiceItem.RevenueSubStreamID = pObjMDAServiceItem.RevenueSubStreamID;
                mObjInsertUpdateMDAServiceItem.AssessmentItemCategoryID = pObjMDAServiceItem.AssessmentItemCategoryID;
                mObjInsertUpdateMDAServiceItem.AssessmentItemSubCategoryID = pObjMDAServiceItem.AssessmentItemSubCategoryID;
                mObjInsertUpdateMDAServiceItem.AgencyID = pObjMDAServiceItem.AgencyID;
                mObjInsertUpdateMDAServiceItem.MDAServiceItemName = pObjMDAServiceItem.MDAServiceItemName;
                mObjInsertUpdateMDAServiceItem.ComputationID = pObjMDAServiceItem.ComputationID;
                mObjInsertUpdateMDAServiceItem.ServiceBaseAmount = pObjMDAServiceItem.ServiceBaseAmount;
                mObjInsertUpdateMDAServiceItem.Percentage = pObjMDAServiceItem.Percentage;
                mObjInsertUpdateMDAServiceItem.ServiceAmount = pObjMDAServiceItem.ServiceAmount;
                mObjInsertUpdateMDAServiceItem.Active = pObjMDAServiceItem.Active;

                if (pObjMDAServiceItem.MDAServiceItemID == 0)
                {
                    _db.MDA_Service_Items.Add(mObjInsertUpdateMDAServiceItem);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjMDAServiceItem.MDAServiceItemID == 0)
                    {
                        mObjFuncResponse.Message = "MDA Service Item Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "MDA Service Item Updated Successfully";
                    }
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjMDAServiceItem.MDAServiceItemID == 0)
                    {
                        mObjFuncResponse.Message = "MDA Service Item Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "MDA Service Item Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetMDAServiceItemList_Result> REP_GetMDAServiceItemList(MDA_Service_Items pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetMDAServiceItemList(pObjMDAServiceItem.MDAServiceItemID, pObjMDAServiceItem.MDAServiceItemReferenceNo, pObjMDAServiceItem.intStatus).ToList();
            }
        }

        public usp_GetMDAServiceItemList_Result REP_GetMDAServiceItemDetails(MDA_Service_Items pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetMDAServiceItemList(pObjMDAServiceItem.MDAServiceItemID, pObjMDAServiceItem.MDAServiceItemReferenceNo, pObjMDAServiceItem.intStatus).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetMDAServiceItemDropDownList(MDA_Service_Items pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from msitem in _db.usp_GetMDAServiceItemList(pObjMDAServiceItem.MDAServiceItemID, pObjMDAServiceItem.MDAServiceItemReferenceNo, pObjMDAServiceItem.intStatus)
                               select new DropDownListResult()
                               {
                                   id = msitem.MDAServiceItemID.GetValueOrDefault(),
                                   text = msitem.MDAServiceItemName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(MDA_Service_Items pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {
                MDA_Service_Items mObjInsertUpdateMDAServiceItem; //MDA Service Item Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load MDAServiceItem
                if (pObjMDAServiceItem.MDAServiceItemID != 0)
                {
                    mObjInsertUpdateMDAServiceItem = (from msitem in _db.MDA_Service_Items
                                                      where msitem.MDAServiceItemID == pObjMDAServiceItem.MDAServiceItemID
                                                      select msitem).FirstOrDefault();

                    if (mObjInsertUpdateMDAServiceItem != null)
                    {
                        mObjInsertUpdateMDAServiceItem.Active = !mObjInsertUpdateMDAServiceItem.Active;
                        mObjInsertUpdateMDAServiceItem.ModifiedBy = pObjMDAServiceItem.ModifiedBy;
                        mObjInsertUpdateMDAServiceItem.ModifiedDate = pObjMDAServiceItem.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "MDA Service Item Updated Successfully";
                           // mObjFuncResponse.AdditionalData = _db.usp_GetMDAServiceItemList(0, pObjMDAServiceItem.MDAServiceItemReferenceNo, pObjMDAServiceItem.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "MDA Service Item Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetVehicleInsuranceVerificationMDAServiceItemForSupplier_Result> REP_GetVehicleInsuranceVerificationMDAServiceItemForSupplier()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleInsuranceVerificationMDAServiceItemForSupplier().ToList();
            }
        }

        public IList<usp_GetVehicleLicenseMDAServiceItemForSupplier_Result> REP_GetVehicleLicenseMDAServiceItemForSupplier()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleLicenseMDAServiceItemForSupplier().ToList();
            }
        }

        public IList<usp_GetEdoGISMDAServiceItemForSupplier_Result> REP_GetEdoGISMDAServiceItemForSupplier()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetEdoGISMDAServiceItemForSupplier().ToList();
            }
        }

        public IList<usp_SearchMDAServiceItemForRDMLoad_Result> REP_SearchMDAServiceItemDetails(MDA_Service_Items pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchMDAServiceItemForRDMLoad(pObjMDAServiceItem.MDAServiceItemReferenceNo, pObjMDAServiceItem.RevenueStreamName, pObjMDAServiceItem.RevenueSubStreamName, pObjMDAServiceItem.AssessmentItemCategoryName, pObjMDAServiceItem.AssessmentItemSubCategoryName, pObjMDAServiceItem.AgencyName, pObjMDAServiceItem.MDAServiceItemName, pObjMDAServiceItem.ComputationName, pObjMDAServiceItem.StrServiceAmount, pObjMDAServiceItem.StrPercentage, pObjMDAServiceItem.StrServiceBaseAmount, pObjMDAServiceItem.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchMDAServiceItem(MDA_Service_Items pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["MDAServiceItemList"] = _db.usp_SearchMDAServiceItem(pObjMDAServiceItem.WhereCondition, pObjMDAServiceItem.OrderBy, pObjMDAServiceItem.OrderByDirection, pObjMDAServiceItem.PageNumber, pObjMDAServiceItem.PageSize, pObjMDAServiceItem.MainFilter,
                                                                        pObjMDAServiceItem.MDAServiceItemReferenceNo, pObjMDAServiceItem.RevenueStreamName, pObjMDAServiceItem.RevenueSubStreamName, pObjMDAServiceItem.AssessmentItemCategoryName, pObjMDAServiceItem.AssessmentItemSubCategoryName, pObjMDAServiceItem.AgencyName, pObjMDAServiceItem.MDAServiceItemName, pObjMDAServiceItem.ComputationName, pObjMDAServiceItem.StrServiceAmount, pObjMDAServiceItem.StrPercentage, pObjMDAServiceItem.StrServiceBaseAmount, pObjMDAServiceItem.ActiveText).ToList()
                };
                string a = pObjMDAServiceItem.MDAServiceItemReferenceNo ?? "", b = "", c = "", d = "", e = "", f = "",g="";


                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(MDAServiceItemID) FROM MDA_Service_Items").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(MDAServiceItemID) ");
                sbFilteredCountQuery.Append(" FROM MDA_Service_Items sitem ");
                sbFilteredCountQuery.Append(" INNER JOIN Revenue_Stream rstrm ON sitem.RevenueStreamID = rstrm.RevenueStreamID ");
                sbFilteredCountQuery.Append(" INNER JOIN Revenue_SubStream rsstrm ON sitem.RevenueSubStreamID = rsstrm.RevenueSubStreamID ");
                sbFilteredCountQuery.Append(" INNER JOIN Assessment_Item_Category aicat  ON sitem.AssessmentItemCategoryID = aicat.AssessmentItemCategoryID ");
                sbFilteredCountQuery.Append(" INNER JOIN Assessment_Item_SubCategory aiscat ON sitem.AssessmentItemSubCategoryID = aiscat.AssessmentItemSubCategoryID ");
                sbFilteredCountQuery.Append(" INNER JOIN Agencies agncy ON sitem.AgencyID = agncy.AgencyID ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_Computation comp ON sitem.ComputationID = comp.ComputationID  WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjMDAServiceItem.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjMDAServiceItem.MainFilter),
                    new SqlParameter("@MDAServiceItemReferenceNo",pObjMDAServiceItem.MDAServiceItemReferenceNo ?? ""),
                    new SqlParameter("@RevenueStreamName",pObjMDAServiceItem.RevenueStreamName ?? ""),
                    new SqlParameter("@RevenueSubStreamName ",pObjMDAServiceItem.RevenueSubStreamName ?? "" ),
                    new SqlParameter("@AssessmentItemCategoryName ",pObjMDAServiceItem.AssessmentItemCategoryName ?? ""),
                    new SqlParameter("@AssessmentItemSubCategoryName ",pObjMDAServiceItem.AssessmentItemSubCategoryName ?? ""),
                    new SqlParameter("@AgencyName",pObjMDAServiceItem.AgencyName ?? ""),
                    new SqlParameter("@MDAServiceItemName",pObjMDAServiceItem.MDAServiceItemName ?? ""),
                    new SqlParameter("@ComputationName",pObjMDAServiceItem.ComputationName ?? ""),
                    new SqlParameter("@ServiceAmount",pObjMDAServiceItem.StrServiceAmount ?? "" ),
                    new SqlParameter("@Percentage",pObjMDAServiceItem.StrPercentage ?? ""),
                    new SqlParameter("@ServiceBaseAmount",pObjMDAServiceItem.StrServiceBaseAmount ?? ""),
                    new SqlParameter("@ActiveText",pObjMDAServiceItem.ActiveText ?? "")

                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }
    }
}
