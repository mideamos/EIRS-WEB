using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLScratchCardDealer
    {
        IScratchCardDealerRespository _ScratchCardDealerRepository;
        public BLScratchCardDealer()
        {
            _ScratchCardDealerRepository = new ScratchCardDealerRespository();
        }

        public FuncResponse BL_InsertUpdateScratchCardDealer(Scratch_Card_Dealers pObjScratchCardDealer)
        {
            return _ScratchCardDealerRepository.REP_InsertUpdateScratchCardDealer(pObjScratchCardDealer);
        }

        public IList<usp_GetScratchCardDealerList_Result> BL_GetScratchCardDealerList(Scratch_Card_Dealers pObjScratchCardDealer)
        {
            return _ScratchCardDealerRepository.REP_GetScratchCardDealerList(pObjScratchCardDealer);
        }

        public usp_GetScratchCardDealerList_Result BL_GetScratchCardDealerDetails(Scratch_Card_Dealers pObjScratchCardDealer)
        {
            return _ScratchCardDealerRepository.REP_GetScratchCardDealerDetails(pObjScratchCardDealer);
        }

        public FuncResponse BL_UpdateStatus(Scratch_Card_Dealers pObjScratchCardDealer)
        {
            return _ScratchCardDealerRepository.REP_UpdateStatus(pObjScratchCardDealer);
        }
    }
}
