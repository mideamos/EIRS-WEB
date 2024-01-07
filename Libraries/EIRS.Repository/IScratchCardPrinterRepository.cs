using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IScratchCardPrinterRepository
    {
        usp_GetScratchCardPrinterList_Result REP_GetScratchCardPrinterDetails(Scratch_Card_Printer pObjScratchCardPrinter);
        IList<usp_GetScratchCardPrinterList_Result> REP_GetScratchCardPrinterList(Scratch_Card_Printer pObjScratchCardPrinter);
        FuncResponse REP_InsertUpdateScratchCardPrinter(Scratch_Card_Printer pObjScratchCardPrinter);
        FuncResponse REP_UpdateStatus(Scratch_Card_Printer pObjScratchCardPrinter);
    }
}