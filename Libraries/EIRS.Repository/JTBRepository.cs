using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class JTBRepository : IJTBRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateIndividual(JTB_Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                bool isNewRecord = false;
                JTB_Individual mObjInsertUpdateIndividual; //Individual Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                mObjInsertUpdateIndividual = (from ind in _db.JTB_Individual
                                              where ind.tin == pObjIndividual.tin && ind.bvn == pObjIndividual.bvn
                                              select ind).FirstOrDefault();

                if (mObjInsertUpdateIndividual != null)
                {
                    mObjInsertUpdateIndividual.ModifiedBy = pObjIndividual.ModifiedBy;
                    mObjInsertUpdateIndividual.ModifiedDate = pObjIndividual.ModifiedDate;
                }
                else
                {
                    isNewRecord = true;
                    mObjInsertUpdateIndividual = new JTB_Individual();
                    mObjInsertUpdateIndividual.CreatedBy = pObjIndividual.CreatedBy;
                    mObjInsertUpdateIndividual.CreatedDate = pObjIndividual.CreatedDate;
                }

                mObjInsertUpdateIndividual.tin = pObjIndividual.tin;
                mObjInsertUpdateIndividual.bvn = pObjIndividual.bvn;
                mObjInsertUpdateIndividual.nin= pObjIndividual.nin;
                mObjInsertUpdateIndividual.Title = pObjIndividual.Title;
                mObjInsertUpdateIndividual.SBIRt_name = pObjIndividual.SBIRt_name;
                mObjInsertUpdateIndividual.middle_name = pObjIndividual.middle_name;
                mObjInsertUpdateIndividual.last_name = pObjIndividual.last_name;
                mObjInsertUpdateIndividual.GenderName = pObjIndividual.GenderName;
                mObjInsertUpdateIndividual.StateOfOrigin = pObjIndividual.StateOfOrigin;
                mObjInsertUpdateIndividual.date_of_birth = pObjIndividual.date_of_birth;
                mObjInsertUpdateIndividual.MaritalStatus = pObjIndividual.MaritalStatus;
                mObjInsertUpdateIndividual.Occupation = pObjIndividual.Occupation;
                mObjInsertUpdateIndividual.nationality_name = pObjIndividual.nationality_name;
                mObjInsertUpdateIndividual.phone_no_1 = pObjIndividual.phone_no_1;
                mObjInsertUpdateIndividual.phone_no_2 = pObjIndividual.phone_no_2;
                mObjInsertUpdateIndividual.taxpayer_photo = pObjIndividual.taxpayer_photo;
                mObjInsertUpdateIndividual.email_address = pObjIndividual.email_address;
                mObjInsertUpdateIndividual.date_of_registration = pObjIndividual.date_of_registration;
                mObjInsertUpdateIndividual.house_number = pObjIndividual.house_number;
                mObjInsertUpdateIndividual.street_name = pObjIndividual.street_name;
                mObjInsertUpdateIndividual.city = pObjIndividual.city;
                mObjInsertUpdateIndividual.LGAName = pObjIndividual.LGAName;
                mObjInsertUpdateIndividual.StateName = pObjIndividual.StateName;
                mObjInsertUpdateIndividual.CountryName = pObjIndividual.CountryName;
                mObjInsertUpdateIndividual.TaxAuthorityCode = pObjIndividual.TaxAuthorityCode;
                mObjInsertUpdateIndividual.TaxAuthorityName = pObjIndividual.TaxAuthorityName;
                mObjInsertUpdateIndividual.TaxpayerStatus = pObjIndividual.TaxpayerStatus;

                if (isNewRecord)
                {
                    _db.JTB_Individual.Add(mObjInsertUpdateIndividual);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertUpdateNonIndividual(JTB_NonIndividual pObjNonIndividual)
        {
            using (_db = new EIRSEntities())
            {
                bool isNewRecord = false;
                JTB_NonIndividual mObjInsertUpdateNonIndividual; //Individual Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                mObjInsertUpdateNonIndividual = (from ind in _db.JTB_NonIndividual
                                              where ind.tin == pObjNonIndividual.tin
                                              select ind).FirstOrDefault();

                if (mObjInsertUpdateNonIndividual != null)
                {
                    mObjInsertUpdateNonIndividual.ModifiedBy = pObjNonIndividual.ModifiedBy;
                    mObjInsertUpdateNonIndividual.ModifiedDate = pObjNonIndividual.ModifiedDate;
                }
                else
                {
                    isNewRecord = true;
                    mObjInsertUpdateNonIndividual = new JTB_NonIndividual();
                    mObjInsertUpdateNonIndividual.CreatedBy = pObjNonIndividual.CreatedBy;
                    mObjInsertUpdateNonIndividual.CreatedDate = pObjNonIndividual.CreatedDate;
                }

                mObjInsertUpdateNonIndividual.tin = pObjNonIndividual.tin;
                mObjInsertUpdateNonIndividual.registered_name = pObjNonIndividual.registered_name;
                mObjInsertUpdateNonIndividual.main_trade_name = pObjNonIndividual.main_trade_name;
                mObjInsertUpdateNonIndividual.org_name = pObjNonIndividual.org_name;
                mObjInsertUpdateNonIndividual.registration_number = pObjNonIndividual.registration_number;
                mObjInsertUpdateNonIndividual.phone_no_1 = pObjNonIndividual.phone_no_1;
                mObjInsertUpdateNonIndividual.phone_no_2 = pObjNonIndividual.phone_no_2;
                mObjInsertUpdateNonIndividual.email_address = pObjNonIndividual.email_address;

                mObjInsertUpdateNonIndividual.line_of_business_code = pObjNonIndividual.line_of_business_code;
                mObjInsertUpdateNonIndividual.lob_name = pObjNonIndividual.lob_name;
                mObjInsertUpdateNonIndividual.date_of_registration = pObjNonIndividual.date_of_registration;
                mObjInsertUpdateNonIndividual.commencement_date = pObjNonIndividual.commencement_date;
                mObjInsertUpdateNonIndividual.date_of_incorporation = pObjNonIndividual.date_of_incorporation;
                mObjInsertUpdateNonIndividual.house_number = pObjNonIndividual.house_number;
                mObjInsertUpdateNonIndividual.street_name = pObjNonIndividual.street_name;
                mObjInsertUpdateNonIndividual.city = pObjNonIndividual.city;
                mObjInsertUpdateNonIndividual.LGAName = pObjNonIndividual.LGAName;
                mObjInsertUpdateNonIndividual.StateName = pObjNonIndividual.StateName;
                mObjInsertUpdateNonIndividual.CountryName = pObjNonIndividual.CountryName;
                mObjInsertUpdateNonIndividual.FinYrBegin = pObjNonIndividual.FinYrBegin;
                mObjInsertUpdateNonIndividual.FinYrEnd = pObjNonIndividual.FinYrEnd;
                mObjInsertUpdateNonIndividual.director_name = pObjNonIndividual.director_name;
                mObjInsertUpdateNonIndividual.director_phone = pObjNonIndividual.director_phone;
                mObjInsertUpdateNonIndividual.director_email = pObjNonIndividual.director_email;
                mObjInsertUpdateNonIndividual.TaxAuthorityCode = pObjNonIndividual.TaxAuthorityCode;
                mObjInsertUpdateNonIndividual.TaxAuthorityName = pObjNonIndividual.TaxAuthorityName;
                mObjInsertUpdateNonIndividual.TaxpayerStatus = pObjNonIndividual.TaxpayerStatus;

                if (isNewRecord)
                {
                    _db.JTB_NonIndividual.Add(mObjInsertUpdateNonIndividual);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                }

                return mObjFuncResponse;
            }
        }

        public IList<JTB_Individual> REP_GetIndividualList()
        {
            using(_db = new EIRSEntities())
            {
                return _db.JTB_Individual.ToList();
            }
        }

        public JTB_Individual REP_GetIndividualDetails(long plngJTBIndividualID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.JTB_Individual.Find(plngJTBIndividualID);
            }
        }

        public IList<JTB_NonIndividual> REP_GetNonIndividualList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.JTB_NonIndividual.ToList();
            }
        }

        public JTB_NonIndividual REP_GetNonIndividualDetails(long plngJTBNonIndividualID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.JTB_NonIndividual.Find(plngJTBNonIndividualID);
            }
        }
    }
}
