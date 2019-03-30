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
using WMS.Model;

namespace WMS
{
    public partial class items_frm : Form
    {
        ItemsController items = new ItemsController();
        public int ItemID = 0;
        public string  ItemCode = "";
        public items_frm()
        {
            InitializeComponent();
        }

        OpenFileDialog file;
        static string filePath = string.Empty;
        DataTable dtUpload;
        int Stat = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable newDt = new DataTable();
            newDt.Columns.Add("ITEM CODE", typeof(string));
            newDt.Columns.Add("ITEM NAME", typeof(string));
            newDt.Columns.Add("DESCRIPTION", typeof(string));
            newDt.Columns.Add("BRAND", typeof(string));
            newDt.Columns.Add("UOM", typeof(string));
            newDt.Columns.Add("SUPPLIER", typeof(string));
            newDt.Columns.Add("SAFETY LVL", typeof(string));
            newDt.Columns.Add("LEAD DELIVERY", typeof(string));
            newDt.Columns.Add("INVENTORY", typeof(string));

            string yn = "";

            if (txtBrand.Text.Length == 0 || txtDescription.Text.Length == 0 || txtICcodeTag.Text.Length == 0
                || txtIName.Text.Length == 0 || txtLTDelivery.Text.Length == 0 || txtSSLevel.Text.Length == 0
                || txtSupName.Text.Length == 0 || txtUnit.Text.Length == 0)
            {
                MessageBox.Show("SOMETHING WENT WRONG! PLEASE FILL IN ALL THE DATA INFORMATION!", "ERROR!");
            }
            else
            {
                if (rdBYes.Checked == true)
                {
                    yn = "Y";
                }
                else
                {
                    yn = "N";
                }
              

                if (button1.Text == "Save")
                {
                    newDt.Rows.Add(txtICcodeTag.Text, txtIName.Text, txtDescription.Text, txtBrand.Text
                    , txtUnit.Text, txtSupName.Text, txtSSLevel.Text, txtLTDelivery.Text
                    , yn);
                    int retVal = items.insertItem(ToXML.Toxml(newDt));

                    if (retVal == 1)
                    {
                        displayData();
                        MessageBox.Show("NEW DATA HAS BEEN SUCCESSFULLY ADDED!", "CONFIRM!");
                       // this.Close();
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
                else if(button1.Text == "Update")
                {
                    ItemModel model = new ItemModel();
                    model.ID = ItemID;
                    model.itemCode  = ItemCode;
                    model.itemCodeTag = txtICcodeTag.Text;
                    model.itemName = txtIName.Text;
                    model.description = txtDescription.Text;
                    model.brand = txtBrand.Text;
                    model.unit = txtUnit.Text;
                    model.supplierName = txtSupName.Text;
                    model.ssLevel = txtSSLevel.Text;
                    model.LTDelivery = txtLTDelivery.Text;
                    model.Inventory = yn;

                    int retVal = items.updateItems(model);

                    if (retVal == 1)
                    {
                        MessageBox.Show("DATA HAS BEEN SUCCESSFULLY UPDATED!", "CONFIRM!");
                        //this.Close();
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
        }

        private void items_frm_Load(object sender, EventArgs e)
        {
            displayData();
        }
        public void displayData()
        {
            DataSet ds = items.selectItems();
            if (ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (Stat != 1)
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedrow = dataGridView1.Rows[selectedindex];
                    txtICcodeTag.Text = Convert.ToString(selectedrow.Cells["ItemCodeTag"].Value);
                    txtDescription.Text = Convert.ToString(selectedrow.Cells["Description"].Value);
                    txtUnit.Text = Convert.ToString(selectedrow.Cells["Unit"].Value);
                    txtLTDelivery.Text = Convert.ToString(selectedrow.Cells["LTDelivery"].Value);
                    txtIName.Text = Convert.ToString(selectedrow.Cells["ItemName"].Value);
                    txtBrand.Text = Convert.ToString(selectedrow.Cells["Brand"].Value);
                    txtSupName.Text = Convert.ToString(selectedrow.Cells["SupplierName"].Value);
                    txtSSLevel.Text = Convert.ToString(selectedrow.Cells["SSLevel"].Value);
                    if (Convert.ToString(selectedrow.Cells["Inventory"].Value).Trim() == "Y")
                    {
                        rdBYes.Checked = true;
                    }
                    else if (Convert.ToString(selectedrow.Cells["Inventory"].Value).Trim() == "N")
                    {
                        rdNo.Checked = true;
                    }

                    ItemID = int.Parse(Convert.ToString(selectedrow.Cells["ID"].Value).Trim());

                    ItemCode = Convert.ToString(selectedrow.Cells["itemCode"].Value).Trim();
                    //textBox2.Text = Convert.ToString(selectedrow.Cells["ID"].Value);

                    button1.Text = "Update";
                } 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Text = "Save";
            txtICcodeTag.Text = "";
            txtBrand.Text = "";
            txtDescription.Text = "";
            txtIName.Text = "";
            txtLTDelivery.Text = "";
            txtSSLevel.Text = "";
            txtSupName.Text = "";
            txtUnit.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
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

                        Stat = 1;
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DtSet.Tables[0];
                        MyConnection.Close();
                        dtUpload = DtSet.Tables[0].Copy();
                        if (dtUpload.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dtUpload;
                            txtBoxUpload.Text = filePath;
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

                            Stat = 1;
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = DtSet.Tables[0];
                            MyConnection.Close();
                            dtUpload = DtSet.Tables[0].Copy();
                            if (dtUpload.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dtUpload;
                                txtBoxUpload.Text = filePath;
                                //txtBoxUpload.Text = dtUpload.Rows.Count.ToString();
                                //ShowData();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            MessageBox.Show("SOMETHING WENT WRONG!\n\nTRY TO CHECK THE SHEET NAME IT MUST BE 'Sheet1' or KINDLY CLOSE THE EXCEL FILE FIRST THEN TRY AGAIN.\n\n", "ERROR!");
                            Stat = 0;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("SOMETHING WENT WRONG!\n\nTRY TO CHECK THE SHEET NAME IT MUST BE 'Sheet1' or KINDLY CLOSE THE EXCEL FILE FIRST THEN TRY AGAIN.\n\n", "ERROR!");
                    Stat = 0;
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int retVal = 0;
                retVal = items.insertItem(ToXML.Toxml(dtUpload));

                if (retVal == 1)
                {
                    MessageBox.Show("DATA SUCCESSFULLY UPLOADED!", "CONFIRM!");
                    this.Close();
                    //ClearData();
                }
                else if (retVal == 2)
                {
                    MessageBox.Show("MULTIPLE DATA FOUND!", "ERROR!");
                    dataGridView1.DataSource = null;
                }
                else
                {
                    MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SOMETHING WENT WRONG! PLEASE CHECK YOUR DATA CORRECTLY!", "ERROR!");
            }
        }
    }
}
