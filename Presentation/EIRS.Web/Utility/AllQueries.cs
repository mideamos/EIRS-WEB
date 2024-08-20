using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Utility
{
    public class AllQueries
    {
        //public static string getSPList => "SELECT ast.AssessmentID,ss.SettlementStatusID,office.UserID, ast.AssessmentRefNo,ast.assessmentamount as Amount,ss.SettlementStatusName as Status,office.ContactName as TaxOfficerName,ast.TaxPayerID as ID,dbo.GetTaxPayerName(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxPayerName,dbo.GetTaxPayerRIN(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxPayerRIN,dbo.GetTaxPayerTaxOfficeName(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxOfficeName FROM Assessment ast left JOIN Settlement_Status ss  ON ast.SettlementStatusID = ss.SettlementStatusID left JOIN TaxPayer_Types tptype ON ast.TaxPayerTypeID = tptype.TaxPayerTypeID   left JOIN ERAS.DBO.MST_Users  office ON ast.CreatedBy = office.UserID where ast.SettlementStatusID = 6  or ast.SettlementStatusID =8 or ast.SettlementStatusID =7";

        public static string getSPList => "SELECT ast.AssessmentID, ss.SettlementStatusID,\toffice.UserID, TOff.OfficeManagerID, ast.AssessmentRefNo, ast.assessmentamount as Amount, ss.SettlementStatusName as Status, office.ContactName as TaxOfficerName, ast.TaxPayerID as ID, dbo.GetTaxPayerName(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxPayerName, dbo.GetTaxPayerRIN(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxPayerRIN, dbo.GetTaxPayerTaxOfficeName(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxOfficeName FROM Assessment ast left JOIN Settlement_Status ss  ON ast.SettlementStatusID = ss.SettlementStatusID left JOIN TaxPayer_Types tptype ON ast.TaxPayerTypeID = tptype.TaxPayerTypeID left JOIN ERAS.DBO.MST_Users  office ON ast.CreatedBy = office.UserID left join Individual Ind on ast.TaxPayerID = Ind.IndividualID left join Tax_Offices TOff on Ind.TaxOfficeID = TOff.TaxOfficeID where ast.SettlementStatusID = 6  or ast.SettlementStatusID =8 or ast.SettlementStatusID =7";

    }
}