using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        EIRSEntities _db;

        public FuncResponse<Vehicle> REP_InsertUpdateVehicle(Vehicle pObjVehicle)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle mObjInsertUpdateVehicle; //Vehicle Insert Update Object
                FuncResponse<Vehicle> mObjFuncResponse = new FuncResponse<Vehicle>(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from vch in _db.Vehicles
                                       where vch.VehicleRegNumber == pObjVehicle.VehicleRegNumber && vch.VehicleID != pObjVehicle.VehicleID
                                       select vch);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Vehicle with same registration number already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Vehicle

                if (pObjVehicle.VehicleID != 0)
                {
                    mObjInsertUpdateVehicle = (from Vehicle in _db.Vehicles
                                               where Vehicle.VehicleID == pObjVehicle.VehicleID
                                               select Vehicle).FirstOrDefault();

                    if (mObjInsertUpdateVehicle != null)
                    {
                        mObjInsertUpdateVehicle.ModifiedBy = pObjVehicle.ModifiedBy;
                        mObjInsertUpdateVehicle.ModifiedDate = pObjVehicle.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateVehicle = new Vehicle();
                        mObjInsertUpdateVehicle.CreatedBy = pObjVehicle.CreatedBy;
                        mObjInsertUpdateVehicle.CreatedDate = pObjVehicle.CreatedDate;
                    }
                }
                else // Else Insert Vehicle Completion
                {
                    mObjInsertUpdateVehicle = new Vehicle();
                    mObjInsertUpdateVehicle.CreatedBy = pObjVehicle.CreatedBy;
                    mObjInsertUpdateVehicle.CreatedDate = pObjVehicle.CreatedDate;
                   
                }

                mObjInsertUpdateVehicle.VehicleRegNumber = pObjVehicle.VehicleRegNumber;
                mObjInsertUpdateVehicle.AssetTypeID = pObjVehicle.AssetTypeID;
                mObjInsertUpdateVehicle.VehicleTypeID = pObjVehicle.VehicleTypeID;
                mObjInsertUpdateVehicle.VehicleSubTypeID = pObjVehicle.VehicleSubTypeID;
                mObjInsertUpdateVehicle.LGAID = pObjVehicle.LGAID;
                mObjInsertUpdateVehicle.VehiclePurposeID = pObjVehicle.VehiclePurposeID;
                mObjInsertUpdateVehicle.VehicleFunctionID = pObjVehicle.VehicleFunctionID;
                mObjInsertUpdateVehicle.VehicleOwnershipID = pObjVehicle.VehicleOwnershipID;
                mObjInsertUpdateVehicle.VehicleDescription = pObjVehicle.VehicleDescription;
                mObjInsertUpdateVehicle.VIN = pObjVehicle.VIN;
                mObjInsertUpdateVehicle.Active = pObjVehicle.Active;

                if (pObjVehicle.VehicleID == 0)
                {
                    _db.Vehicles.Add(mObjInsertUpdateVehicle);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjVehicle.VehicleID == 0)
                        mObjFuncResponse.Message = "Vehicle Added Successfully";
                    else
                        mObjFuncResponse.Message = "Vehicle Updated Successfully";

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateVehicle;

                    //if vehicle is Updated , Check For Profile Update
                    if (pObjVehicle.VehicleID > 0)
                    {
                        var vTPAList = (from tpa in _db.MAP_TaxPayer_Asset
                                        where tpa.AssetID == pObjVehicle.VehicleID && tpa.AssetTypeID == pObjVehicle.AssetTypeID
                                        select tpa).ToList();

                        foreach (var item in vTPAList)
                        {

                            //Delete Profile Information
                            var vDeleteTaxPayerAssetProfile = (from tpap in _db.MAP_TaxPayer_Asset_Profile
                                                               where tpap.TPAID == item.TPAID
                                                               select tpap);

                            _db.MAP_TaxPayer_Asset_Profile.RemoveRange(vDeleteTaxPayerAssetProfile);

                            _db.SaveChanges();

                            //Update Profile
                            _db.usp_InsertProfileInformation(item.TPAID);
                        }
                    }

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjVehicle.VehicleID == 0)
                        mObjFuncResponse.Message = "Vehicle Addition Failed";
                    else
                        mObjFuncResponse.Message = "Vehicle Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<vw_Vehicle> REP_GetVehicleList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Vehicle.ToList();
            }
        }

        public IList<usp_GetVehicleList_Result> REP_GetVehicleList(Vehicle pObjVehicle)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleList(pObjVehicle.VehicleID, pObjVehicle.VehicleRIN, pObjVehicle.VehicleRegNumber, pObjVehicle.VehicleDescription, pObjVehicle.LGAID, pObjVehicle.intStatus).ToList();
            }
        }

        public usp_GetVehicleList_Result REP_GetVehicleDetails(Vehicle pObjVehicle)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleList(pObjVehicle.VehicleID, pObjVehicle.VehicleRIN, pObjVehicle.VehicleRegNumber, pObjVehicle.VehicleDescription, pObjVehicle.LGAID, pObjVehicle.intStatus).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetVehicleDropDownList(Vehicle pObjVehicle)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from vh in _db.usp_GetVehicleList(pObjVehicle.VehicleID, pObjVehicle.VehicleRIN, pObjVehicle.VehicleRegNumber, pObjVehicle.VehicleDescription, pObjVehicle.LGAID, pObjVehicle.intStatus)
                               select new DropDownListResult()
                               {
                                   id = vh.VehicleID.GetValueOrDefault(),
                                   text = vh.VehicleRIN + " - " + vh.VehicleRegNumber
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Vehicle pObjVehicle)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle mObjInsertUpdateVehicle; //Vehicle Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Vehicle
                if (pObjVehicle.VehicleID != 0)
                {
                    mObjInsertUpdateVehicle = (from bcomp in _db.Vehicles
                                               where bcomp.VehicleID == pObjVehicle.VehicleID
                                               select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateVehicle != null)
                    {
                        if (mObjInsertUpdateVehicle.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vCompanyList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join comp in _db.Companies on new { CompanyID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { comp.CompanyID, comp.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateVehicle.VehicleID && tpa.AssetTypeID == mObjInsertUpdateVehicle.AssetTypeID && comp.Active == true
                                                select tpa);
                            if (vCompanyList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vIndividualList = (from tpa in _db.MAP_TaxPayer_Asset
                                                   join ind in _db.Individuals on new { IndividualID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { ind.IndividualID, ind.TaxPayerTypeID }
                                                   where tpa.AssetID == mObjInsertUpdateVehicle.VehicleID && tpa.AssetTypeID == mObjInsertUpdateVehicle.AssetTypeID && ind.Active == true
                                                   select tpa);
                            if (vIndividualList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vGovernmentList = (from tpa in _db.MAP_TaxPayer_Asset
                                                   join gov in _db.Governments on new { GovernmentID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { gov.GovernmentID, gov.TaxPayerTypeID }
                                                   where tpa.AssetID == mObjInsertUpdateVehicle.VehicleID && tpa.AssetTypeID == mObjInsertUpdateVehicle.AssetTypeID && gov.Active == true
                                                   select tpa);
                            if (vGovernmentList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vSpecialList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join sp in _db.Specials on new { SpecialID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { sp.SpecialID, sp.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateVehicle.VehicleID && tpa.AssetTypeID == mObjInsertUpdateVehicle.AssetTypeID && sp.Active == true
                                                select tpa);
                            if (vSpecialList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }
                        }

                        mObjInsertUpdateVehicle.Active = !mObjInsertUpdateVehicle.Active;
                        mObjInsertUpdateVehicle.ModifiedBy = pObjVehicle.ModifiedBy;
                        mObjInsertUpdateVehicle.ModifiedDate = pObjVehicle.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Vehicle Updated Successfully";
                            //mObjFuncResponse.AdditionalData = _db.usp_GetVehicleList(0, pObjVehicle.VehicleRIN, pObjVehicle.VehicleRegNumber, pObjVehicle.VehicleDescription, pObjVehicle.LGAID, pObjVehicle.intStatus).ToList();
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Vehicle Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_SearchVehicleByRegNumber_Result> REP_SearchVehicleByRegNumber(string pStrRegNumber)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchVehicleByRegNumber(pStrRegNumber).ToList();
            }
        }

        public IList<usp_GetVehicleTaxPayerList_Result> REP_GetVehicleTaxPayerList(int pIntVehicleID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleTaxPayerList(pIntVehicleID).ToList();
            }
        }

        //public IList<usp_GetVehicleChart_Result> REP_GetVehicleChart(int pIntChartType)
        //{
        //    using (_db = new EIRSEntities())
        //    {
        //        return _db.usp_GetVehicleChart(pIntChartType).ToList();
        //    }
        //}

        public IList<usp_SearchVehicleForRDMLoad_Result> REP_SearchVehicleDetails(Vehicle pObjVehicle)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchVehicleForRDMLoad(pObjVehicle.VehicleRIN,pObjVehicle.VehicleRegNumber,pObjVehicle.VIN,pObjVehicle.VehicleTypeName,
                               pObjVehicle.VehicleSubTypeName,pObjVehicle.LGAName,pObjVehicle.VehiclePurposeName,pObjVehicle.VehicleFunctionName,pObjVehicle.VehicleOwnershipName,
                               pObjVehicle.VehicleDescription,pObjVehicle.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchVehicle(Vehicle pObjVehicle)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["VehicleList"] = _db.usp_SearchVehicle(pObjVehicle.WhereCondition, pObjVehicle.OrderBy, pObjVehicle.OrderByDirection, pObjVehicle.PageNumber, pObjVehicle.PageSize, pObjVehicle.MainFilter, 
                    pObjVehicle.VehicleRIN, pObjVehicle.VehicleRegNumber, pObjVehicle.VIN, pObjVehicle.VehicleTypeName,
                               pObjVehicle.VehicleSubTypeName, pObjVehicle.LGAName, pObjVehicle.VehiclePurposeName, pObjVehicle.VehicleFunctionName, pObjVehicle.VehicleOwnershipName,
                               pObjVehicle.VehicleDescription, pObjVehicle.ActiveText).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(VehicleID) FROM Vehicle").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(VehicleID) ");
                sbFilteredCountQuery.Append(" FROM Vehicle veh ");
                sbFilteredCountQuery.Append(" INNER JOIN LGA lga ON veh.LGAID = lga.LGAID ");
                sbFilteredCountQuery.Append(" INNER JOIN Asset_Types atype ON veh.AssetTypeID = atype.AssetTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Vehicle_Types vtype ON veh.VehicleTypeID = vtype.VehicleTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Vehicle_SubTypes vstype ON veh.VehicleSubTypeID = vstype.VehicleSubTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Vehicle_Purpose vpurp ON veh.VehiclePurposeID = vpurp.VehiclePurposeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Vehicle_Ownership vowner ON veh.VehicleOwnershipID = vowner.VehicleOwnershipID ");
                sbFilteredCountQuery.Append(" INNER JOIN Vehicle_Function vfunc ON veh.VehicleFunctionID = vfunc.VehicleFunctionID  WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjVehicle.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjVehicle.MainFilter),
                    new SqlParameter("@VehicleRIN",pObjVehicle.VehicleRIN),
                    new SqlParameter("@VehicleRegNumber",pObjVehicle.VehicleRegNumber),
                    new SqlParameter("@VIN",pObjVehicle.VIN),
                    new SqlParameter("@VehicleTypeName",pObjVehicle.VehicleTypeName),
                    new SqlParameter("@VehicleSubTypeName",pObjVehicle.VehicleSubTypeName),
                    new SqlParameter("@LGAName",pObjVehicle.LGAName),
                    new SqlParameter("@VehiclePurposeName",pObjVehicle.VehiclePurposeName),
                    new SqlParameter("@VehicleFunctionName",pObjVehicle.VehicleFunctionName),
                    new SqlParameter("@VehicleOwnershipName",pObjVehicle.VehicleOwnershipName),
                    new SqlParameter("@VehicleDescription",pObjVehicle.VehicleDescription),
                    new SqlParameter("@ActiveText",pObjVehicle.ActiveText)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public IDictionary<string, object> REP_SearchVehicleForSideMenu(Vehicle pObjVehicle)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["VehicleList"] = _db.usp_SearchVehicleForSideMenu(pObjVehicle.WhereCondition, pObjVehicle.OrderBy, pObjVehicle.OrderByDirection, pObjVehicle.PageNumber, pObjVehicle.PageSize, pObjVehicle.MainFilter).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(VehicleID) FROM Vehicle veh INNER JOIN Vehicle_SubTypes vstype ON veh.VehicleSubTypeID = vstype.VehicleSubTypeID INNER JOIN Vehicle_Purpose vpurp ON veh.VehiclePurposeID = vpurp.VehiclePurposeID").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(VehicleID) ");
                sbFilteredCountQuery.Append(" FROM Vehicle veh ");
                sbFilteredCountQuery.Append(" INNER JOIN Vehicle_SubTypes vstype ON veh.VehicleSubTypeID = vstype.VehicleSubTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Vehicle_Purpose vpurp ON veh.VehiclePurposeID = vpurp.VehiclePurposeID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjVehicle.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjVehicle.MainFilter)
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
