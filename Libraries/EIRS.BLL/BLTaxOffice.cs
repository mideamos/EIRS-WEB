using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLTaxOffice
    {
        ITaxOfficeRepository _TaxOfficeRepository;

        public BLTaxOffice()
        {
            _TaxOfficeRepository = new TaxOfficeRepository();
        }

        public IList<usp_GetTaxOfficeList_Result> BL_GetTaxOfficeList(Tax_Offices pObjTaxOffice)
        {
            return _TaxOfficeRepository.REP_GetTaxOfficeList(pObjTaxOffice);
        }

        public FuncResponse BL_InsertUpdateTaxOffice(Tax_Offices pObjTaxOffice)
        {
            return _TaxOfficeRepository.REP_InsertUpdateTaxOffice(pObjTaxOffice);
        }

        public usp_GetTaxOfficeList_Result BL_GetTaxOfficeDetails(Tax_Offices pObjTaxOffice)
        {
            return _TaxOfficeRepository.REP_GetTaxOfficeDetails(pObjTaxOffice);
        }
        public usp_GetTaxOfficeListNew_Result BL_GetTaxOfficeNewDetails(Tax_Offices pObjTaxOffice)
        {
            return _TaxOfficeRepository.REP_GetTaxOfficeDetailsNew(pObjTaxOffice);
        }

        public IList<DropDownListResult> BL_GetTaxOfficeDropDownList(Tax_Offices pObjTaxOffice)
        {
            return _TaxOfficeRepository.REP_GetTaxOfficeDropDownList(pObjTaxOffice);
        }

        public FuncResponse BL_UpdateStatus(Tax_Offices pObjTaxOffice)
        {
            return _TaxOfficeRepository.REP_UpdateStatus(pObjTaxOffice);
        }

        public FuncResponse REP_UpdateTaxOfficeAddress(Tax_Offices pObjTaxOffice)
        {
            return _TaxOfficeRepository.REP_UpdateTaxOfficeAddress(pObjTaxOffice);
        }

        public FuncResponse BL_InsertUpdateTaxOfficeTarget(IList<MAP_TaxOffice_Target> plstTaxOfficeTarget)
        {
            return _TaxOfficeRepository.REP_InsertUpdateTaxOfficeTarget(plstTaxOfficeTarget);
        }

        public IList<usp_GetTaxOfficeTargetList_Result> BL_GetTaxOfficeTarget(MAP_TaxOffice_Target pObjTarget)
        {
            return _TaxOfficeRepository.REP_GetTaxOfficeTarget(pObjTarget);
        }
    }
}
