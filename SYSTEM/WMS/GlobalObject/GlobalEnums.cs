using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalObject
{
    public enum SignatoryType
    {
            Approver,
            Noted
    }
    public enum TransactionType
    {
        RequestOrder,
        Canvass,
        PurchaseOrder,
        ReceivingReport,
        ReturnInventory
    }
}
