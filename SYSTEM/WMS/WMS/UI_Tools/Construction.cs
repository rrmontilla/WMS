using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WMS.Class;
using WMS.Controller;

namespace Uploading.UI
{
    public partial class Construction : Form
    {
        ConstructionController cons = new ConstructionController();
        DataTable newDt = new DataTable();

        public Construction()
        {
            InitializeComponent();
        }

        OpenFileDialog file;
        string filePath = string.Empty;
        DataTable dtUpload;
        DataTable tmpTbl = new DataTable();
        

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                Cursor.Current = Cursors.Default;
                filePath = file.FileName; //get the path of the file  

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
                        if (dataGridView1.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dtUpload;
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
                                // ShowData();
                            }
                        }
                        catch (Exception)
                        {
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("NO DATA TO SAVE!", "ERROR!");
                }
                else
                {
                    tmpTbl = (DataTable)dataGridView1.DataSource;
                    int retVal = cons.InsertConstruction(ToXML.Toxml(tmpTbl));

                    if (retVal == 1)
                    {
                        MessageBox.Show("NEW DATA HAS BEEN SUCCESSFULLY ADDED!", "CONFIRM!");
                        this.Close();
                    }
                    else if (retVal == 2)
                    {
                        MessageBox.Show("MULTIPLE DATA FOUND!", "ERROR!");
                        dataGridView1.DataSource = null;
                    }
                    else
                    {
                        MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox3.Text.Length == 0 || textBox4.Text.Length == 0)
            {
                MessageBox.Show("SOMETHING WENT WRONG! PLEASE FILL IN ALL THE DATA INFORMATION!", "ERROR!");
            }
            else
            {
                textBox4.ReadOnly = true;
                if (int.Parse(textBox1.Text) > 1)
                {
                    textBox3.Text = textBox3.Text + "s";
                }

                newDt.Rows.Add(comboBox1.Text.Split('~')[1].Trim(),
                                   comboBox1.Text.Split('~')[0].Trim(),
                                   textBox1.Text,
                                   textBox3.Text,
                                   textBox4.Text);

                dataGridView1.DataSource = newDt;
                textBox1.Text = "";

            }
        }

        private void Construction_Load(object sender, EventArgs e)
        {
            fill_items();

            newDt.Columns.Add("ItemCode", typeof(string));
            newDt.Columns.Add("Description", typeof(string));
            newDt.Columns.Add("Quantity", typeof(string));
            newDt.Columns.Add("Unit", typeof(string));
            newDt.Columns.Add("GroupCode", typeof(string));
        }

        public void fill_items()
        {

            foreach (DataRow row in cons.selectItems().Tables[0].Rows)
            {
                WMS.Model.ConstructionModel items = new WMS.Model.ConstructionModel();
                items.Description = row["Description"].ToString() + " ~ " + row["ItemCode"].ToString();
                //items.Value = row["ID"].ToString();
                comboBox1.Items.Add(items.Description);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
