using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public interface IZoneRepository
    {
        FuncResponse REP_InsertUpdateZone(Zone pObjZone);
        IList<Zone> REP_GetZoneList(Zone pObjZone);
        IList<spZoneDetailNew_Result> REP_GetZoneList(int zoneid);
    }
    public class ZoneRepository : IZoneRepository
    {
        EIRSEntities _db;
        public IList<Zone> REP_GetZoneList(Zone pObjZone)
        {
            using (_db = new EIRSEntities())
            {
               if(pObjZone.ZoneId == 0)
                    return _db.Zones.ToList();
               else
                    return _db.Zones.Where(o=>o.ZoneId == pObjZone.ZoneId).ToList();
            }
        }

        public IList<spZoneDetailNew_Result> REP_GetZoneList(int zoneid)
        {
            using (_db = new EIRSEntities())
            {
               return _db.spZoneDetailNew(zoneid).ToList();
            }
        }

        public FuncResponse REP_InsertUpdateZone(Zone pObjZone)
        {
            using (_db = new EIRSEntities())
            {
                Zone mObjInsertUpdateZone; //Zone Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from agny in _db.Zones
                                       where agny.ZoneName == pObjZone.ZoneName && agny.ZoneCode == pObjZone.ZoneCode && agny.ZoneId != pObjZone.ZoneId
                                       select agny);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Zone already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Zone
                if (pObjZone.ZoneId != 0)
                {
                    mObjInsertUpdateZone = (from agny in _db.Zones
                                            where agny.ZoneId == pObjZone.ZoneId
                                            select agny).FirstOrDefault();

                    if (mObjInsertUpdateZone != null)
                    {
                        mObjInsertUpdateZone.ModifiedBY = pObjZone.ModifiedBY;
                        mObjInsertUpdateZone.ModifiedDate = pObjZone.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateZone = new Zone();
                        mObjInsertUpdateZone.CreatedBy = pObjZone.CreatedBy;
                        mObjInsertUpdateZone.CreatedDate = pObjZone.CreatedDate;
                    }
                }
                else // Else Insert Zone
                {
                    mObjInsertUpdateZone = new Zone();
                    mObjInsertUpdateZone.CreatedBy = pObjZone.CreatedBy;
                    mObjInsertUpdateZone.CreatedDate = pObjZone.CreatedDate;
                }

                mObjInsertUpdateZone.ZoneName = pObjZone.ZoneName;
                mObjInsertUpdateZone.ZoneCode = pObjZone.ZoneCode;
                mObjInsertUpdateZone.LgaId = pObjZone.LgaId;
                mObjInsertUpdateZone.Active = pObjZone.Active;

                if (pObjZone.ZoneId == 0)
                {
                    _db.Zones.Add(mObjInsertUpdateZone);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjZone.ZoneId == 0)
                        mObjFuncResponse.Message = "Zone Added Successfully";
                    else
                        mObjFuncResponse.Message = "Zone Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjZone.ZoneId == 0)
                        mObjFuncResponse.Message = "Zone Addition Failed";
                    else
                        mObjFuncResponse.Message = "Zone Updation Failed";
                }

                return mObjFuncResponse;
            }
        }
    }
}
