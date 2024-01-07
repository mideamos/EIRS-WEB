using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLScratchCardPrinter
    {
        IScratchCardPrinterRepository _NotificationRepository;
        public BLScratchCardPrinter()
        {
            _NotificationRepository = new ScratchCardPrinterRepository();
        }

        public FuncResponse BL_InsertUpdateScratchCardPrinter(Scratch_Card_Printer pObjNotifictionMethod)
        {
            return _NotificationRepository.REP_InsertUpdateScratchCardPrinter(pObjNotifictionMethod);
        }

        public IList<usp_GetScratchCardPrinterList_Result> BL_GetScratchCardPrinterList(Scratch_Card_Printer pObjScratchCardPrinter)
        {
            return _NotificationRepository.REP_GetScratchCardPrinterList(pObjScratchCardPrinter);
        }

        public usp_GetScratchCardPrinterList_Result BL_GetScratchCardPrinterDetails(Scratch_Card_Printer pObjScratchCardPrinter)
        {
            return _NotificationRepository.REP_GetScratchCardPrinterDetails(pObjScratchCardPrinter);
        }

        public FuncResponse BL_UpdateStatus(Scratch_Card_Printer pObjScratchCardPrinter)
        {
            return _NotificationRepository.REP_UpdateStatus(pObjScratchCardPrinter);
        }
    }
}
