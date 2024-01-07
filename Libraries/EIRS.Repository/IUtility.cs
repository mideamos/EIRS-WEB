using EIRS.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public interface IUtility
    {
        bool BL_TaxPayerCreated(EmailDetails pObjEmailDetails);
        bool BL_AssetProfileLinked(EmailDetails pObjEmailDetails);
        bool BL_BillGenerated(EmailDetails pObjEmailDetails);
        bool BL_SettlementReceived(EmailDetails pObjEmailDetails);
        bool BL_PaymentonAccount(EmailDetails pObjEmailDetails);
    }
}
