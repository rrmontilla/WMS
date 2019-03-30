using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WMS.Controller
{
   public class CanvassController
    {
       wms_service.Service1 wms = new wms_service.Service1();
       public string SubmitCanvass(string summary, string details,bool type,int CanvassID)
       {
           string res = "";
           if (type)
           {
               res = wms.InsertCanvass(summary, details);
           }
           else
           {
               res = wms.InsertCanvassDetails(details, CanvassID);
           }
          

           return res;
       }


       public DataSet getCanvasData(int userid,string type)
       {
           DataSet ds = new DataSet();
           if (type == "PENDING")
           {
               ds = wms.Get_Canvass(userid);
           }
           else if (type == "CANVASS-PENDING")
           {
               ds = wms.Get_Canvass_for_Noted(userid);
           }
           else if (type == "CANVASS-NOTED")
           {
               ds = wms.Get_Canvass_for_Approved(userid);
           }
           return ds;
       }

       public string removeCanvass()
       {
           string res = "";

          
           return res;
       }

       public DataTable CanvassCount(DataTable Canvass_Table, int index)
       {
           int count = 0;
           string Canvass_No;

           DataTable container = new DataTable();

           Canvass_No = Canvass_Table.Rows[index]["CanvassNumber"].ToString();//getRO_Requestor(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();

           container = getCanvassData(Canvass_Table, Canvass_No, "Requestor");

           //count = container.Rows.Count;
           return container;
       }

       public DataTable getCanvassData(DataTable container, string Canvass_No, string type)
       {
           DataTable dt = new DataTable();
           dt.Columns.Add("ROID");
           dt.Columns.Add("CanvassID");
           dt.Columns.Add("CanvassNumber");
           dt.Columns.Add("RONumber");
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
                   if (row["CanvassNumber"].ToString().Trim() == Canvass_No)
                   {
                       dt.Rows.Add(row["ROID"].ToString(),
                                   row["ID"].ToString(),
                                   row["CanvassNumber"].ToString(), 
                                   row["RONumber"].ToString(), 
                                   row["Remarks"].ToString(), 
                                   row["DatePrepared"].ToString(), 
                                   row["TargetDate"].ToString(),
                                   row["Canvassor"].ToString(),
                                   row["Noter"].ToString(),
                                   row["Approver"].ToString());
                   }
               }
           }

           return dt;
       }

       public DataSet getCanvassDetails(int CanvassID)
       {
           DataSet ds = new DataSet();
           ds = wms.Get_CanvassDetailsAll(CanvassID);
          
           return ds;
       }

       public DataSet getCanvassDetails1(int CanvassID)
       {
           DataSet ds = new DataSet();
           ds = wms.Get_CanvassDetailsAll1(CanvassID);

           return ds;
       }

       public string ApproveCanvassDetails(DataTable container,string type)
       {
           string response = "";
           int counter = 0;
           if (container.Rows.Count > 0)
           {
               foreach (DataRow row in container.Rows)
               {
                   int canvassID = int.Parse(row["CanvassID"].ToString());
                   int itemID = int.Parse(row["ItemID"].ToString());
                   int SupplierID = int.Parse(row["SupplierID"].ToString());
                   string ret = "";
                   if (type == "Preparation")
                   {
                        ret = wms.Approve_CanvassItems(canvassID, itemID, SupplierID);
                   }
                   else if (type == "Endorse")
                   {
                       ret = wms.Update_Canvass_Noted(canvassID, int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid));
                   }
                   else if (type == "Approved")
                   {
                       ret = wms.Approve_CanvassItems(canvassID, itemID, SupplierID);
                       ret = wms.Update_Canvass_Approved(canvassID, int.Parse(string.IsNullOrEmpty(Program.loginfrm.userid) ? "0" : Program.loginfrm.userid));
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

       public DataTable getCanvasApproved()
       {
         DataSet ds =  wms.Select_Canvass_Approved();
         DataTable dt = new DataTable();

         if (ds.Tables.Count > 0)
         {
             if (ds.Tables[0].Rows.Count > 0)
             {
                 dt = ds.Tables[0];
             }
         }
         return dt;
       }

       public DataSet GetPrintableReport(string canvassNo, DateTime tdt1, DateTime tdt2, string id)
       {
           DataSet ds = wms.SelectSummaryReport(canvassNo, tdt1, tdt2, id);

           return ds;
       }

    }
}
