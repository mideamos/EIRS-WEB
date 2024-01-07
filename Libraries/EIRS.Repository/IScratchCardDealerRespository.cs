using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IScratchCardDealerRespository
    {
        usp_GetScratchCardDealerList_Result REP_GetScratchCardDealerDetails(Scratch_Card_Dealers pObjScratchCardDealer);
        IList<usp_GetScratchCardDealerList_Result> REP_GetScratchCardDealerList(Scratch_Card_Dealers pObjScratchCardDealer);
        FuncResponse REP_InsertUpdateScratchCardDealer(Scratch_Card_Dealers pObjScratchCardDealer);
        FuncResponse REP_UpdateStatus(Scratch_Card_Dealers pObjScratchCardDealer);
    }
}