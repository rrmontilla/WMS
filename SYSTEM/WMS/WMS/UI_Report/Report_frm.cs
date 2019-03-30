using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.wms_service;

namespace WMS.UI_Report
{
    public partial class Report_frm : Form
    {
        wms_service.Service1 wms = new wms_service.Service1();

        string title = "";
        public Report_frm(string ttle)
        {
            title = ttle;
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        private void button5_Click(object sender, EventArgs e)
        {
            if (title == "Request")
            {
                if (comboBox4.Text == "ALL")
                {
                    UI_Report.Report_RO rro = new Report_RO("Request Order Summary Report Monitoring - " + comboBox4.Text, ds, 2);
                    rro.ShowDialog(); 
                }
                else
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        DataSet ds1 = new DataSet();
                        DataTable dt = new DataTable();
                        dt = (DataTable)dataGridView1.DataSource;
                        ds1.Tables.Add(dt);
                        UI_Report.Report_RO rro = new Report_RO("Request Order Summary Report Monitoring - " + comboBox4.Text, ds1, 2);
                        rro.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No data to print!", "ERROR!");
                    }
                }
            }
            else if (title == "Canvass")
            {
                if (comboBox4.Text == "ALL")
                {
                    UI_Report.Report_RO rro = new Report_RO("Canvass Summary Report Monitoring - " + comboBox4.Text, ds, 4);
                    rro.ShowDialog();
                }
                else
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        DataSet ds1 = new DataSet();
                        DataTable dt = new DataTable();
                        dt = (DataTable)dataGridView1.DataSource;
                        ds1.Tables.Add(dt);
                        UI_Report.Report_RO rro = new Report_RO("Canvass Summary Report Monitoring - " + comboBox4.Text, ds1, 4);
                        rro.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No data to print!", "ERROR!");
                    }
                }
            }
            else if (title == "Purchase")
            {
                if (comboBox4.Text == "ALL")
                {
                    UI_Report.Report_RO rro = new Report_RO("Purchase Order Summary Report Monitoring - " + comboBox4.Text, ds, 6);
                    rro.ShowDialog();
                }
                else
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        DataSet ds1 = new DataSet();
                        DataTable dt = new DataTable();
                        dt = (DataTable)dataGridView1.DataSource;
                        ds1.Tables.Add(dt);
                        UI_Report.Report_RO rro = new Report_RO("Purchase Order Summary Report Monitoring - " + comboBox4.Text, ds1, 6);
                        rro.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No data to print!", "ERROR!");
                    }
                }
            }
        }

        private void Report_frm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        public void LoadData()
        {
            if (title == "Request")
            {
                ds = wms.SelectSummaryReport("RO", dateTimePicker1.Value, dateTimePicker2.Value, Program.loginfrm.userid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    comboBox4.Text = "ALL";
                }
                else
                {
                    MessageBox.Show("NO DATA TO DISPLAY!", "SORRY!");
                }
            }
            else if (title == "Canvass")
            {
                this.Text = "Canvass Monitoring";
                label16.Text = "Canvass Monitoring";

                ds = wms.SelectSummaryReport("CANVASS REPORT", dateTimePicker1.Value, dateTimePicker2.Value, Program.loginfrm.userid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    comboBox4.Text = "ALL";
                }
                else
                {
                    MessageBox.Show("NO DATA TO DISPLAY!", "SORRY!");
                }
            }
            else if (title == "Purchase")
            {
                this.Text = "Purchase Order Monitoring";
                label16.Text = "Purchase Order Monitoring";

                ds = wms.SelectSummaryReport("PURCHASE", dateTimePicker1.Value, dateTimePicker2.Value, Program.loginfrm.userid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    comboBox4.Text = "ALL";
                }
                else
                {
                    MessageBox.Show("NO DATA TO DISPLAY!", "SORRY!");
                }
            }
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox4.Text == "ALL")
                {
                    LoadData();
                    dataGridView1.DataSource = ds.Tables[0];
                }
                else
                {
                    var query1 = ds.Tables[0].AsEnumerable()
                                     .Where(p => p.Field<string>("Status") == comboBox4.Text)
                                     ;

                    if (query1.Any())
                    {
                        dataGridView1.DataSource = query1.CopyToDataTable();
                    }
                    else
                    {
                        MessageBox.Show("No " + comboBox4.Text + " data to show.");
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMWTHING WENT WRONG!", "ERROR!");
            }
        }
    }
}
