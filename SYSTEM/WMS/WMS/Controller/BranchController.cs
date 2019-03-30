using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WMS.Model;

namespace WMS.Controller
{
    public class BranchController
    {
        wms_service.Service1 wms = new wms_service.Service1();

        public DataSet getBranch()
        {
            DataSet ds = wms.SelectBranch();

            return ds;
        }
        public string getBranchByID(int ID)
        {

            DataSet ds = wms.SelectBranch();
            string PositionName = "";
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["ID"].ToString().Trim() == ID.ToString())
                        {
                            PositionName = row["BranchName"].ToString().Trim();
                        }
                    }
                }
            }
            return PositionName;
        }
        public string InsertBranch(BranchModel model)
        {
            string response = wms.InsertBranch(model.BranchName, model.Status);
            return response;
        }

        public string UpdateBranch(BranchModel model)
        {
            string response = wms.UpdateBranch(model.ID, model.Status);
            return response;
        }
    }
}
