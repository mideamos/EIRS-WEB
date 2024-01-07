using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLEconomicActivities
    {
        IEconomicActivitiesRepository _EconomicActivitiesRepository;

        public BLEconomicActivities()
        {
            _EconomicActivitiesRepository = new EconomicActivitiesRepository();
        }

        public IList<usp_GetEconomicActivitiesList_Result> BL_GetEconomicActivitiesList(Economic_Activities pObjEconomicActivities)
        {
            return _EconomicActivitiesRepository.REP_GetEconomicActivitiesList(pObjEconomicActivities);
        }

        public FuncResponse BL_InsertUpdateEconomicActivities(Economic_Activities pObjEconomicActivities)
        {
            return _EconomicActivitiesRepository.REP_InsertUpdateEconomicActivities(pObjEconomicActivities);
        }

        public usp_GetEconomicActivitiesList_Result BL_GetEconomicActivitiesDetails(Economic_Activities pObjEconomicActivities)
        {
            return _EconomicActivitiesRepository.REP_GetEconomicActivitiesDetails(pObjEconomicActivities);
        }

        public IList<DropDownListResult> BL_GetEconomicActivitiesDropDownList(Economic_Activities pObjEconomicActivities)
        {
            return _EconomicActivitiesRepository.REP_GetEconomicActivitiesDropDownList(pObjEconomicActivities);
        }

        public FuncResponse BL_UpdateStatus(Economic_Activities pObjEconomicActivities)
        {
            return _EconomicActivitiesRepository.REP_UpdateStatus(pObjEconomicActivities);
        }
    }
}
