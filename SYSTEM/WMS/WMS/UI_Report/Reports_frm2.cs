using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Uploading.UI
{
    public partial class Report : Form
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string title = "";
        public Report(string ttl, DataSet data)
        {
            InitializeComponent();
            ds = data;
            title = ttl;
        }

        private void Report_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            this.Text = title;
            Cursor.Current = Cursors.WaitCursor;
            this.reportViewer1.RefreshReport();
            
            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            //ReportViewer1.ShowToolBar = false;
            //this.reportViewer1.RefreshReport();
            if (title.Contains("SUPPLIER") == true)
            {
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Supplier.rdlc";
            }
            else if (title.Contains("ITEM") == true)
            {
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Items.rdlc";
            }
            else if (title.Contains("ACCOUNT") == true)
            {
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.Accounts.rdlc";
            }
            else if (title.Contains("CONSTRUCTION") == true)
            {
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "WMS.UI_Report.ConstructionType.rdlc";
            }
            
            
            ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(datasource);


            ReportParameter[] parameter = new ReportParameter[2]; 
            parameter[0] = new ReportParameter("Date", DateTime.Now.ToString("F"));
            parameter[1] = new ReportParameter("Count", ds.Tables[0].Rows.Count.ToString());
            //parameter[2] = new ReportParameter("PrintedDate", prDate.Trim());
            //parameter[3] = new ReportParameter("DeliveryRefNo", deliveryNum.Trim());
            //parameter[4] = new ReportParameter("BatchNo", batchNo);

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.SetParameters(parameter);

            //System.Drawing.Printing.PageSettings ps = new System.Drawing.Printing.PageSettings();
            //ps.Landscape = true;
            //ps.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20);
            //ps.PaperSize = new System.Drawing.Printing.PaperSize("Letter", 827, 1100);
            //ps.PaperSize.RawKind = (int)System.Drawing.Printing.PaperKind.Letter;
            //reportViewer2.SetPageSettings(ps);
            //reportViewer1.SetPageSettings(setup);

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
