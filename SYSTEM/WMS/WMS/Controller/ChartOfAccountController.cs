using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WMS.Controller
{
    public class ChartOfAccountController
    {
        wms_service.Service1 wms = new wms_service.Service1();
        public int InsertAccounts(string xmlData)
        {
            int retval = 0;

            return retval;
        }

        public DataSet getAccount()
        {
            DataSet ds = new DataSet();
            return ds;
        }
    }
}
