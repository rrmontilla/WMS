using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WMS.Controller
{
   public class ReturnInventoryController
    {
        wms_service.Service1 wms = new wms_service.Service1();
        public string SubmitRI(string summary, string details)
        {
            wms.InsertRI(summary, details);

            return "SUCCESS";
        }
        public DataTable getMRIS_Details(int MRIS_ID)
        {
            DataTable dt = wms.Get_MRISDetails(MRIS_ID).Tables[0];
            return dt;
        }
        public DataTable getRI(int userid, string frm_type)
        {
            DataTable dt = new DataTable();
            if (frm_type == "RI_Prep")
            {
                dt = wms.Get_RI(userid, "1").Tables[0];
            }
            else if (frm_type == "Noted")
            {
                dt = wms.Get_RI(userid, "2").Tables[0];
            }
            else if (frm_type == "Confirm")
            {
                dt = wms.Get_RI(userid, "3").Tables[0];
            }
          
            return dt;
        }
        public DataTable getMRIS(int userid, string frm_type)
        {
            DataTable dt = new DataTable();
            if (frm_type == "Preparation")
            {
                dt = wms.Get_MRIS(userid, "1").Tables[0];
            }
            else if (frm_type == "Approved")
            {
                dt = wms.Get_MRIS(userid, "2").Tables[0];
            }
            else if (frm_type == "Issued")
            {
                dt = wms.Get_MRIS(userid, "3").Tables[0];
            }
            else if (frm_type == "RI_Prep")
            {
                dt = wms.Get_MRIS(userid, "4").Tables[0];
            }
            return dt;
        }
        public string Approved_RI(int RIID, string frm_type)
        {
            string response = "";
            if (frm_type == "Noted")
            {
                response = wms.Update_RI(RIID, 1);
            }
            else if (frm_type == "Confirmed")
            {
                response = wms.Update_RI(RIID, 2);
            }
            return response;
        }
        public DataTable getRI_Details(int RI_ID)
        {
            DataTable dt = wms.Get_RIDetails(RI_ID).Tables[0];
            return dt;
        }
        public DataTable RICount(DataTable RI_Table, int index)
        {
            int count = 0;
            string RI_no;

            DataTable container = new DataTable();

            RI_no = RI_Table.Rows[index]["RINo"].ToString();//getRO_Requestor(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();

            container = getRI(RI_Table, RI_no);

            //count = container.Rows.Count;
            return container;
        }
        public DataTable getRI(DataTable container, string RI_No)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RIID");
            dt.Columns.Add("RINo");
            dt.Columns.Add("DocNo");
            dt.Columns.Add("ReturnedByName");
            dt.Columns.Add("ReceivedBYName");
            dt.Columns.Add("NotedByName");
            dt.Columns.Add("ConfirmedByName");
            dt.Columns.Add("Status");
            dt.Columns.Add("ReturnedDate");
            dt.Columns.Add("ReceivedDate");
            dt.Columns.Add("ConfirmedDadte");

            foreach (DataRow row in container.Rows)
            {
                if (row["RINo"].ToString().Trim() == RI_No.Trim())
                {

                    dt.Rows.Add(row["ID"].ToString(),
                           row["RINo"].ToString(),
                           row["DocNo"].ToString(),
                           row["ReturnedByName"].ToString(),
                           row["ReceivedBYName"].ToString(),
                           row["NotedByName"].ToString(),
                           row["ConfirmedByName"].ToString(),
                           row["Status"].ToString(),
                           row["ReturnedDate"].ToString(),
                           row["ReceivedDate"].ToString(),
                            row["ConfirmedDadte"].ToString()
                          );


                }
            }

            return dt;
        }
    }
}
