using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WMS.Controller
{
    public class PurchaseOrderController
    {
        wms_service.Service1 wms = new wms_service.Service1();
        
        //canvass dummy data
        public string SubmitPO(string summary, string details)
        {
            string res = "";
            res = wms.InsertPurchaseOrder(summary, details);
            return res;
        }
        public DataSet getPOData(int userid, string type)
        {
            DataSet ds = new DataSet();
            if (type == "PENDING")
            {
                //wms.pur;
                ds = wms.Get_Pending_PO(userid); // get all pending Purchase wating for approval
            }
            else if (type == "PO-NOTED")
            {
                ds = wms.Get_PO_for_Noted(userid);
            }
            else if (type == "PO-APPROVED")
            {
                ds = wms.Get_PO_for_Approved(userid);
            }
            return ds;
        }

        public string removeCanvass()
        {
            string res = "";


            return res;
        }
        
        public DataTable POCount(DataTable PO_Table, int index)
        {
            int count = 0;
            string PO_No;

            DataTable container = new DataTable();

            PO_No = PO_Table.Rows[index]["PONumber"].ToString();//getRO_Requestor(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();

            container = getPOData(PO_Table, PO_No, "Requestor");

            //count = container.Rows.Count;
            return container;
        }

        public DataTable getPOData(DataTable container, string PO_No, string type)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ROID");
            dt.Columns.Add("POID");
            dt.Columns.Add("CanvassNumber");
            dt.Columns.Add("RONumber");
            dt.Columns.Add("PONumber");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("DatePrepared");
            dt.Columns.Add("TargetDate");
            dt.Columns.Add("Canvassor");
            dt.Columns.Add("Endorser");
            dt.Columns.Add("Approver");



            if (container.Rows.Count > 0)
            {
                foreach (DataRow row in container.Rows)
                {
                    if (row["PONumber"].ToString().Trim() == PO_No)
                    {
                        dt.Rows.Add(row["ROID"].ToString(),
                                    row["ID"].ToString(),
                                    row["CanvassNumber"].ToString(),
                                    row["RONumber"].ToString(),
                                    row["PONumber"].ToString(),
                                    row["Remarks"].ToString(),
                                    row["DateRequested"].ToString(),
                                    row["TargetDate"].ToString(),
                                    row["Canvassor"].ToString(),
                                    row["Noter"].ToString(),
                                    row["Approver"].ToString());
                    }
                }
            }

            return dt;
        }

        public DataSet getPODetails(int ROID)
        {
            DataSet ds = new DataSet();
            ds = wms.Get_PODetailsAll(ROID);

            return ds;
        }

        public string ApprovePODetails(DataTable container, string type)
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
                        ret = wms.Update_PO_Noted(POID, int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid));
                    }
                    else if (type == "Approved")
                    {
                        ret = wms.Update_PO_Approved(POID, int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid));
                    }

                    if (ret.Trim() == "SUCCESS")
                    {
                        counter++;
                    }
                }
            }

            if (counter == container.Rows.Count)
            {
                response = "SUCCESS";
            }
            else
            {
                response = "Unable to save all data!";
            }

            //

            return response;
        }

        public DataSet getPOApproved()
        {
            DataSet ds = new DataSet();

            ds = wms.Select_PO_Approved();

            return ds;
        }
    }
}
