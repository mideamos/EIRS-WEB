using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        EIRSEntities _db;

        public FuncResponse<Profile> REP_InsertUpdateProfile(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                Profile mObjInsertUpdateProfile; //Profile Insert Update Object
                FuncResponse<Profile> mObjFuncResponse = new FuncResponse<Profile>(); //Return Object

                ////Check if Duplicate
                //var vDuplicateCheck = (from prf in _db.Profiles
                //                       where prf.ProfileName == pObjProfile.ProfileName && prf.ProfileID != pObjProfile.ProfileID
                //                       select prf);

                //if (vDuplicateCheck.Count() > 0)
                //{
                //    mObjFuncResponse.Success = false;
                //    mObjFuncResponse.Message = "Profile already exists";
                //    return mObjFuncResponse;
                //}

                //If Update Load Profile
                if (pObjProfile.ProfileID != 0)
                {
                    mObjInsertUpdateProfile = (from prf in _db.Profiles
                                               where prf.ProfileID == pObjProfile.ProfileID
                                               select prf).FirstOrDefault();

                    if (mObjInsertUpdateProfile != null)
                    {
                        mObjInsertUpdateProfile.ModifiedBy = pObjProfile.ModifiedBy;
                        mObjInsertUpdateProfile.ModifiedDate = pObjProfile.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateProfile = new Profile();
                        mObjInsertUpdateProfile.CreatedBy = pObjProfile.CreatedBy;
                        mObjInsertUpdateProfile.CreatedDate = pObjProfile.CreatedDate;
                    }
                }
                else // Else Insert Profile
                {
                    mObjInsertUpdateProfile = new Profile();
                    mObjInsertUpdateProfile.CreatedBy = pObjProfile.CreatedBy;
                    mObjInsertUpdateProfile.CreatedDate = pObjProfile.CreatedDate;
                }

                mObjInsertUpdateProfile.ProfileDescription = pObjProfile.ProfileDescription;
                mObjInsertUpdateProfile.AssetTypeID = pObjProfile.AssetTypeID;
                mObjInsertUpdateProfile.AssetTypeStatus = pObjProfile.AssetTypeStatus;
                mObjInsertUpdateProfile.Active = pObjProfile.Active;

                if (pObjProfile.ProfileID == 0)
                {
                    _db.Profiles.Add(mObjInsertUpdateProfile);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjProfile.ProfileID == 0)
                        mObjFuncResponse.Message = "Profile Added Successfully";
                    else
                        mObjFuncResponse.Message = "Profile Updated Successfully";

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateProfile;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjProfile.ProfileID == 0)
                        mObjFuncResponse.Message = "Profile Addition Failed";
                    else
                        mObjFuncResponse.Message = "Profile Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetProfileList_Result> REP_GetProfileList(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetProfileList(pObjProfile.ProfileID, pObjProfile.AssetTypeID, pObjProfile.IntStatus).ToList();
            }
        }

        public usp_GetProfileList_Result REP_GetProfileDetails(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetProfileList(pObjProfile.ProfileID, pObjProfile.AssetTypeID, pObjProfile.IntStatus).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetProfileDropDownList(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                var vQueryable = _db.Profiles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(pObjProfile.ProfileDescription))
                {
                    vQueryable = vQueryable.Where(t => t.ProfileDescription.Contains(pObjProfile.ProfileDescription));
                }


                var vResult = (from prf in vQueryable
                               where prf.Active == true
                               select new DropDownListResult()
                               {
                                   id = prf.ProfileID,
                                   text = prf.ProfileReferenceNo + " - " + prf.ProfileDescription
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                Profile mObjInsertUpdateProfile; //Profile Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Profile
                if (pObjProfile.ProfileID != 0)
                {
                    mObjInsertUpdateProfile = (from prf in _db.Profiles
                                               where prf.ProfileID == pObjProfile.ProfileID
                                               select prf).FirstOrDefault();

                    if (mObjInsertUpdateProfile != null)
                    {
                        mObjInsertUpdateProfile.Active = !mObjInsertUpdateProfile.Active;
                        mObjInsertUpdateProfile.ModifiedBy = pObjProfile.ModifiedBy;
                        mObjInsertUpdateProfile.ModifiedDate = pObjProfile.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Profile Updated Successfully";
                            // mObjFuncResponse.AdditionalData = _db.usp_GetProfileList(0, pObjProfile.AssetTypeID, pObjProfile.IntStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Profile Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertProfileSector(ProfileSector pObjProfileSector)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prfsec in _db.ProfileSectors
                               where prfsec.ProfileID == pObjProfileSector.ProfileID && prfsec.SectorID == pObjProfileSector.SectorID
                               select prfsec);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sector Already Exists";
                }

                _db.ProfileSectors.Add(pObjProfileSector);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileSector(ProfileSector pObjProfileSector)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileSector mObjDeleteProfileSector;

                mObjDeleteProfileSector = _db.ProfileSectors.Find(pObjProfileSector.ProfileSectorID);

                if (mObjDeleteProfileSector == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sector Already Removed.";
                }
                else
                {
                    _db.ProfileSectors.Remove(mObjDeleteProfileSector);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileSector> REP_GetProfileSector(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileSectors.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }

        public FuncResponse REP_InsertProfileSubSector(ProfileSubSector pObjProfileSubSector)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prfsec in _db.ProfileSubSectors
                               where prfsec.ProfileID == pObjProfileSubSector.ProfileID && prfsec.SubSectorID == pObjProfileSubSector.SubSectorID
                               select prfsec);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sub Sector Already Exists";
                }

                _db.ProfileSubSectors.Add(pObjProfileSubSector);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileSubSector(ProfileSubSector pObjProfileSubSector)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileSubSector mObjDeleteProfileSubSector;

                mObjDeleteProfileSubSector = _db.ProfileSubSectors.Find(pObjProfileSubSector.ProfileSubSectorID);

                if (mObjDeleteProfileSubSector == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sub Sector Already Removed.";
                }
                else
                {
                    _db.ProfileSubSectors.Remove(mObjDeleteProfileSubSector);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileSubSector> REP_GetProfileSubSector(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileSubSectors.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }

        public FuncResponse REP_InsertProfileGroup(ProfileGroup pObjProfileGroup)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prfsec in _db.ProfileGroups
                               where prfsec.ProfileID == pObjProfileGroup.ProfileID && prfsec.GroupID == pObjProfileGroup.GroupID
                               select prfsec);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Group Already Exists";
                }

                _db.ProfileGroups.Add(pObjProfileGroup);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileGroup(ProfileGroup pObjProfileGroup)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileGroup mObjDeleteProfileGroup;

                mObjDeleteProfileGroup = _db.ProfileGroups.Find(pObjProfileGroup.ProfileGroupID);

                if (mObjDeleteProfileGroup == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Group Already Removed.";
                }
                else
                {
                    _db.ProfileGroups.Remove(mObjDeleteProfileGroup);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileGroup> REP_GetProfileGroup(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileGroups.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }

        public FuncResponse REP_InsertProfileSubGroup(ProfileSubGroup pObjProfileSubGroup)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prfsec in _db.ProfileSubGroups
                               where prfsec.ProfileID == pObjProfileSubGroup.ProfileID && prfsec.SubGroupID == pObjProfileSubGroup.SubGroupID
                               select prfsec);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sub Group Already Exists";
                }

                _db.ProfileSubGroups.Add(pObjProfileSubGroup);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileSubGroup(ProfileSubGroup pObjProfileSubGroup)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileSubGroup mObjDeleteProfileSubGroup;

                mObjDeleteProfileSubGroup = _db.ProfileSubGroups.Find(pObjProfileSubGroup.ProfileSubGroupID);

                if (mObjDeleteProfileSubGroup == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sub Group Already Removed.";
                }
                else
                {
                    _db.ProfileSubGroups.Remove(mObjDeleteProfileSubGroup);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileSubGroup> REP_GetProfileSubGroup(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileSubGroups.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }

        public FuncResponse REP_InsertProfileSectorElement(ProfileSectorElement pObjProfileSectorElement)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prfsec in _db.ProfileSectorElements
                               where prfsec.ProfileID == pObjProfileSectorElement.ProfileID && prfsec.SectorElementID == pObjProfileSectorElement.SectorElementID
                               select prfsec);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sector Element Already Exists";
                }

                _db.ProfileSectorElements.Add(pObjProfileSectorElement);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileSectorElement(ProfileSectorElement pObjProfileSectorElement)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileSectorElement mObjDeleteProfileSectorElement;

                mObjDeleteProfileSectorElement = _db.ProfileSectorElements.Find(pObjProfileSectorElement.ProfileSectorElementID);

                if (mObjDeleteProfileSectorElement == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sector Element Already Removed.";
                }
                else
                {
                    _db.ProfileSectorElements.Remove(mObjDeleteProfileSectorElement);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileSectorElement> REP_GetProfileSectorElement(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileSectorElements.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }

        public FuncResponse REP_InsertProfileSectorSubElement(ProfileSectorSubElement pObjProfileSectorSubElement)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prfsec in _db.ProfileSectorSubElements
                               where prfsec.ProfileID == pObjProfileSectorSubElement.ProfileID && prfsec.SectorSubElementID == pObjProfileSectorSubElement.SectorSubElementID
                               select prfsec);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sector Sub Element Already Exists";
                }

                _db.ProfileSectorSubElements.Add(pObjProfileSectorSubElement);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileSectorSubElement(ProfileSectorSubElement pObjProfileSectorSubElement)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileSectorSubElement mObjDeleteProfileSectorSubElement;

                mObjDeleteProfileSectorSubElement = _db.ProfileSectorSubElements.Find(pObjProfileSectorSubElement.ProfileSectorSubElementID);

                if (mObjDeleteProfileSectorSubElement == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sector Sub Element Already Removed.";
                }
                else
                {
                    _db.ProfileSectorSubElements.Remove(mObjDeleteProfileSectorSubElement);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileSectorSubElement> REP_GetProfileSectorSubElement(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileSectorSubElements.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }

        public FuncResponse REP_InsertProfileAttribute(ProfileAttribute pObjProfileAttribute)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prfsec in _db.ProfileAttributes
                               where prfsec.ProfileID == pObjProfileAttribute.ProfileID && prfsec.AttributeID == pObjProfileAttribute.AttributeID
                               select prfsec);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Attribute Already Exists";
                }

                _db.ProfileAttributes.Add(pObjProfileAttribute);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileAttribute(ProfileAttribute pObjProfileAttribute)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileAttribute mObjDeleteProfileAttribute;

                mObjDeleteProfileAttribute = _db.ProfileAttributes.Find(pObjProfileAttribute.ProfileAttributeID);

                if (mObjDeleteProfileAttribute == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Attribute Already Removed.";
                }
                else
                {
                    _db.ProfileAttributes.Remove(mObjDeleteProfileAttribute);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileAttribute> REP_GetProfileAttribute(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileAttributes.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }

        public FuncResponse REP_InsertProfileSubAttribute(ProfileSubAttribute pObjProfileSubAttribute)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prfsec in _db.ProfileSubAttributes
                               where prfsec.ProfileID == pObjProfileSubAttribute.ProfileID && prfsec.SubAttributeID == pObjProfileSubAttribute.SubAttributeID
                               select prfsec);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sub Attribute Already Exists";
                }

                _db.ProfileSubAttributes.Add(pObjProfileSubAttribute);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileSubAttribute(ProfileSubAttribute pObjProfileSubAttribute)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileSubAttribute mObjDeleteProfileSubAttribute;

                mObjDeleteProfileSubAttribute = _db.ProfileSubAttributes.Find(pObjProfileSubAttribute.ProfileSubAttributeID);

                if (mObjDeleteProfileSubAttribute == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Profile Sub Attribute Already Removed.";
                }
                else
                {
                    _db.ProfileSubAttributes.Remove(mObjDeleteProfileSubAttribute);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileSubAttribute> REP_GetProfileSubAttribute(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileSubAttributes.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }


        public FuncResponse REP_InsertProfileTaxPayerType(ProfileTaxPayerType pObjProfileTaxPayerType)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prftptype in _db.ProfileTaxPayerTypes
                               where prftptype.ProfileID == pObjProfileTaxPayerType.ProfileID && prftptype.TaxPayerTypeID == pObjProfileTaxPayerType.TaxPayerTypeID
                               select prftptype);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Tax Payer Type Already Exists";
                }

                _db.ProfileTaxPayerTypes.Add(pObjProfileTaxPayerType);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileTaxPayerType(ProfileTaxPayerType pObjProfileTaxPayerType)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileTaxPayerType mObjDeleteProfileTaxPayerType;

                mObjDeleteProfileTaxPayerType = _db.ProfileTaxPayerTypes.Find(pObjProfileTaxPayerType.ProfileTaxPayerTypeID);

                if (mObjDeleteProfileTaxPayerType == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Tax Payer Type Already Removed.";
                }
                else
                {
                    _db.ProfileTaxPayerTypes.Remove(mObjDeleteProfileTaxPayerType);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileTaxPayerType> REP_GetProfileTaxPayerType(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileTaxPayerTypes.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }


        public FuncResponse REP_InsertProfileTaxPayerRole(ProfileTaxPayerRole pObjProfileTaxPayerRole)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from prftprol in _db.ProfileTaxPayerRoles
                               where prftprol.ProfileID == pObjProfileTaxPayerRole.ProfileID && prftprol.TaxPayerRoleID == pObjProfileTaxPayerRole.TaxPayerRoleID
                               select prftprol);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Tax Payer Role Already Exists";
                }

                _db.ProfileTaxPayerRoles.Add(pObjProfileTaxPayerRole);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveProfileTaxPayerRole(ProfileTaxPayerRole pObjProfileTaxPayerRole)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ProfileTaxPayerRole mObjDeleteProfileTaxPayerRole;

                mObjDeleteProfileTaxPayerRole = _db.ProfileTaxPayerRoles.Find(pObjProfileTaxPayerRole.ProfileTaxPayerRoleID);

                if (mObjDeleteProfileTaxPayerRole == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Tax Payer Role Already Removed.";
                }
                else
                {
                    _db.ProfileTaxPayerRoles.Remove(mObjDeleteProfileTaxPayerRole);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<ProfileTaxPayerRole> REP_GetProfileTaxPayerRole(int pIntProfileID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.ProfileTaxPayerRoles.Where(t => t.ProfileID == pIntProfileID).ToList();
            }
        }

        public IList<usp_GetProfileData_Result> REP_GetProfileData(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetProfileData(pObjProfile.ProfileDescription, pObjProfile.IntSearchType).ToList();
            }
        }

        public IList<usp_GetProfileTaxPayerData_Result> REP_GetProfileTaxPayerData(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetProfileTaxPayerData(pObjProfile.ProfileID).ToList();
            }
        }

        public IList<usp_GetProfileAssetData_Result> REP_GetProfileAssetData(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetProfileAssetData(pObjProfile.ProfileID).ToList();
            }
        }

        public IList<usp_GetCompanyPAYEAsset_Result> REP_GetCompanyPAYEAssetList(int pIntCompanyID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCompanyPAYEAsset(pIntCompanyID).ToList();
            }
        }

        public IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> REP_GetTaxPayerBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerBasedOnProfileForSupplier(pObjProfile.ProfileDescription, pObjProfile.VehiclePurposeID, pObjProfile.BusinessSector, pObjProfile.BusinessCategory, pObjProfile.TaxPayerName, pObjProfile.IntSearchType).ToList();
            }
        }

        public IList<usp_GetBuildingBasedOnProfileForSupplier_Result> REP_GetBuildingBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingBasedOnProfileForSupplier(pObjProfile.ProfileDescription).ToList();
            }
        }

        public IList<usp_GetBusinessBasedOnProfileForSupplier_Result> REP_GetBusinessBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {

                var ret = _db.usp_GetBusinessBasedOnProfileForSupplier(pObjProfile.ProfileDescription, pObjProfile.BusinessSector, pObjProfile.BusinessCategory, pObjProfile.TaxPayerName, pObjProfile.IntSearchType).ToList();
                var number = ret.Count();
                return ret;
            }
        }

        public IList<usp_GetLandBasedOnProfileForSupplier_Result> REP_GetLandBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandBasedOnProfileForSupplier(pObjProfile.ProfileDescription).ToList();
            }
        }

        public IList<usp_GetVehicleBasedOnProfileForSupplier_Result> REP_GetVehicleBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleBasedOnProfileForSupplier(pObjProfile.ProfileDescription, pObjProfile.VehiclePurposeID, pObjProfile.IntSearchType).ToList();
            }
        }

        public IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> REP_GetTaxPayerAssetProfileBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier(pObjProfile.ProfileDescription, pObjProfile.VehiclePurposeID, pObjProfile.BusinessSector, pObjProfile.BusinessCategory, pObjProfile.TaxPayerName, pObjProfile.IntSearchType).ToList();
            }
        }

        public IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> REP_GetAssessmentRuleBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleBasedOnProfileForSupplier(pObjProfile.ProfileDescription, pObjProfile.VehiclePurposeID, pObjProfile.BusinessSector, pObjProfile.BusinessCategory, pObjProfile.TaxPayerName, pObjProfile.IntSearchType).ToList();
            }
        }
        public IList<usp_GetAssessmentRuleBasedOnProfileForSupplierNew_Result> REP_GetAssessmentRuleBasedOnProfileForSupplierNew(Profile pObjProfile, int year)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleBasedOnProfileForSupplierNew(pObjProfile.ProfileDescription, pObjProfile.VehiclePurposeID, year, pObjProfile.BusinessSector, pObjProfile.BusinessCategory, pObjProfile.TaxPayerName, pObjProfile.IntSearchType).ToList();
            }
        }

      
        public IList<Assessment_Rules> GetAssessment_Rules_Api()
        {
            using (_db = new EIRSEntities())
            {
                return _db.Assessment_Rules.Where(o => o.ProfileID == 1277).ToList();
            }
        }

        public IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> REP_GetAssessmentItemBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentItemBasedOnProfileForSupplier(pObjProfile.ProfileDescription, pObjProfile.VehiclePurposeID, pObjProfile.BusinessSector, pObjProfile.BusinessCategory, pObjProfile.TaxPayerName, pObjProfile.IntSearchType).ToList();
            }
        }

        public IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> REP_GetAssessmentBasedOnProfileForSupplier(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentBasedOnProfileForSupplier(pObjProfile.ProfileDescription, pObjProfile.VehiclePurposeID, pObjProfile.BusinessSector, pObjProfile.BusinessCategory, pObjProfile.TaxPayerName, pObjProfile.IntSearchType).ToList();
            }
        }



        public IList<usp_SearchProfileForRDMLoad_Result> REP_SearchProfileDetails(Profile pObjProfile)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchProfileForRDMLoad(pObjProfile.ProfileReferenceNo, pObjProfile.AssetTypeName, pObjProfile.ProfileSectorName, pObjProfile.ProfileSubSectorName,
                                                         pObjProfile.ProfileGroupName, pObjProfile.ProfileSubGroupName, pObjProfile.ProfileSectorElementName, pObjProfile.ProfileSectorSubElementName,
                                                         pObjProfile.ProfileAttributeName, pObjProfile.ProfileSubAttributeName, pObjProfile.TaxPayerRoleName, pObjProfile.ProfileDescription, pObjProfile.TaxPayerTypeName, pObjProfile.ActiveText).ToList();
            }
        }

       
    }
}
