﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EIRS.BOL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ERASEntities : DbContext
    {
        public ERASEntities()
            : base("name=ERASEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MST_AwarenessCategory> MST_AwarenessCategory { get; set; }
        public virtual DbSet<MST_Business> MST_Business { get; set; }
        public virtual DbSet<MST_FAQ> MST_FAQ { get; set; }
        public virtual DbSet<MST_Menu> MST_Menu { get; set; }
        public virtual DbSet<MST_Pages> MST_Pages { get; set; }
        public virtual DbSet<SystemRole> SystemRoles { get; set; }
        public virtual DbSet<SystemUser> SystemUsers { get; set; }
        public virtual DbSet<MST_UserType> MST_UserType { get; set; }
        public virtual DbSet<MAP_CentralMenu_Screen> MAP_CentralMenu_Screen { get; set; }
        public virtual DbSet<MAP_User_Screen> MAP_User_Screen { get; set; }
        public virtual DbSet<MST_CentralMenu> MST_CentralMenu { get; set; }
        public virtual DbSet<MST_Screen> MST_Screen { get; set; }
        public virtual DbSet<MAP_API_Users_Rights> MAP_API_Users_Rights { get; set; }
        public virtual DbSet<MST_UserToken> MST_UserToken { get; set; }
        public virtual DbSet<MST_API> MST_API { get; set; }
        public virtual DbSet<MST_Users> MST_Users { get; set; }
    
        public virtual ObjectResult<usp_GetAwarenessCategoryList_Result> usp_GetAwarenessCategoryList(string awarenessCategoryName, Nullable<int> awarenessCategoryID, string awarenessCategoryIds, Nullable<int> intStatus, string includeAwarenessCategoryIds, string excludeAwarenessCategoryIds)
        {
            var awarenessCategoryNameParameter = awarenessCategoryName != null ?
                new ObjectParameter("AwarenessCategoryName", awarenessCategoryName) :
                new ObjectParameter("AwarenessCategoryName", typeof(string));
    
            var awarenessCategoryIDParameter = awarenessCategoryID.HasValue ?
                new ObjectParameter("AwarenessCategoryID", awarenessCategoryID) :
                new ObjectParameter("AwarenessCategoryID", typeof(int));
    
            var awarenessCategoryIdsParameter = awarenessCategoryIds != null ?
                new ObjectParameter("AwarenessCategoryIds", awarenessCategoryIds) :
                new ObjectParameter("AwarenessCategoryIds", typeof(string));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            var includeAwarenessCategoryIdsParameter = includeAwarenessCategoryIds != null ?
                new ObjectParameter("IncludeAwarenessCategoryIds", includeAwarenessCategoryIds) :
                new ObjectParameter("IncludeAwarenessCategoryIds", typeof(string));
    
            var excludeAwarenessCategoryIdsParameter = excludeAwarenessCategoryIds != null ?
                new ObjectParameter("ExcludeAwarenessCategoryIds", excludeAwarenessCategoryIds) :
                new ObjectParameter("ExcludeAwarenessCategoryIds", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetAwarenessCategoryList_Result>("usp_GetAwarenessCategoryList", awarenessCategoryNameParameter, awarenessCategoryIDParameter, awarenessCategoryIdsParameter, intStatusParameter, includeAwarenessCategoryIdsParameter, excludeAwarenessCategoryIdsParameter);
        }
    
        public virtual ObjectResult<usp_GetFAQList_Result> usp_GetFAQList(string fAQTitle, Nullable<int> fAQID, string fAQIds, string awarenessCategoryIds, Nullable<int> intStatus, string includeFAQIds, string excludeFAQIds)
        {
            var fAQTitleParameter = fAQTitle != null ?
                new ObjectParameter("FAQTitle", fAQTitle) :
                new ObjectParameter("FAQTitle", typeof(string));
    
            var fAQIDParameter = fAQID.HasValue ?
                new ObjectParameter("FAQID", fAQID) :
                new ObjectParameter("FAQID", typeof(int));
    
            var fAQIdsParameter = fAQIds != null ?
                new ObjectParameter("FAQIds", fAQIds) :
                new ObjectParameter("FAQIds", typeof(string));
    
            var awarenessCategoryIdsParameter = awarenessCategoryIds != null ?
                new ObjectParameter("AwarenessCategoryIds", awarenessCategoryIds) :
                new ObjectParameter("AwarenessCategoryIds", typeof(string));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            var includeFAQIdsParameter = includeFAQIds != null ?
                new ObjectParameter("IncludeFAQIds", includeFAQIds) :
                new ObjectParameter("IncludeFAQIds", typeof(string));
    
            var excludeFAQIdsParameter = excludeFAQIds != null ?
                new ObjectParameter("ExcludeFAQIds", excludeFAQIds) :
                new ObjectParameter("ExcludeFAQIds", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetFAQList_Result>("usp_GetFAQList", fAQTitleParameter, fAQIDParameter, fAQIdsParameter, awarenessCategoryIdsParameter, intStatusParameter, includeFAQIdsParameter, excludeFAQIdsParameter);
        }
    
        public virtual ObjectResult<usp_GetMenuList_Result> usp_GetMenuList(Nullable<int> menuID, Nullable<int> parentMenuID, Nullable<int> intStatus, string menuName, string menuUrl)
        {
            var menuIDParameter = menuID.HasValue ?
                new ObjectParameter("MenuID", menuID) :
                new ObjectParameter("MenuID", typeof(int));
    
            var parentMenuIDParameter = parentMenuID.HasValue ?
                new ObjectParameter("ParentMenuID", parentMenuID) :
                new ObjectParameter("ParentMenuID", typeof(int));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            var menuNameParameter = menuName != null ?
                new ObjectParameter("MenuName", menuName) :
                new ObjectParameter("MenuName", typeof(string));
    
            var menuUrlParameter = menuUrl != null ?
                new ObjectParameter("MenuUrl", menuUrl) :
                new ObjectParameter("MenuUrl", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetMenuList_Result>("usp_GetMenuList", menuIDParameter, parentMenuIDParameter, intStatusParameter, menuNameParameter, menuUrlParameter);
        }
    
        public virtual ObjectResult<usp_GetPageList_Result> usp_GetPageList(Nullable<int> pageID)
        {
            var pageIDParameter = pageID.HasValue ?
                new ObjectParameter("PageID", pageID) :
                new ObjectParameter("PageID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetPageList_Result>("usp_GetPageList", pageIDParameter);
        }
    
        public virtual ObjectResult<usp_GetSystemUserList_Result> usp_GetSystemUserList(string systemUserName, Nullable<int> systemUserID, string systemUserIds, Nullable<int> intStatus, string includeSystemUserIds, string excludeSystemUserIds)
        {
            var systemUserNameParameter = systemUserName != null ?
                new ObjectParameter("SystemUserName", systemUserName) :
                new ObjectParameter("SystemUserName", typeof(string));
    
            var systemUserIDParameter = systemUserID.HasValue ?
                new ObjectParameter("SystemUserID", systemUserID) :
                new ObjectParameter("SystemUserID", typeof(int));
    
            var systemUserIdsParameter = systemUserIds != null ?
                new ObjectParameter("SystemUserIds", systemUserIds) :
                new ObjectParameter("SystemUserIds", typeof(string));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            var includeSystemUserIdsParameter = includeSystemUserIds != null ?
                new ObjectParameter("IncludeSystemUserIds", includeSystemUserIds) :
                new ObjectParameter("IncludeSystemUserIds", typeof(string));
    
            var excludeSystemUserIdsParameter = excludeSystemUserIds != null ?
                new ObjectParameter("ExcludeSystemUserIds", excludeSystemUserIds) :
                new ObjectParameter("ExcludeSystemUserIds", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetSystemUserList_Result>("usp_GetSystemUserList", systemUserNameParameter, systemUserIDParameter, systemUserIdsParameter, intStatusParameter, includeSystemUserIdsParameter, excludeSystemUserIdsParameter);
        }
    
        public virtual ObjectResult<usp_GetClaimBusinessList_Result> usp_GetClaimBusinessList(Nullable<long> businessID, string businessName, Nullable<int> intClaimed, Nullable<int> intStatus)
        {
            var businessIDParameter = businessID.HasValue ?
                new ObjectParameter("BusinessID", businessID) :
                new ObjectParameter("BusinessID", typeof(long));
    
            var businessNameParameter = businessName != null ?
                new ObjectParameter("BusinessName", businessName) :
                new ObjectParameter("BusinessName", typeof(string));
    
            var intClaimedParameter = intClaimed.HasValue ?
                new ObjectParameter("intClaimed", intClaimed) :
                new ObjectParameter("intClaimed", typeof(int));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetClaimBusinessList_Result>("usp_GetClaimBusinessList", businessIDParameter, businessNameParameter, intClaimedParameter, intStatusParameter);
        }
    
        public virtual ObjectResult<usp_GetUserList_Result> usp_GetUserList(Nullable<int> userID, Nullable<int> userTypeID, Nullable<int> intStatus, Nullable<int> managerID, Nullable<int> taxOfficeID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var userTypeIDParameter = userTypeID.HasValue ?
                new ObjectParameter("UserTypeID", userTypeID) :
                new ObjectParameter("UserTypeID", typeof(int));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            var managerIDParameter = managerID.HasValue ?
                new ObjectParameter("ManagerID", managerID) :
                new ObjectParameter("ManagerID", typeof(int));
    
            var taxOfficeIDParameter = taxOfficeID.HasValue ?
                new ObjectParameter("TaxOfficeID", taxOfficeID) :
                new ObjectParameter("TaxOfficeID", typeof(int));

            var emailAddressParameter = new ObjectParameter("EmailAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetUserList_Result>("usp_GetUserList", userIDParameter, userTypeIDParameter, intStatusParameter, managerIDParameter, taxOfficeIDParameter, emailAddressParameter);
        }
        public virtual ObjectResult<usp_GetUserList_Result> usp_GetUserList(Nullable<int> userID, Nullable<int> userTypeID, Nullable<int> intStatus, Nullable<int> managerID, Nullable<int> taxOfficeID, string emailAddress)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var userTypeIDParameter = userTypeID.HasValue ?
                new ObjectParameter("UserTypeID", userTypeID) :
                new ObjectParameter("UserTypeID", typeof(int));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            var managerIDParameter = managerID.HasValue ?
                new ObjectParameter("ManagerID", managerID) :
                new ObjectParameter("ManagerID", typeof(int));
    
            var taxOfficeIDParameter = taxOfficeID.HasValue ?
                new ObjectParameter("TaxOfficeID", taxOfficeID) :
                new ObjectParameter("TaxOfficeID", typeof(int));

            var emailAddressParameter = emailAddress != null ?
                new ObjectParameter("EmailAddress", emailAddress) :
                new ObjectParameter("EmailAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetUserList_Result>("usp_GetUserList", userIDParameter, userTypeIDParameter, intStatusParameter, managerIDParameter, taxOfficeIDParameter, emailAddressParameter);
        }
    
        public virtual ObjectResult<usp_GetCentralMenuList_Result> usp_GetCentralMenuList(Nullable<int> centralMenuID, Nullable<int> parentCentralMenuID, Nullable<int> intStatus, Nullable<int> intMenuType, string centralMenuName)
        {
            var centralMenuIDParameter = centralMenuID.HasValue ?
                new ObjectParameter("CentralMenuID", centralMenuID) :
                new ObjectParameter("CentralMenuID", typeof(int));
    
            var parentCentralMenuIDParameter = parentCentralMenuID.HasValue ?
                new ObjectParameter("ParentCentralMenuID", parentCentralMenuID) :
                new ObjectParameter("ParentCentralMenuID", typeof(int));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            var intMenuTypeParameter = intMenuType.HasValue ?
                new ObjectParameter("intMenuType", intMenuType) :
                new ObjectParameter("intMenuType", typeof(int));
    
            var centralMenuNameParameter = centralMenuName != null ?
                new ObjectParameter("CentralMenuName", centralMenuName) :
                new ObjectParameter("CentralMenuName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetCentralMenuList_Result>("usp_GetCentralMenuList", centralMenuIDParameter, parentCentralMenuIDParameter, intStatusParameter, intMenuTypeParameter, centralMenuNameParameter);
        }
    
        public virtual ObjectResult<usp_GetCentralMenuUserBased_Result> usp_GetCentralMenuUserBased(Nullable<int> userID, Nullable<int> parentMenuID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var parentMenuIDParameter = parentMenuID.HasValue ?
                new ObjectParameter("ParentMenuID", parentMenuID) :
                new ObjectParameter("ParentMenuID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetCentralMenuUserBased_Result>("usp_GetCentralMenuUserBased", userIDParameter, parentMenuIDParameter);
        }
    
        public virtual ObjectResult<usp_GetScreenList_Result> usp_GetScreenList(string screenName, Nullable<int> screenID, Nullable<int> intStatus)
        {
            var screenNameParameter = screenName != null ?
                new ObjectParameter("ScreenName", screenName) :
                new ObjectParameter("ScreenName", typeof(string));
    
            var screenIDParameter = screenID.HasValue ?
                new ObjectParameter("ScreenID", screenID) :
                new ObjectParameter("ScreenID", typeof(int));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetScreenList_Result>("usp_GetScreenList", screenNameParameter, screenIDParameter, intStatusParameter);
        }
    
        public virtual ObjectResult<usp_GetScreenMenuList_Result> usp_GetScreenMenuList(Nullable<int> screenID, Nullable<int> centralMenuID)
        {
            var screenIDParameter = screenID.HasValue ?
                new ObjectParameter("ScreenID", screenID) :
                new ObjectParameter("ScreenID", typeof(int));
    
            var centralMenuIDParameter = centralMenuID.HasValue ?
                new ObjectParameter("CentralMenuID", centralMenuID) :
                new ObjectParameter("CentralMenuID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetScreenMenuList_Result>("usp_GetScreenMenuList", screenIDParameter, centralMenuIDParameter);
        }
    
        public virtual ObjectResult<usp_GetScreenUserList_Result> usp_GetScreenUserList(Nullable<int> screenID, Nullable<int> userID)
        {
            var screenIDParameter = screenID.HasValue ?
                new ObjectParameter("ScreenID", screenID) :
                new ObjectParameter("ScreenID", typeof(int));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetScreenUserList_Result>("usp_GetScreenUserList", screenIDParameter, userIDParameter);
        }
    
        public virtual ObjectResult<usp_GetAPIList_Result> usp_GetAPIList(string aPIName, Nullable<int> aPIID, Nullable<int> intStatus)
        {
            var aPINameParameter = aPIName != null ?
                new ObjectParameter("APIName", aPIName) :
                new ObjectParameter("APIName", typeof(string));
    
            var aPIIDParameter = aPIID.HasValue ?
                new ObjectParameter("APIID", aPIID) :
                new ObjectParameter("APIID", typeof(int));
    
            var intStatusParameter = intStatus.HasValue ?
                new ObjectParameter("intStatus", intStatus) :
                new ObjectParameter("intStatus", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetAPIList_Result>("usp_GetAPIList", aPINameParameter, aPIIDParameter, intStatusParameter);
        }
    
        public virtual ObjectResult<usp_GetAPIUserRightList_Result> usp_GetAPIUserRightList(Nullable<int> userID, Nullable<int> aPIID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var aPIIDParameter = aPIID.HasValue ?
                new ObjectParameter("APIID", aPIID) :
                new ObjectParameter("APIID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetAPIUserRightList_Result>("usp_GetAPIUserRightList", userIDParameter, aPIIDParameter);
        }
    
        public virtual int AddUserToken(Nullable<int> createdBy, Nullable<System.DateTime> createddate, Nullable<int> userId, string token, Nullable<System.DateTime> tokenExpiresDate, Nullable<System.DateTime> tokenIssuedDate)
        {
            var createdByParameter = createdBy.HasValue ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(int));
    
            var createddateParameter = createddate.HasValue ?
                new ObjectParameter("Createddate", createddate) :
                new ObjectParameter("Createddate", typeof(System.DateTime));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(int));
    
            var tokenParameter = token != null ?
                new ObjectParameter("token", token) :
                new ObjectParameter("token", typeof(string));
    
            var tokenExpiresDateParameter = tokenExpiresDate.HasValue ?
                new ObjectParameter("tokenExpiresDate", tokenExpiresDate) :
                new ObjectParameter("tokenExpiresDate", typeof(System.DateTime));
    
            var tokenIssuedDateParameter = tokenIssuedDate.HasValue ?
                new ObjectParameter("tokenIssuedDate", tokenIssuedDate) :
                new ObjectParameter("tokenIssuedDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddUserToken", createdByParameter, createddateParameter, userIdParameter, tokenParameter, tokenExpiresDateParameter, tokenIssuedDateParameter);
        }
    }
}
