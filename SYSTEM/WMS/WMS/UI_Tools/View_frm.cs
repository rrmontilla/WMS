using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI_Tools
{
    public partial class View_frm : Form
    {
        wms_service.Service1 wms = new wms_service.Service1();
        string name = "";
        public View_frm(string data)
        {
            InitializeComponent();
            name = data;
        }

        DataTable dtView = new DataTable();
        DataSet ds = new DataSet();
        int stat = 0;
        private void View_frm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dtView.Clear();
            try
            {
                if (name == "SUPPLIER")
                {
                    this.Text = "SUPPLIER";
                    groupBox1.Text = "View Suppliers:";
                    label1.Text = "Search a Supplier:";

                    ds = Class.ToXML.ToDataSet(wms.GetAllData("Supplier", ""));
                }
                else if (name == "ITEM")
                {
                    this.Text = "ITEM";
                    groupBox1.Text = "View Items:";
                    label1.Text = "Search an Items:";

                    ds = Class.ToXML.ToDataSet(wms.GetAllData("Items", ""));
                }
                else if (name == "ACCOUNT")
                {
                    this.Text = "ACCOUNT";
                    groupBox1.Text = "View Accounts:";
                    label1.Text = "Search an Accounts:";

                    ds = Class.ToXML.ToDataSet(wms.GetAllData("Accounts", ""));
                    
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dgView.DataSource = ds.Tables[0];

                    label3.Text = dgView.Rows.Count.ToString();
                }
                else
                {
                    MessageBox.Show("NO DATA SHOW!", "SORRY!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG.", "ERROR!");
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (name == "SUPPLIER")
                {
                    var query1 = ds.Tables[0].AsEnumerable()
                        .Where(p => p.Field<string>("SupplierName").ToLower().Contains(txtSearch.Text.ToLower()))
                        ;

                    if (query1.Any())
                    {
                        dgView.DataSource = query1.CopyToDataTable();
                        label3.Text = dgView.Rows.Count.ToString();
                        stat = 1;
                    }
                    else
                    {
                        MessageBox.Show("NO RECORD FOUND.", "SORRY!");
                        DisplayData();
                    }
                }
                else if (name == "ITEM")
                {
                    var query1 = ds.Tables[0].AsEnumerable()
                        .Where(p => p.Field<string>("Description").ToLower().Contains(txtSearch.Text.ToLower()))
                        ;

                    if (query1.Any())
                    {
                        dgView.DataSource = query1.CopyToDataTable();
                        label3.Text = dgView.Rows.Count.ToString();
                        stat = 1;
                    }
                    else
                    {
                        MessageBox.Show("NO RECORD FOUND.", "SORRY!");
                        DisplayData();
                    }
                }
                else if (name == "ACCOUNT")
                {
                    var query1 = ds.Tables[0].AsEnumerable()
                       .Where(p => p.Field<string>("Description").ToLower().Contains(txtSearch.Text.ToLower()))
                       ;

                    if (query1.Any())
                    {
                        dgView.DataSource = query1.CopyToDataTable();
                        label3.Text = dgView.Rows.Count.ToString();
                        stat = 1;
                    }
                    else
                    {
                        MessageBox.Show("NO RECORD FOUND.", "SORRY!");
                        DisplayData();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG.", "ERROR!");
            }
        }

        public void DisplayData()
        {
            dgView.DataSource = ds.Tables[0];
            label3.Text = dgView.Rows.Count.ToString();
            stat = 1;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string title = "";
            if (name == "ACCOUNT")
            {
                title = "CHART OF ACCOUNTS";
            }
            else if (name == "ITEM")
            {
                title = "LIST OF ITEMS";
            }
            else if (name == "SUPPLIER")
            {
                title = "LIST OF SUPPLIERS";
            }

            Cursor.Current = Cursors.WaitCursor;

            if (stat == 1)
            {
                DataSet dsNew = new DataSet();
                DataTable dtNew = new DataTable();
                dtNew = (DataTable)dgView.DataSource;
                dsNew.Tables.Add(dtNew);
                UI_Report.Report_RO rpt = new UI_Report.Report_RO(title, dsNew, 0);
                rpt.ShowDialog();
            }
            else
            {
                UI_Report.Report_RO rpt = new UI_Report.Report_RO(title, ds, 0);
                rpt.ShowDialog();
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
