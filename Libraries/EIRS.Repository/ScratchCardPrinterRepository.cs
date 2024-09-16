using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public class ScratchCardPrinterRepository : IScratchCardPrinterRepository
    {
        EIRSEntities _db;
        public FuncResponse REP_InsertUpdateScratchCardPrinter(Scratch_Card_Printer pObjScratchCardPrinter)
        {
            using (_db = new EIRSEntities())
            {
                Scratch_Card_Printer mObjInsertUpdateScratchCardPrinter;
                FuncResponse mObjFuncRespsonse = new FuncResponse();
                if (pObjScratchCardPrinter == null)
                {
                    mObjFuncRespsonse.Success = false;
                    mObjFuncRespsonse.Message = "No Data";
                    return mObjFuncRespsonse;
                }
                else
                {
                    var vduplicateScratchCardPrinter = (from ScrPrinter in _db.Scratch_Card_Printer where ScrPrinter.ScratchCardPrinterName == pObjScratchCardPrinter.ScratchCardPrinterName && ScrPrinter.ScratchCardPrinterID != pObjScratchCardPrinter.ScratchCardPrinterID select ScrPrinter);
                    if (vduplicateScratchCardPrinter != null && vduplicateScratchCardPrinter.Count() > 0)
                    {
                        mObjFuncRespsonse.Success = false;
                        mObjFuncRespsonse.Message = "Scratch Card Printer already exists";
                        return mObjFuncRespsonse;
                    }
                    else
                    {
                        // Update Scratch Card Printer
                        if (pObjScratchCardPrinter.ScratchCardPrinterID != 0)
                        {
                            mObjInsertUpdateScratchCardPrinter = (from ScrPrinter in _db.Scratch_Card_Printer where (ScrPrinter.ScratchCardPrinterID == pObjScratchCardPrinter.ScratchCardPrinterID) select ScrPrinter).FirstOrDefault();

                            if (mObjInsertUpdateScratchCardPrinter != null)
                            {
                                mObjInsertUpdateScratchCardPrinter.ModifiedBy = pObjScratchCardPrinter.ModifiedBy;
                                mObjInsertUpdateScratchCardPrinter.ModifiedDate = pObjScratchCardPrinter.ModifiedDate;
                            }
                            else
                            {
                                mObjInsertUpdateScratchCardPrinter = new Scratch_Card_Printer()
                                {
                                    CreatedBy = pObjScratchCardPrinter.CreatedBy,
                                    CreatedDate = pObjScratchCardPrinter.CreatedDate
                                };
                            }
                        }
                        // Add Scratch Card Printer
                        else
                        {
                            mObjInsertUpdateScratchCardPrinter = new Scratch_Card_Printer()
                            {
                                CreatedBy = pObjScratchCardPrinter.CreatedBy,
                                CreatedDate = pObjScratchCardPrinter.CreatedDate,
                            };
                        }
                        mObjInsertUpdateScratchCardPrinter.Active = pObjScratchCardPrinter.Active;
                        mObjInsertUpdateScratchCardPrinter.ScratchCardPrinterName = pObjScratchCardPrinter.ScratchCardPrinterName;
                        mObjInsertUpdateScratchCardPrinter.CompanyID = pObjScratchCardPrinter.CompanyID;
                        mObjInsertUpdateScratchCardPrinter.AgreedUnitPrice = pObjScratchCardPrinter.AgreedUnitPrice;

                        if (pObjScratchCardPrinter.ScratchCardPrinterID == 0)
                        {
                            _db.Scratch_Card_Printer.Add(mObjInsertUpdateScratchCardPrinter);
                        }

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncRespsonse.Success = true;
                            mObjFuncRespsonse.Message = pObjScratchCardPrinter.ScratchCardPrinterID == 0 ? "Added Successfully" : "Updated Successfully";
                        }
                        catch (Exception ex)
                        {
                            mObjFuncRespsonse.Success = false;
                            mObjFuncRespsonse.Exception = ex;
                            mObjFuncRespsonse.Message = pObjScratchCardPrinter.ScratchCardPrinterID == 0 ? "Addition Failed" : "Updation Failed";
                        }
                        return mObjFuncRespsonse;
                    }
                }
            }
        }


        public IList<usp_GetScratchCardPrinterList_Result> REP_GetScratchCardPrinterList(Scratch_Card_Printer pObjScratchCardPrinter)
        {
            using (_db = new EIRSEntities())
            {
                var vlstScratchCardPrinter = _db.usp_GetScratchCardPrinterList(pObjScratchCardPrinter.ScratchCardPrinterID, pObjScratchCardPrinter.ScratchCardPrinterName, pObjScratchCardPrinter.ScratchCardPrinterIds, pObjScratchCardPrinter.intStatus, pObjScratchCardPrinter.IncludeScratchCardPrinterIds, pObjScratchCardPrinter.ExcludeScratchCardPrinterIds,pObjScratchCardPrinter.CompanyID).ToList();
                return vlstScratchCardPrinter;
            }
        }

        public usp_GetScratchCardPrinterList_Result REP_GetScratchCardPrinterDetails(Scratch_Card_Printer pObjScratchCardPrinter)
        {
            using (_db = new EIRSEntities())
            {
                var vScratchCardPrinterDetails = _db.usp_GetScratchCardPrinterList(pObjScratchCardPrinter.ScratchCardPrinterID, pObjScratchCardPrinter.ScratchCardPrinterName, pObjScratchCardPrinter.ScratchCardPrinterIds, pObjScratchCardPrinter.intStatus, pObjScratchCardPrinter.IncludeScratchCardPrinterIds, pObjScratchCardPrinter.ExcludeScratchCardPrinterIds,pObjScratchCardPrinter.CompanyID).FirstOrDefault();
                return vScratchCardPrinterDetails;
            }
        }

        public FuncResponse REP_UpdateStatus(Scratch_Card_Printer pObjScratchCardPrinter)
        {
            using (_db = new EIRSEntities())
            {
                Scratch_Card_Printer mObjInsertUpdateScratchCardPrinter; //Scratch Card Printer Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Update Load Scratch Card Printer
                if (pObjScratchCardPrinter.ScratchCardPrinterID != 0)
                {
                    mObjInsertUpdateScratchCardPrinter = (from NotifType in _db.Scratch_Card_Printer
                                                        where NotifType.ScratchCardPrinterID == pObjScratchCardPrinter.ScratchCardPrinterID
                                                        select NotifType).FirstOrDefault();

                    if (mObjInsertUpdateScratchCardPrinter != null)
                    {
                        mObjInsertUpdateScratchCardPrinter.Active = !mObjInsertUpdateScratchCardPrinter.Active;
                        mObjInsertUpdateScratchCardPrinter.ModifiedBy = pObjScratchCardPrinter.ModifiedBy;
                        mObjInsertUpdateScratchCardPrinter.ModifiedDate = pObjScratchCardPrinter.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Scratch Card Printer Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetScratchCardPrinterList(0, pObjScratchCardPrinter.ScratchCardPrinterName, pObjScratchCardPrinter.ScratchCardPrinterIds, pObjScratchCardPrinter.intStatus, pObjScratchCardPrinter.IncludeScratchCardPrinterIds, pObjScratchCardPrinter.ExcludeScratchCardPrinterIds,pObjScratchCardPrinter.CompanyID).ToList();
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Scratch Card Printer Updation Failed";
                        }
                    }
                }
                return mObjFuncResponse;
            }
        }

    }
}
