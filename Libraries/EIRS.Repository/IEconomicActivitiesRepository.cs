using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IEconomicActivitiesRepository
    {
		FuncResponse REP_InsertUpdateEconomicActivities(Economic_Activities pObjEconomicActivities);
		IList<usp_GetEconomicActivitiesList_Result> REP_GetEconomicActivitiesList(Economic_Activities pObjEconomicActivities);
        usp_GetEconomicActivitiesList_Result REP_GetEconomicActivitiesDetails(Economic_Activities pObjEconomicActivities);
        IList<DropDownListResult> REP_GetEconomicActivitiesDropDownList(Economic_Activities pObjEconomicActivities);
        FuncResponse REP_UpdateStatus(Economic_Activities pObjEconomicActivities);
    }
}