using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ITaxOfficeRepository
    {
        usp_GetTaxOfficeList_Result REP_GetTaxOfficeDetails(Tax_Offices pObjTaxOffice);
        usp_GetTaxOfficeListNew_Result REP_GetTaxOfficeDetailsNew(Tax_Offices pObjTaxOffice);
        IList<DropDownListResult> REP_GetTaxOfficeDropDownList(Tax_Offices pObjTaxOffice);
        IList<usp_GetTaxOfficeList_Result> REP_GetTaxOfficeList(Tax_Offices pObjTaxOffice);
        FuncResponse REP_InsertUpdateTaxOffice(Tax_Offices pObjTaxOffice);
        FuncResponse REP_UpdateStatus(Tax_Offices pObjTaxOffice);

        FuncResponse REP_UpdateTaxOfficeAddress(Tax_Offices pObjTaxOffice);

        FuncResponse REP_InsertUpdateTaxOfficeTarget(IList<MAP_TaxOffice_Target> plstTaxOfficeTarget);
        IList<usp_GetTaxOfficeTargetList_Result> REP_GetTaxOfficeTarget(MAP_TaxOffice_Target pObjTarget);
    }
}