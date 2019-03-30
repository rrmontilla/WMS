using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WMS.Model;

namespace WMS.Controller
{
    public class PositionController
    {
        wms_service.Service1 wms = new wms_service.Service1();
        public DataSet getPosition()
        {
            DataSet ds = wms.SelectPosition();
            return ds;
        }

        public string getPositionByID(int ID)
        {
            
            DataSet ds = wms.SelectPosition();
            string positionname = "";
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["ID"].ToString().Trim() == ID.ToString())
                        {
                            positionname = row["PositionName"].ToString().Trim();
                        }
                    }
                }
            }
            return positionname;
        }
        public string InsertPosition(PositionModel model)
        {
            string response = wms.InsertPosition(model.PositionName,model.Status);
            return response;
        }

        public string InsertDepartment(PositionModel model)
        {
            string response = wms.InsertDepartment(model.PositionName, model.Status);
            return response;
        }

        public string UpdatePosition(PositionModel model)
        {
            string response = wms.UpdateDepartment(model.ID, model.Status);
            return response;
        }
    }
}
