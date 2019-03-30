using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WMS.Model;

namespace WMS.Controller
{
    public class DepartmentController
    {
        wms_service.Service1 wms = new wms_service.Service1();

        public DataSet getDepartment()
        {
            DataSet ds = wms.SelectDepartment();
            return ds;
        }
        public string getDeptByID(int ID)
        {

            DataSet ds = wms.SelectDepartment();
            string DeptName = "";
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["ID"].ToString().Trim() == ID.ToString())
                        {
                            DeptName = row["DeptName"].ToString().Trim();
                        }
                    }
                }
            }
            return DeptName;
        }
        public string InsertDept(DepartmentModel model)
        {
            string response = wms.InsertDepartment(model.DeptName, model.Status);
            return response;
        }

        public string UpdateDept(DepartmentModel model)
        {
            string response = wms.UpdateDepartment(model.ID, model.Status);
            return response;
        }
    }
}
