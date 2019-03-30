using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.Class;
using WMS.Controller;

namespace WMS
{
    public partial class COA_frm : Form
    {
        ChartOfAccountController coa = new ChartOfAccountController();
        wms_service.Service1 wms = new wms_service.Service1();
        public COA_frm()
        {
            InitializeComponent();
        }

        OpenFileDialog file;
        static string filePath = string.Empty;
        DataTable dtUpload;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable newDt = new DataTable();
                newDt.Columns.Add("ACCOUNT CODE", typeof(string));
                newDt.Columns.Add("PCC", typeof(string));
                newDt.Columns.Add("DESCRIPTION", typeof(string));
                newDt.Columns.Add("DEBIT CREDIT", typeof(string));
                newDt.Columns.Add("ID", typeof(string));
                newDt.Columns.Add("SL", typeof(string));
                newDt.Columns.Add("GROUP", typeof(string));

                if (txtACode.Text.Length == 0 || txtDesc.Text.Length == 0 || txtDrCr.Text.Length == 0
                    || txtGroup.Text.Length == 0 || txtID.Text.Length == 0 || txtPCC.Text.Length == 0
                    || txtSL.Text.Length == 0)
                {
                    MessageBox.Show("SOMETHING WENT WRONG! PLEASE FILL IN ALL THE DATA INFORMATION!", "ERROR!");
                }
                else
                {
                    newDt.Rows.Add(txtACode.Text, txtPCC.Text, txtDesc.Text, txtDrCr.Text
                        , txtID.Text, txtSL.Text, txtGroup.Text);

                    int retVal = wms.InsertAccounts(ToXML.Toxml(newDt));

                    if (retVal == 1)
                    {
                        MessageBox.Show("NEW DATA HAS BEEN SUCCESSFULLY ADDED!", "CONFIRM!");
                        this.Close();
                    }
                    else if (retVal == 2)
                    {
                        MessageBox.Show("MULTIPLE DATA FOUND!", "ERROR!");
                    }
                    else
                    {
                        MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedrow = dataGridView1.Rows[selectedindex];
               // txtICcodeTag.Text = Convert.ToString(selectedrow.Cells["ItemCodeTag"].Value);
                
                button1.Text = "Update";
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void COA_frm_Load(object sender, EventArgs e)
        {
            displayData();
        }
        public void displayData()
        {
            DataSet ds = coa.getAccount();
            if (ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    int retVal = wms.InsertAccounts(ToXML.Toxml(dtUpload));

                    if (retVal == 1)
                    {
                        MessageBox.Show("NEW DATA HAS BEEN SUCCESSFULLY ADDED!", "CONFIRM!");
                        this.Close();
                    }
                    else if (retVal == 2)
                    {
                        MessageBox.Show("MULTIPLE DATA FOUND!", "ERROR!");
                    }
                    else
                    {
                        MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
                    }
                }
                else
                {
                    MessageBox.Show("NO DATA TO BE UPLOADED!", "ERROR!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            file = new OpenFileDialog();
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor.Current = Cursors.Default;
                filePath = file.FileName;

                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.DataSet DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;

                try
                {
                    try
                    {
                        MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filePath + "';Extended Properties=Excel 8.0;");
                        MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                        DtSet = new System.Data.DataSet();
                        MyCommand.Fill(DtSet);

                        dataGridView1.DataSource = DtSet.Tables[0];
                        MyConnection.Close();
                        dtUpload = DtSet.Tables[0].Copy();
                        if (dtUpload.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dtUpload;
                            //txtBoxUpload.Text = dtUpload.Rows.Count.ToString();
                            //ShowData();
                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties='Excel 12.0;HDR=YES';");
                            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                            DtSet = new System.Data.DataSet();
                            MyCommand.Fill(DtSet);

                            dataGridView1.DataSource = DtSet.Tables[0];
                            MyConnection.Close();
                            dtUpload = DtSet.Tables[0].Copy();
                            if (dtUpload.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dtUpload;
                                //txtBoxUpload.Text = dtUpload.Rows.Count.ToString();
                                //ShowData();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            MessageBox.Show("SOMETHING WENT WRONG!\n\nTRY TO CHECK THE SHEET NAME IT MUST BE 'Sheet1' or KINDLY CLOSE THE EXCEL FILE FIRST THEN TRY AGAIN.\n\n", "ERROR!");
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("SOMETHING WENT WRONG!\n\nTRY TO CHECK THE SHEET NAME IT MUST BE 'Sheet1' or KINDLY CLOSE THE EXCEL FILE FIRST THEN TRY AGAIN.\n\n", "ERROR!");
                }
                Cursor.Current = Cursors.Default;

            }

            Cursor.Current = Cursors.Default;  
        }
    }
}
