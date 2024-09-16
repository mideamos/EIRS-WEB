using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public class TaxOfficeRepository : ITaxOfficeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateTaxOffice(Tax_Offices pObjTaxOffice)
        {
            using (_db = new EIRSEntities())
            {
                Tax_Offices mObjInsertUpdateTaxOffice; //Tax Offices Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from toff in _db.Tax_Offices
                                       where toff.TaxOfficeName == pObjTaxOffice.TaxOfficeName && toff.TaxOfficeID != pObjTaxOffice.TaxOfficeID
                                       select toff);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Tax Office already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Tax Offices
                if (pObjTaxOffice.TaxOfficeID != 0)
                {
                    mObjInsertUpdateTaxOffice = (from toff in _db.Tax_Offices
                                                 where toff.TaxOfficeID == pObjTaxOffice.TaxOfficeID
                                                 select toff).FirstOrDefault();

                    if (mObjInsertUpdateTaxOffice != null)
                    {
                        mObjInsertUpdateTaxOffice.ModifiedBy = pObjTaxOffice.ModifiedBy;
                        mObjInsertUpdateTaxOffice.ModifiedDate = pObjTaxOffice.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateTaxOffice = new Tax_Offices();
                        mObjInsertUpdateTaxOffice.CreatedBy = pObjTaxOffice.CreatedBy;
                        mObjInsertUpdateTaxOffice.CreatedDate = pObjTaxOffice.CreatedDate;
                    }
                } 
                else // Else Insert Tax Offices
                {
                    mObjInsertUpdateTaxOffice = new Tax_Offices();
                    mObjInsertUpdateTaxOffice.CreatedBy = pObjTaxOffice.CreatedBy;
                    mObjInsertUpdateTaxOffice.CreatedDate = pObjTaxOffice.CreatedDate;
                }

                mObjInsertUpdateTaxOffice.TaxOfficeName = pObjTaxOffice.TaxOfficeName;
                mObjInsertUpdateTaxOffice.AddressTypeID = pObjTaxOffice.AddressTypeID;
                mObjInsertUpdateTaxOffice.BuildingID = pObjTaxOffice.BuildingID;
                mObjInsertUpdateTaxOffice.ZoneId = pObjTaxOffice.ZoneId;
                mObjInsertUpdateTaxOffice.OfficeManagerID = pObjTaxOffice.OfficeManagerID;
                mObjInsertUpdateTaxOffice.IncomeDirector = pObjTaxOffice.IncomeDirector;
                mObjInsertUpdateTaxOffice.Approver1 = pObjTaxOffice.Approver1;
                mObjInsertUpdateTaxOffice.Approver2 = pObjTaxOffice.Approver2;
                mObjInsertUpdateTaxOffice.Approver3 = pObjTaxOffice.Approver3;
                mObjInsertUpdateTaxOffice.Active = pObjTaxOffice.Active;

                if (pObjTaxOffice.TaxOfficeID == 0)
                {
                    _db.Tax_Offices.Add(mObjInsertUpdateTaxOffice);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjTaxOffice.TaxOfficeID == 0)
                    {
                        mObjFuncResponse.Message = "Tax Office Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Tax Office Updated Successfully";
                    }
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjTaxOffice.TaxOfficeID == 0)
                    {
                        mObjFuncResponse.Message = "Tax Office Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Tax Office Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTaxOfficeList_Result> REP_GetTaxOfficeList(Tax_Offices pObjTaxOffice)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxOfficeList(pObjTaxOffice.TaxOfficeName, pObjTaxOffice.TaxOfficeID, pObjTaxOffice.AddressTypeID, pObjTaxOffice.TaxOfficeIds, pObjTaxOffice.intStatus, pObjTaxOffice.IncludeTaxOfficeIds, pObjTaxOffice.ExcludeTaxOfficeIds).ToList();
            }
        }

        public usp_GetTaxOfficeList_Result REP_GetTaxOfficeDetails(Tax_Offices pObjTaxOffice)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxOfficeList(pObjTaxOffice.TaxOfficeName, pObjTaxOffice.TaxOfficeID, pObjTaxOffice.AddressTypeID, pObjTaxOffice.TaxOfficeIds, pObjTaxOffice.intStatus, pObjTaxOffice.IncludeTaxOfficeIds, pObjTaxOffice.ExcludeTaxOfficeIds).FirstOrDefault();
            }
        }
        public usp_GetTaxOfficeListNew_Result REP_GetTaxOfficeDetailsNew(Tax_Offices pObjTaxOffice)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxOfficeListNew(pObjTaxOffice.TaxOfficeName, pObjTaxOffice.TaxOfficeID, pObjTaxOffice.AddressTypeID, pObjTaxOffice.TaxOfficeIds, pObjTaxOffice.intStatus, pObjTaxOffice.IncludeTaxOfficeIds, pObjTaxOffice.ExcludeTaxOfficeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetTaxOfficeDropDownList(Tax_Offices pObjTaxOffice)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from toff in _db.usp_GetTaxOfficeList(pObjTaxOffice.TaxOfficeName, pObjTaxOffice.TaxOfficeID, pObjTaxOffice.AddressTypeID, pObjTaxOffice.TaxOfficeIds, pObjTaxOffice.intStatus, pObjTaxOffice.IncludeTaxOfficeIds, pObjTaxOffice.ExcludeTaxOfficeIds)
                               select new DropDownListResult()
                               {
                                   id = toff.TaxOfficeID.GetValueOrDefault(),
                                   text = toff.TaxOfficeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Tax_Offices pObjTaxOffice)
        {
            using (_db = new EIRSEntities())
            {
                Tax_Offices mObjInsertUpdateTaxOffice; //Tax Offices Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load TaxOffice
                if (pObjTaxOffice.TaxOfficeID != 0)
                {
                    mObjInsertUpdateTaxOffice = (from toff in _db.Tax_Offices
                                                 where toff.TaxOfficeID == pObjTaxOffice.TaxOfficeID
                                                 select toff).FirstOrDefault();

                    if (mObjInsertUpdateTaxOffice != null)
                    {
                        mObjInsertUpdateTaxOffice.Active = !mObjInsertUpdateTaxOffice.Active;
                        mObjInsertUpdateTaxOffice.ModifiedBy = pObjTaxOffice.ModifiedBy;
                        mObjInsertUpdateTaxOffice.ModifiedDate = pObjTaxOffice.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Tax Office Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetTaxOfficeList(pObjTaxOffice.TaxOfficeName, 0, pObjTaxOffice.AddressTypeID, pObjTaxOffice.TaxOfficeIds, pObjTaxOffice.intStatus, pObjTaxOffice.IncludeTaxOfficeIds, pObjTaxOffice.ExcludeTaxOfficeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Tax Office Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateTaxOfficeAddress(Tax_Offices pObjTaxOffice)
        {
            using (_db = new EIRSEntities())
            {
                Tax_Offices mObjUpdateTaxOffice = _db.Tax_Offices.Find(pObjTaxOffice.TaxOfficeID);
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                if (mObjUpdateTaxOffice != null)
                {
                    mObjUpdateTaxOffice.AddressTypeID = pObjTaxOffice.AddressTypeID;
                    mObjUpdateTaxOffice.ZoneId = pObjTaxOffice.ZoneId;
                    mObjUpdateTaxOffice.BuildingID = pObjTaxOffice.BuildingID;

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Tax Office Updated Successfully";
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "Tax Office Updation Failed";
                    }
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Tax Office not found";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertUpdateTaxOfficeTarget(IList<MAP_TaxOffice_Target> plstTaxOfficeTarget)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                MAP_TaxOffice_Target mObjInsertUpdateTarget;

                foreach (var item in plstTaxOfficeTarget)
                {
                    if (item.TOTID > 0)
                    {
                        mObjInsertUpdateTarget = _db.MAP_TaxOffice_Target.Find(item.TOTID);

                        if (mObjInsertUpdateTarget != null)
                        {
                            mObjInsertUpdateTarget.ModifiedBy = item.ModifiedBy;
                            mObjInsertUpdateTarget.ModifiedDate = item.ModifiedDate;
                        }
                    }
                    else
                    {
                        mObjInsertUpdateTarget = new MAP_TaxOffice_Target()
                        {
                            CreatedBy = item.CreatedBy,
                            CreatedDate = item.CreatedDate
                        };
                    }

                    mObjInsertUpdateTarget.RevenueStreamID = item.RevenueStreamID;
                    mObjInsertUpdateTarget.TaxOfficeID = item.TaxOfficeID;
                    mObjInsertUpdateTarget.TaxYear = item.TaxYear;
                    mObjInsertUpdateTarget.TargetAmount = item.TargetAmount;

                    if (item.TOTID == 0)
                    {
                        _db.MAP_TaxOffice_Target.Add(mObjInsertUpdateTarget);
                    }
                }

                try
                {
                    _db.SaveChanges();

                    mObjFuncResponse.Message = "Target Set Successfully";
                    mObjFuncResponse.Success = true;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Exception = ex;
                    mObjFuncResponse.Message = ex.Message;
                    mObjFuncResponse.Success = false;
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTaxOfficeTargetList_Result> REP_GetTaxOfficeTarget(MAP_TaxOffice_Target pObjTarget)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxOfficeTargetList(pObjTarget.TaxOfficeID, pObjTarget.TaxYear).ToList();
            }
        }
    }
}
