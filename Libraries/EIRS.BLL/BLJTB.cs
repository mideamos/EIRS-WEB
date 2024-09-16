using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLJTB
    {
        IJTBRepository _JTBRepository;

        public BLJTB()
        {
            _JTBRepository = new JTBRepository();
        }

        public FuncResponse BL_InsertUpdateIndividual(JTB_Individual pObjIndividual)
        {
            return _JTBRepository.REP_InsertUpdateIndividual(pObjIndividual);
        }

        public FuncResponse BL_InsertUpdateNonIndividual(JTB_NonIndividual pObjNonIndividual)
        {
            return _JTBRepository.REP_InsertUpdateNonIndividual(pObjNonIndividual);
        }

        public IList<JTB_Individual> BL_GetIndividualList()
        {
            return _JTBRepository.REP_GetIndividualList();
        }

        public JTB_Individual BL_GetIndividualDetails(long plngJTBIndividualID)
        {
            return _JTBRepository.REP_GetIndividualDetails(plngJTBIndividualID);
        }

        public IList<JTB_NonIndividual> BL_GetNonIndividualList()
        {
            return _JTBRepository.REP_GetNonIndividualList();
        }

        public JTB_NonIndividual BL_GetNonIndividualDetails(long plngJTBNonIndividualID)
        {
            return _JTBRepository.REP_GetNonIndividualDetails(plngJTBNonIndividualID);
        }


    }
}
