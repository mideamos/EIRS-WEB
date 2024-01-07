using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IJTBRepository
    {
        FuncResponse REP_InsertUpdateIndividual(JTB_Individual pObjIndividual);
        FuncResponse REP_InsertUpdateNonIndividual(JTB_NonIndividual pObjNonIndividual);
        IList<JTB_Individual> REP_GetIndividualList();
        JTB_Individual REP_GetIndividualDetails(long plngJTBIndividualID);
        IList<JTB_NonIndividual> REP_GetNonIndividualList();
        JTB_NonIndividual REP_GetNonIndividualDetails(long plngJTBNonIndividualID);
    }
}