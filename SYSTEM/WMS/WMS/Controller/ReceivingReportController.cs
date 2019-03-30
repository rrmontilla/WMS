using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WMS.Controller
{
    public class ReceivingReportController
    {
        wms_service.Service1 wms = new wms_service.Service1();

        public DataSet getRRData(int userid, string type)
        {
            DataSet ds = new DataSet();
            if (type == "PENDING")
            {
                //wms.pur;
                ds = wms.Get_Pending_RR(userid); // get all pending Receiving Report wating for approval
            }
            else if (type == "RR-NOTED")
            {
                ds = wms.Get_RR_for_Noted(userid);
            }
            else if (type == "RR-APPROVED")
            {
                ds = wms.Get_RR_for_Approved(userid);
            }
            return ds;
        }

        public DataTable RRCount(DataTable RR_Table, int index)
        {
            int count = 0;
            string RR_No;

            DataTable container = new DataTable();

            RR_No = RR_Table.Rows[index]["RRNo"].ToString();//getRO_Requestor(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();

            container = getRRData(RR_Table, RR_No, "Requestor");

            //count = container.Rows.Count;
            return container;
        }

        public DataTable getRRData(DataTable container, string PO_No, string type)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RRNo");
            dt.Columns.Add("PONo");
            dt.Columns.Add("DateReceived");
            dt.Columns.Add("Supplier");
            dt.Columns.Add("Description");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Prepared");
            dt.Columns.Add("Received");
            dt.Columns.Add("Noted");
            dt.Columns.Add("Approver");
            dt.Columns.Add("RRID");
            dt.Columns.Add("RRIDDetails");



            if (container.Rows.Count > 0)
            {
                foreach (DataRow row in container.Rows)
                {
                    if (row["RRNo"].ToString().Trim() == PO_No)
                    {
                        dt.Rows.Add(row["RRNo"].ToString(),
                                    row["PONo"].ToString(),
                                    row["DateReceived"].ToString(),
                                    row["SupplierName"].ToString(),
                                    row["Description"].ToString(),
                                    row["Quantity"].ToString(),
                                    row["Prepared"].ToString(),
                                    row["Received"].ToString(),
                                    row["Noted"].ToString(),
                                    row["Approver"].ToString(),
                                    row["RRID"].ToString(),
                                    row["RRIDDetails"].ToString());
                    }
                }
            }

            return dt;
        }

        public string SubmitRR(string summary, string details, bool stat, int rrid, string ponum)
        {
            string res = "";

            if (stat == false)
            {
                res = wms.InsertReceivingReport(summary, details);
            }
            else
            {
                
            }
            return res;
        }

        public string ApproveRRDetails(DataTable container, string type)
        {
            string response = "";
            int counter = 0;
            if (container.Rows.Count > 0)
            {
                foreach (DataRow row in container.Rows)
                {
                    int POID = int.Parse(row["POID"].ToString());
                    string ret = "";
                    if (type == "Preparation")
                    {
                        //ret = wms.Approve_CanvassItems(canvassID, itemID, SupplierID);
                    }
                    else if (type == "Endorse")
                    {
                        ret = wms.Update_RR_Noted(POID, int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid));
                    }
                    else if (type == "Approved")
                    {
                        ret = wms.Update_RR_Approved(POID, int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid));
                    }

                    if (ret.Trim() == "SUCCESS")
                    {
                        counter++;
                    }

                    response = ret;
                }
            }

            return response;
        }
    }
}
