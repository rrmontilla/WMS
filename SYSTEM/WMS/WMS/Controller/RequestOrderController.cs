using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WMS.Controller
{
    public class RequestOrderController
    {
        wms_service.Service1 wms = new wms_service.Service1();
        #region Request order preparation
        public string SubmitRO(string summary,string details)
        {
            string res = wms.InsertRequestOrder(summary,details);

            return res;
        }

        public  DataTable RequestOrderCount(DataTable RO_Table, int index)
        {
            int count = 0;
            string RO_no;
           
            DataTable container = new DataTable();

            RO_no = RO_Table.Rows[index]["RONumber"].ToString();//getRO_Requestor(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();

            container = getRequestOrder(RO_Table,RO_no, "Requestor");

            //count = container.Rows.Count;
            return container;
        }
        public DataTable getROByRONumber(string RO_No)
        {
            DataTable dt = new DataTable();
            dt = wms.Get_RO_ByRONumber(int.Parse(RO_No)).Tables[0];
            return dt;
        }
        public DataTable getRequestOrder(DataTable container,string RO_No,string type)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RequestDate");
            dt.Columns.Add("TargetDate");
            dt.Columns.Add("RONumber");
            dt.Columns.Add("Requestor");
            dt.Columns.Add("RequestorID");
            dt.Columns.Add("Endorser");
            dt.Columns.Add("EndorserID");
            dt.Columns.Add("DateEndorse");
            dt.Columns.Add("Recommender");
            dt.Columns.Add("RecommenderID");
            dt.Columns.Add("DateRecommend");
            dt.Columns.Add("Approver");
            dt.Columns.Add("ApproverID");
            dt.Columns.Add("DateApproved");
            dt.Columns.Add("Urgent");
            dt.Columns.Add("ROID");
            dt.Columns.Add("Purpose");

            //DataTable container = new DataTable();

            //if (type == "Requestor")
            //{
            //    container = getRO_Requestor(int.Parse(Program.loginfrm.userid));
            //}
            //else if(type == "Endorser")
            //{
            //    container = getEndorser(int.Parse(Program.loginfrm.userid));
            //}
            //else if (type == "Recommend")
            //{
            //    container = getForReccomendation(int.Parse(Program.loginfrm.userid));
            //}
            //else if (type == "Approved")
            //{
            //    container = getForApproved(int.Parse(Program.loginfrm.userid));
            
            //}

            foreach (DataRow row in container.Rows)
            {
                if (row["RONumber"].ToString().Trim() == RO_No.Trim())
                {
                    if (type == "Requestor")
                    {
                        dt.Rows.Add(row["DateRequested"].ToString(),
                               row["TargetDate"].ToString(),
                               row["RONumber"].ToString(),
                               row["Requestor"].ToString(),
                               row["RequestorID"].ToString(),
                               row["Endorser"].ToString(),
                               row["EndorserID"].ToString(),
                               "",
                               "",
                               "",
                               "",
                               row["Approver"].ToString(),
                               row["ApproverID"].ToString(),
                               "",
                               row["Urgent"].ToString(),
                               row["ID"].ToString(),
                               row["Remarks"].ToString());
                    }
                    else if (type == "Endorser")
                    {
                        dt.Rows.Add(row["DateRequested"].ToString(),
                               row["TargetDate"].ToString(),
                               row["RONumber"].ToString(),
                               row["Requestor"].ToString(),
                               row["RequestorID"].ToString(),
                               row["Endorser"].ToString(),
                               row["EndorserID"].ToString(),
                               "",
                               "",
                               "",
                               "",
                               row["Approver"].ToString(),
                               row["ApproverID"].ToString(),
                               "",
                               row["Urgent"].ToString(),
                               row["ID"].ToString(),
                               row["Remarks"].ToString());

                    }
                    //else if (type == "Recommend")
                    //{
                    //    dt.Rows.Add(row["DateRequested"].ToString(),
                    //           row["TargetDate"].ToString(),
                    //           row["RONumber"].ToString(),
                    //           row["Requestor"].ToString(),
                    //           row["RequestorID"].ToString(),
                    //           row["Endorser"].ToString(),
                    //           row["EndorserID"].ToString(),
                    //           row["DateEndorse"].ToString(),
                    //           row["Recommender"].ToString(),
                    //           row["RecommendersID"].ToString(),
                    //           "",
                    //           row["Approver"].ToString(),
                    //           row["ApproverID"].ToString(),
                    //           "",
                    //           row["Urgent"].ToString(),
                    //           row["ID"].ToString());
                    //}
                    else if (type == "Approved")
                    {
                        dt.Rows.Add(row["DateRequested"].ToString(),
                               row["TargetDate"].ToString(),
                               row["RONumber"].ToString(),
                               row["Requestor"].ToString(),
                               row["RequestorID"].ToString(),
                               row["Endorser"].ToString(),
                               row["EndorserID"].ToString(),
                               row["DateEndorse"].ToString(),
                               "",
                               "",
                               row["DateRecommended"].ToString(),
                               row["Approver"].ToString(),
                               row["ApproverID"].ToString(),
                               row["DateApproved"].ToString(),
                               row["Urgent"].ToString(),
                               row["ID"].ToString(),
                               row["Remarks"].ToString());
                    }

                   
                }
            }
            
            return dt;
        }

        public DataTable getRO_Requestor(int userid)
        {
            DataTable dt = wms.Get_RO_Requestor(userid).Tables[0];
            return dt;
        }

        public DataSet Print_Transaction(int userID, string RONum, string title)
        {
            DataSet ds = wms.Print_Transaction(userID, RONum, title);
            return ds;
        }
        public DataTable getRO_Details(int RO_ID)
        {
            DataTable dt = wms.Get_RO_Details(RO_ID).Tables[0];
            return dt;
        }
        #endregion

        #region RO endorser
        public DataTable EndorseCount(DataTable RO_Endorser, int index)
        {
            int count = 0;
            string RO_no;

            DataTable container = new DataTable();
            try
            {
                RO_no = RO_Endorser.Rows[index]["RONumber"].ToString();//getEndorser(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();
                container = getRequestOrder(RO_Endorser,RO_no, "Endorser");            
            }
            catch (Exception ex)
            { 
            
            }

            //count = container.Rows.Count;
            return container;
        }

        public DataTable getEndorser(int userid)
        {
            DataTable dt = new DataTable();
            dt = wms.Get_RO_Endorser(userid).Tables[0];
            return dt;
        }

        public string SubmitEndorse(int ROID, int UserId)
        {
            string response = "";
           response = wms.RO_UpdateForEndorse(ROID.ToString(), UserId);
            return response;
        }

        #endregion

        #region Recommendation
        public DataTable RecommendCount(DataTable RO_Reccomend,int index)
        {
            int count = 0;
            string RO_no;

            DataTable container = new DataTable();
            try
            {
                RO_no = RO_Reccomend.Rows[index]["RONumber"].ToString();//getForReccomendation(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();

                container = getRequestOrder(RO_Reccomend,RO_no, "Recommend");
            }
            catch (Exception ex)
            { 
            
            }
            //count = container.Rows.Count;
            return container;
        }
        public DataTable getForReccomendation(int userid)
        {
            DataTable dt = new DataTable();
            dt = wms.Get_RO_Recommenders(userid).Tables[0];
            return dt;
        }
        public string SubmitRecommend(int ROID)
        {
            string response = "";
            response = wms.RO_UpdateForRecommend(ROID.ToString());
            return response;
        }

        #endregion

        #region Approver
        public DataTable ApprovedCount(DataTable RO_Approved,int index)
        {
            int count = 0;
            string RO_no;

            DataTable container = new DataTable();
            try
            {
                RO_no = RO_Approved.Rows[index]["RONumber"].ToString();// getForApproved(int.Parse(Program.loginfrm.userid)).Rows[index]["RONumber"].ToString();//crud.getSalesSalesNo().Rows[index]["SalesNo"].ToString();

                container = getRequestOrder(RO_Approved,RO_no, "Approved");
            }
            catch (Exception ex)
            { 
            
            }

            //count = container.Rows.Count;
            return container;
        }
        public DataTable getForApproved(int userid)
        {
            DataTable dt = new DataTable();
            dt = wms.Get_RO_Approver(userid).Tables[0];
            return dt;
        }
        public string SubmitApproved(int ROID, int pUserId)
        {
            string response = "";
            response = wms.RO_UpdateForApproved(ROID.ToString(), pUserId);
            return response;
        }

        public DataSet getApprovedRO()
        {
            DataSet ds = new DataSet();
            ds = wms.Select_RO_Approve();
            return ds;
        }

        public DataSet getApprovedDetails(int ROID)
        {
            DataSet ds = new DataSet();
            ds = wms.Get_RO_Details(ROID);
            return ds;
        }

        #endregion

        #region Request Order Get RO Requestor BY RONum
        public DataSet GetROByRONum(string RONum)
        {
            DataSet ds = new DataSet();
            ds = wms.Get_RO_BY_RONumber(RONum);
            return ds;
        }
        #endregion
    }
}
