using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WMS.Controller
{
    public class MRISController
    {
        wms_service.Service1 wms = new wms_service.Service1();
        public string SubmitMRIS(string summary,string details)
        {
            wms.InsertMRIS(summary,details);

            return "SUCCESS";
        }

        public DataTable getMRIS(int userid,string frm_type)
        {
            DataTable dt = new DataTable();
            if (frm_type == "Preparation")
            {
                dt = wms.Get_MRIS(userid,"1").Tables[0];
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
        public string Approved_MRIS(int MRISID,string frm_type)
        {
            string response = "";
            if (frm_type == "Approved")
            {
                response = wms.Update_MRIS(MRISID,1);
            }
            else if (frm_type == "Issued")
            {
                response = wms.Update_MRIS(MRISID, 2);
            }
            return response;
        }
        public DataTable getMRIS_Details(int RO_ID)
        {
            DataTable dt = wms.Get_MRISDetails(RO_ID).Tables[0];
            return dt;
        }
        public DataTable MRISCount(DataTable MRIS_Table, int index)
        {
            int count = 0;
            string MRIS_no;

            DataTable container = new DataTable();

            MRIS_no = MRIS_Table.Rows[index]["MRISNo"].ToString();//getRO_Requestor(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();

            container = getMRIS(MRIS_Table, MRIS_no);

            //count = container.Rows.Count;
            return container;
        }
        public DataTable getMRIS(DataTable container, string MRIS_No)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MRISID");
            dt.Columns.Add("MRISNo");
            dt.Columns.Add("Purpose");
            dt.Columns.Add("DateNeeded");
            dt.Columns.Add("DateRequested");
            dt.Columns.Add("Requestor");
            dt.Columns.Add("Approver");
            dt.Columns.Add("IssuerName");
            dt.Columns.Add("Department");
            dt.Columns.Add("Status");
            

            foreach (DataRow row in container.Rows)
            {
                if (row["MRISNo"].ToString().Trim() == MRIS_No.Trim())
                {
                   
                        dt.Rows.Add(row["ID"].ToString(),
                               row["MRISNo"].ToString(),
                               row["Purpose"].ToString(),
                               row["DateNeeded"].ToString(),
                               row["DateRequested"].ToString(),
                               row["Requestor"].ToString(),
                               row["Approver"].ToString(),
                               row["IssuerName"].ToString(),
                               row["Department"].ToString(),
                               row["Status"].ToString()
                              );
                   

                }
            }

            return dt;
        }

    }
}
