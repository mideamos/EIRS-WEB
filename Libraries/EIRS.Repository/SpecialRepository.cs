using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class SpecialRepository : ISpecialRepository
    {
        EIRSEntities _db;

        public FuncResponse<Special> REP_InsertUpdateSpecial(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {
                Special mObjInsertUpdateSpecial; //Special Insert Update Object
                FuncResponse<Special> mObjFuncResponse = new FuncResponse<Special>(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from spl in _db.Specials
                                       where spl.SpecialTaxPayerName == pObjSpecial.SpecialTaxPayerName && spl.SpecialID != pObjSpecial.SpecialID
                                       select spl);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Special with same name already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Special

                if (pObjSpecial.SpecialID != 0)
                {
                    mObjInsertUpdateSpecial = (from spl in _db.Specials
                                               where spl.SpecialID == pObjSpecial.SpecialID
                                               select spl).FirstOrDefault();

                    if (mObjInsertUpdateSpecial != null)
                    {
                        if (pObjSpecial.Active == false && mObjInsertUpdateSpecial.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vBuildingList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bld in _db.Buildings on new { BuildingID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bld.BuildingID, bld.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && tpa.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && bld.Active == true
                                                 select tpa);
                            if (vBuildingList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vBusinessList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bus in _db.Businesses on new { BusinessID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bus.BusinessID, bus.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && tpa.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && bus.Active == true
                                                 select tpa);
                            if (vBusinessList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vLandList = (from tpa in _db.MAP_TaxPayer_Asset
                                             join lnd in _db.Lands on new { LandID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { lnd.LandID, lnd.AssetTypeID }
                                             where tpa.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && tpa.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && lnd.Active == true
                                             select tpa);
                            if (vLandList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vVechicleList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join vch in _db.Vehicles on new { VehicleID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { vch.VehicleID, vch.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && tpa.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && vch.Active == true
                                                 select tpa);

                            if (vVechicleList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vAssessmentCount = (from ast in _db.Assessments
                                                    where ast.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && ast.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && ast.Active == true && ast.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                    select ast);
                            if (vAssessmentCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled  assessment bill";
                                return mObjFuncResponse;
                            }

                            var vServiceBillCount = (from sb in _db.ServiceBills
                                                     where sb.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && sb.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && sb.Active == true && sb.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                     select sb);
                            if (vServiceBillCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled service bill";
                                return mObjFuncResponse;
                            }

                            var vPoABalance = _db.usp_GetWalletBalance(mObjInsertUpdateSpecial.TaxPayerTypeID, mObjInsertUpdateSpecial.SpecialID).FirstOrDefault();
                            if (vPoABalance.GetValueOrDefault() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "PoA Balance is more than 0";
                                return mObjFuncResponse;
                            }
                        }

                        if (pObjSpecial.TaxOfficeID != mObjInsertUpdateSpecial.TaxOfficeID)
                        {
                            mObjInsertUpdateSpecial.TaxOfficerID = null;
                        }

                        mObjInsertUpdateSpecial.ModifiedBy = pObjSpecial.ModifiedBy;
                        mObjInsertUpdateSpecial.ModifiedDate = pObjSpecial.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateSpecial = new Special();
                        mObjInsertUpdateSpecial.CreatedBy = pObjSpecial.CreatedBy;
                        mObjInsertUpdateSpecial.CreatedDate = pObjSpecial.CreatedDate;
                    }
                }
                else // Else Insert Special Completion
                {
                    mObjInsertUpdateSpecial = new Special
                    {
                        CreatedBy = pObjSpecial.CreatedBy,
                        CreatedDate = pObjSpecial.CreatedDate
                    };
                }

                mObjInsertUpdateSpecial.SpecialTaxPayerName = pObjSpecial.SpecialTaxPayerName;
                mObjInsertUpdateSpecial.Description = pObjSpecial.Description;
                mObjInsertUpdateSpecial.TIN = pObjSpecial.TIN;
                mObjInsertUpdateSpecial.TaxOfficeID = pObjSpecial.TaxOfficeID;
                mObjInsertUpdateSpecial.TaxPayerTypeID = pObjSpecial.TaxPayerTypeID;
                mObjInsertUpdateSpecial.ContactNumber = pObjSpecial.ContactNumber;
                mObjInsertUpdateSpecial.ContactEmail = pObjSpecial.ContactEmail;
                mObjInsertUpdateSpecial.ContactName = pObjSpecial.ContactName;
                mObjInsertUpdateSpecial.NotificationMethodID = pObjSpecial.NotificationMethodID;
                mObjInsertUpdateSpecial.Active = pObjSpecial.Active;

                if (pObjSpecial.SpecialID == 0)
                {
                    _db.Specials.Add(mObjInsertUpdateSpecial);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjSpecial.SpecialID == 0)
                        mObjFuncResponse.Message = "Special Added Successfully";
                    else
                        mObjFuncResponse.Message = "Special Updated Successfully";

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateSpecial;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjSpecial.SpecialID == 0)
                        mObjFuncResponse.Message = "Special Addition Failed";
                    else
                        mObjFuncResponse.Message = "Special Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetSpecialList_Result> REP_GetSpecialList(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSpecialList(pObjSpecial.SpecialID, pObjSpecial.SpecialTaxPayerName, pObjSpecial.TIN, pObjSpecial.SpecialRIN, pObjSpecial.ContactNumber, pObjSpecial.intStatus).ToList();
            }
        }

        public IList<vw_Special> REP_GetSpecialList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Special.ToList();
            }
        }

        public IList<DropDownListResult> REP_GetSpecialDropDownList(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from spl in _db.usp_GetSpecialList(pObjSpecial.SpecialID, pObjSpecial.SpecialTaxPayerName, pObjSpecial.TIN, pObjSpecial.SpecialRIN, pObjSpecial.ContactNumber, pObjSpecial.intStatus)
                               select new DropDownListResult()
                               {
                                   id = spl.SpecialID.GetValueOrDefault(),
                                   text = spl.SpecialTaxPayerName
                               });

                return vResult.ToList();
            }
        }

        public IList<DropDownListResult> REP_GetSpecialDropDownList(string pStrSpecialName, int pIntTaxOfficeID = 0)
        {
            using (_db = new EIRSEntities())
            {
                if (pIntTaxOfficeID != 0)
                {
                    var vResult = (from sp in _db.vw_Special
                                   where (sp.SpecialTaxPayerName.ToLower().Contains(pStrSpecialName.ToLower()) || sp.SpecialRIN.ToLower().Contains(pStrSpecialName.ToLower()))
                                   && sp.TaxOfficeID == pIntTaxOfficeID
                                   select new DropDownListResult()
                                   {
                                       id = sp.SpecialID,
                                       text = sp.SpecialRIN + " - " + sp.SpecialTaxPayerName
                                   });

                    return vResult.ToList();

                }
                else
                {
                    var vResult = (from sp in _db.vw_Special
                                   where sp.SpecialTaxPayerName.ToLower().Contains(pStrSpecialName.ToLower()) || sp.SpecialRIN.ToLower().Contains(pStrSpecialName.ToLower())
                                   select new DropDownListResult()
                                   {
                                       id = sp.SpecialID,
                                       text = sp.SpecialRIN + " - " + sp.SpecialTaxPayerName
                                   });

                    return vResult.ToList();
                }
            }
        }

        public usp_GetSpecialList_Result REP_GetSpecialDetails(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSpecialList(pObjSpecial.SpecialID, pObjSpecial.SpecialTaxPayerName, pObjSpecial.TIN, pObjSpecial.SpecialRIN, pObjSpecial.ContactNumber, pObjSpecial.intStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {
                Special mObjInsertUpdateSpecial; //Special Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Special
                if (pObjSpecial.SpecialID != 0)
                {
                    mObjInsertUpdateSpecial = (from spl in _db.Specials
                                               where spl.SpecialID == pObjSpecial.SpecialID
                                               select spl).FirstOrDefault();

                    if (mObjInsertUpdateSpecial != null)
                    {
                        if (mObjInsertUpdateSpecial.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vBuildingList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bld in _db.Buildings on new { BuildingID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bld.BuildingID, bld.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && tpa.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && bld.Active == true
                                                 select tpa);
                            if (vBuildingList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vBusinessList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bus in _db.Businesses on new { BusinessID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bus.BusinessID, bus.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && tpa.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && bus.Active == true
                                                 select tpa);
                            if (vBusinessList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vLandList = (from tpa in _db.MAP_TaxPayer_Asset
                                             join lnd in _db.Lands on new { LandID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { lnd.LandID, lnd.AssetTypeID }
                                             where tpa.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && tpa.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && lnd.Active == true
                                             select tpa);
                            if (vLandList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vVechicleList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join vch in _db.Vehicles on new { VehicleID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { vch.VehicleID, vch.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && tpa.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && vch.Active == true
                                                 select tpa);

                            if (vVechicleList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vAssessmentCount = (from ast in _db.Assessments
                                                    where ast.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && ast.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && ast.Active == true && ast.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                    select ast);
                            if (vAssessmentCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled  assessment bill";
                                return mObjFuncResponse;
                            }

                            var vServiceBillCount = (from sb in _db.ServiceBills
                                                     where sb.TaxPayerID == mObjInsertUpdateSpecial.SpecialID && sb.TaxPayerTypeID == mObjInsertUpdateSpecial.TaxPayerTypeID && sb.Active == true && sb.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                     select sb);
                            if (vServiceBillCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled service bill";
                                return mObjFuncResponse;
                            }

                            var vPoABalance = _db.usp_GetWalletBalance(mObjInsertUpdateSpecial.TaxPayerTypeID, mObjInsertUpdateSpecial.SpecialID).FirstOrDefault();
                            if (vPoABalance.GetValueOrDefault() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "PoA Balance is more than 0";
                                return mObjFuncResponse;
                            }
                        }

                        mObjInsertUpdateSpecial.Active = !mObjInsertUpdateSpecial.Active;
                        mObjInsertUpdateSpecial.ModifiedBy = pObjSpecial.ModifiedBy;
                        mObjInsertUpdateSpecial.ModifiedDate = pObjSpecial.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Special Updated Successfully";
                            //mObjFuncResponse.AdditionalData = _db.usp_GetSpecialList(0, pObjSpecial.SpecialTaxPayerName, pObjSpecial.TIN, pObjSpecial.SpecialRIN, pObjSpecial.ContactNumber, pObjSpecial.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Special Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetSpecialAddressInformation_Result> REP_GetAddressInformation(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSpecialAddressInformation(pObjSpecial.SpecialID).ToList();
            }
        }

        public FuncResponse REP_InsertAddressInformation(MAP_Special_AddressInformation pObjAddressInformation)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                _db.MAP_Special_AddressInformation.Add(pObjAddressInformation);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Address Information Added Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Address Information Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse<IList<usp_GetSpecialAddressInformation_Result>> REP_RemoveAddressInformation(MAP_Special_AddressInformation pObjAddressInformation)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetSpecialAddressInformation_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetSpecialAddressInformation_Result>>(); //Return Object

                MAP_Special_AddressInformation mObjDeleteAddressInformation;

                mObjDeleteAddressInformation = _db.MAP_Special_AddressInformation.Find(pObjAddressInformation.SAIID);

                if (mObjDeleteAddressInformation == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Address Information Already Removed.";
                }
                else
                {
                    _db.MAP_Special_AddressInformation.Remove(mObjDeleteAddressInformation);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;

                        mObjFuncResponse.Message = "Address Information Removed Successfully";

                        mObjFuncResponse.AdditionalData = _db.usp_GetSpecialAddressInformation(pObjAddressInformation.SpecialID).ToList();
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_SearchSpecialForRDMLoad_Result> REP_SearchSpecialDetails(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchSpecialForRDMLoad(pObjSpecial.SpecialRIN, pObjSpecial.SpecialTaxPayerName, pObjSpecial.TIN, pObjSpecial.TaxOfficeName, pObjSpecial.TaxPayerTypeName, pObjSpecial.ContactName, pObjSpecial.ContactEmail, pObjSpecial.ContactNumber, pObjSpecial.Description, pObjSpecial.NotificationMethodName, pObjSpecial.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchSpecial(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["SpecialList"] = _db.usp_SearchSpecial(pObjSpecial.WhereCondition, pObjSpecial.OrderBy, pObjSpecial.OrderByDirection, pObjSpecial.PageNumber, pObjSpecial.PageSize, pObjSpecial.MainFilter,
                                                                   pObjSpecial.SpecialRIN, pObjSpecial.SpecialTaxPayerName, pObjSpecial.TIN, pObjSpecial.TaxOfficeName, pObjSpecial.TaxPayerTypeName, pObjSpecial.ContactName, pObjSpecial.ContactEmail, 
                                                                   pObjSpecial.ContactNumber, pObjSpecial.Description, pObjSpecial.NotificationMethodName, pObjSpecial.ActiveText).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(SpecialID) FROM Special").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(SpecialID) ");
                sbFilteredCountQuery.Append(" FROM Special sp ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Tax_Offices toff ON sp.TaxOfficeID = toff.TaxOfficeID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tptype ON sp.TaxPayerTypeID = tptype.TaxPayerTypeID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Notification_Method nmthd ON sp.NotificationMethodID = nmthd.NotificationMethodID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjSpecial.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@SpecialRIN", pObjSpecial.SpecialRIN),
                    new SqlParameter("@SpecialTaxPayerName", pObjSpecial.SpecialTaxPayerName),
                    new SqlParameter("@TIN", pObjSpecial.TIN),
                    new SqlParameter("@TaxOfficeName", pObjSpecial.TaxOfficeName),
                    new SqlParameter("@TaxPayerTypeName", pObjSpecial.TaxPayerTypeName),
                    new SqlParameter("@ContactName", pObjSpecial.ContactName),
                    new SqlParameter("@ContactEmail", pObjSpecial.ContactEmail),
                    new SqlParameter("@ContactNumber", pObjSpecial.ContactNumber),
                    new SqlParameter("@Description",pObjSpecial.Description),
                    new SqlParameter("@NotificationMethodName", pObjSpecial.NotificationMethodName),
                    new SqlParameter("@ActiveText", pObjSpecial.ActiveText),
                    new SqlParameter("@MainFilter", pObjSpecial.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public FuncResponse REP_UpdateTaxOfficer(Special pObjSpecial)
        {
            using (_db = new EIRSEntities())
            {
                Special mObjUpdateSpecial; //Special Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Special


                mObjUpdateSpecial = (from Special in _db.Specials
                                        where Special.SpecialID == pObjSpecial.SpecialID
                                        select Special).FirstOrDefault();

                if (mObjUpdateSpecial != null)
                {
                    mObjUpdateSpecial.ModifiedBy = pObjSpecial.ModifiedBy;
                    mObjUpdateSpecial.ModifiedDate = pObjSpecial.ModifiedDate;
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "No Record Found";
                    return mObjFuncResponse;
                }



                mObjUpdateSpecial.TaxOfficerID = pObjSpecial.TaxOfficerID;

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                }

                return mObjFuncResponse;
            }
        }
    }
}
