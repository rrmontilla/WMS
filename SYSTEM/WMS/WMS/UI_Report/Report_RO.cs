using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI_Report
{
    public partial class Report_RO : Form
    {
        string title = "";
        DataSet ds = new DataSet();
        int type = 0;
        public Report_RO(string ttle, DataSet dtSet, int ttype)
        {
            title = ttle;
            ds = dtSet;
            type = ttype;
            InitializeComponent();
        }

        private void Report_RO_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.reportViewer1.RefreshReport();
            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            this.reportViewer1.RefreshReport();
            ReportParameter[] param = new ReportParameter[0];
            if (type == 1)
            {
                ds.Tables[0].TableName = "RO";
                this.Text = "Request Order Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.RO.rdlc";

                param = new ReportParameter[2];
                param[0] = new ReportParameter("Title", "REQUEST ORDER");
                param[1] = new ReportParameter("Date", DateTime.Now.ToShortDateString());
                //param[2] = new ReportParameter("Status", "");

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["RO"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (type == 2)
            {
                ds.Tables[0].TableName = "ROSummary";
                this.Text = "Request Order Summary Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Summary.rdlc";

                string[] titleReport = title.Split('-'); 

                param = new ReportParameter[3];
                param[0] = new ReportParameter("Title", titleReport[0].Trim());
                param[1] = new ReportParameter("Date", DateTime.Now.ToShortDateString());
                param[2] = new ReportParameter("Status", titleReport[1].Trim());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["ROSummary"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (type == 3)
            {
                ds.Tables[0].TableName = "Canvass";
                this.Text = "Canvass Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Canvass.rdlc";

                string[] titleReport = title.Split('$');

                param = new ReportParameter[12];
                param[0] = new ReportParameter("RequestedBy", titleReport[4].Trim());
                param[1] = new ReportParameter("DateRequisition", DateTime.Parse(titleReport[5].Trim().Substring(0, 10)).ToString("F").Replace("00:00:00", "").Trim());
                param[2] = new ReportParameter("Reason", titleReport[6].Trim());
                param[3] = new ReportParameter("Department", titleReport[9].Trim());
                param[4] = new ReportParameter("LastDatePurchase", DateTime.Parse(titleReport[10].Trim().Substring(0, 10)).ToString("F").Replace("00:00:00", "").Trim());
                param[5] = new ReportParameter("TargetDate", DateTime.Parse(titleReport[11].Trim().Substring(0, 10)).ToString("F").Replace("00:00:00", "").Trim());
                param[6] = new ReportParameter("CanvassNo", titleReport[1].Trim());
                param[7] = new ReportParameter("RONo", titleReport[2].Trim());
                param[8] = new ReportParameter("Requester", titleReport[7].Trim());
                param[9] = new ReportParameter("Recommend", titleReport[8].Trim());
                param[10] = new ReportParameter("Approver", titleReport[12].Trim());
                param[11] = new ReportParameter("Date", DateTime.Parse(titleReport[3].Trim().Substring(0, 10)).ToString("F").Replace("00:00:00", "").Trim());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["Canvass"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (type == 4)
            {
                ds.Tables[0].TableName = "CanvassSummary";
                this.Text = "Canvass Summary Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.CanvassSummary.rdlc";

                string[] titleReport = title.Split('-'); 

                param = new ReportParameter[3];
                param[0] = new ReportParameter("Title", titleReport[0].Trim());
                param[1] = new ReportParameter("Date", DateTime.Now.ToShortDateString());
                param[2] = new ReportParameter("Status", titleReport[1].Trim());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["CanvassSummary"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (type == 5)
            {
                ds.Tables[0].TableName = "PO";
                this.Text = "Purchase Order Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.PO.rdlc";

                string[] titleReport = title.Split('-'); 

                param = new ReportParameter[2];
                param[0] = new ReportParameter("Title", titleReport[0].Trim());
                param[1] = new ReportParameter("Date", titleReport[1].Trim());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["PO"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (type == 6)
            {
                ds.Tables[0].TableName = "POSummary";
                this.Text = "Purchase Order Summary Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.POSummary.rdlc";

                string[] titleReport = title.Split('-'); 

                param = new ReportParameter[3];
                param[0] = new ReportParameter("Title", titleReport[0].Trim());
                param[1] = new ReportParameter("Date", DateTime.Now.ToShortDateString());
                param[2] = new ReportParameter("Status", titleReport[1].Trim());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["POSummary"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (type == 7)
            {
                ds.Tables[0].TableName = "RR";
                this.Text = "Receiving Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.RR.rdlc";

                param = new ReportParameter[9];
                param[0] = new ReportParameter("Supplier", ds.Tables["RR"].Rows[0]["Supplier"].ToString());
                param[1] = new ReportParameter("DRNo", "");
                param[2] = new ReportParameter("Date", DateTime.Now.ToShortDateString());
                param[3] = new ReportParameter("RRNo", "");
                param[4] = new ReportParameter("PONo", "");
                param[5] = new ReportParameter("Purpose", "");
                param[6] = new ReportParameter("Received", "");
                param[7] = new ReportParameter("Noted", "");
                param[8] = new ReportParameter("Confirmed", "");


                //Commented by Ren | 2019-03-30 Wa ko kasabot ani mag error wala pakoy RDLC
                //param[0] = new ReportParameter("Title", title.Trim());
                //param[1] = new ReportParameter("Date", DateTime.Now.ToShortDateString());
                //param[2] = new ReportParameter("SupplierName", "");
                //param[3] = new ReportParameter("InvoiceNo", "");
                //param[4] = new ReportParameter("RRNo", "");
                //param[5] = new ReportParameter("PONo", "");
                //param[6] = new ReportParameter("Purpose", "");
                //End
                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["RR"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (title.Contains("ACCOUNT") == true)
            {
                ds.Tables[0].TableName = "Accounts";
                this.Text = "Accounts  Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Accounts.rdlc";
                
                param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", DateTime.Now.ToString("F"));
                param[1] = new ReportParameter("Count", ds.Tables[0].Rows.Count.ToString());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["Accounts"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (title.Contains("SUPPLIER") == true)
            {
                ds.Tables[0].TableName = "Supplier";
                this.Text = "Suppliers Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Supplier.rdlc";

                param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", DateTime.Now.ToString("F"));
                param[1] = new ReportParameter("Count", ds.Tables[0].Rows.Count.ToString());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["Supplier"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (title.Contains("ITEM") == true)
            {
                ds.Tables[0].TableName = "Item";
                this.Text = "Items Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Items.rdlc";

                param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", DateTime.Now.ToString("F"));
                param[1] = new ReportParameter("Count", ds.Tables[0].Rows.Count.ToString());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["Item"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (title.Contains("USER") == true)
            {
                ds.Tables[0].TableName = "User";
                this.Text = "Users Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.User.rdlc";

                param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", DateTime.Now.ToString("F"));
                param[1] = new ReportParameter("Count", ds.Tables[0].Rows.Count.ToString());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["User"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (title.Contains("DEPT") == true)
            {
                ds.Tables[0].TableName = "Department";
                this.Text = "Departments Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Department.rdlc";

                param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", DateTime.Now.ToString("F"));
                param[1] = new ReportParameter("Count", ds.Tables[0].Rows.Count.ToString());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["Department"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (title.Contains("POST") == true)
            {
                ds.Tables[0].TableName = "Position";
                this.Text = "Position Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Position.rdlc";

                param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", DateTime.Now.ToString("F"));
                param[1] = new ReportParameter("Count", ds.Tables[0].Rows.Count.ToString());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["Position"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            else if (title.Contains("BRANCH") == true)
            {
                ds.Tables[0].TableName = "Branch";
                this.Text = "Branch Report";
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Branch.rdlc";

                param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", DateTime.Now.ToString("F"));
                param[1] = new ReportParameter("Count", ds.Tables[0].Rows.Count.ToString());

                ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables["Branch"]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(datasource);
            }

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.SetParameters(param); 

            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;
            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();

            this.reportViewer1.RefreshReport();
            Cursor.Current = Cursors.Default;



        }
    }
}
