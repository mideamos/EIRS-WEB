using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
    public class BLZone
    {
        IZoneRepository _ZoneRepository;

        public BLZone()
        {
            _ZoneRepository = new ZoneRepository();
        }

        public IList<Zone> BL_GetZoneList(Zone pObjZone)
        {
            return _ZoneRepository.REP_GetZoneList(pObjZone);
        } 
        public IList<spZoneDetailNew_Result> BL_GetZoneList(int pObjZone)
        {
            return _ZoneRepository.REP_GetZoneList(pObjZone);
        }

        public FuncResponse BL_InsertUpdateZone(Zone pObjZone)
        {
            return _ZoneRepository.REP_InsertUpdateZone(pObjZone);
        }
    }
}
