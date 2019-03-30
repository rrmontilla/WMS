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
    public partial class Supplier_frm : Form
    {
        SupplierController supplier = new SupplierController();
        public int supplierID = 0;
        public string supplierCode = "";
        public Supplier_frm()
        {
            InitializeComponent();
        }

        OpenFileDialog file;
        static string filePath = string.Empty;
        DataTable dtUpload;
        int Stat = 0;
        private void btnSave_Click(object sender, EventArgs e)
        {
           
                DataTable newDt = new DataTable();
                newDt.Columns.Add("SUPPLIER CODE", typeof(string));
                newDt.Columns.Add("SUPPLIER NAME", typeof(string));
                newDt.Columns.Add("ADDRESS", typeof(string));
                newDt.Columns.Add("TIN #", typeof(string));
                newDt.Columns.Add("CELLPHONE NUMBER", typeof(string));
                newDt.Columns.Add("CONTACT #", typeof(string));
                newDt.Columns.Add("CONTACT PERSON", typeof(string));
                newDt.Columns.Add("PRODUCTS AVAILED", typeof(string));
                newDt.Columns.Add("PT", typeof(string));
                newDt.Columns.Add("COUNTRY", typeof(string));
                newDt.Columns.Add("CURRENCY", typeof(string));

                if (txtAddress.Text.Length == 0 || txtCellNum.Text.Length == 0 || txtCodeTag.Text.Length == 0
                    || txtConPerson.Text.Length == 0 || txtCountry.Text.Length == 0 || txtCurrency.Text.Length == 0
                    || txtProdAvailed.Text.Length == 0 || txtPT.Text.Length == 0 || txtSupplierName.Text.Length == 0
                    || txtTelNum.Text.Length == 0 || txtTIN.Text.Length == 0)
                {
                    MessageBox.Show("SOMETHING WENT WRONG! PLEASE FILL IN ALL THE DATA INFORMATION!", "ERROR!");
                }
                else
                {
                    if (btnSave.Text == "Save")
                    {
                        newDt.Rows.Add(txtCodeTag.Text, txtSupplierName.Text, txtAddress.Text, txtTIN.Text
                       , txtCellNum.Text, txtTelNum.Text, txtConPerson.Text, txtProdAvailed.Text
                       , txtPT.Text, txtCountry.Text, txtCurrency.Text);

                        int retVal = supplier.insertSupplier(ToXML.Toxml(newDt));

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
                    else if(btnSave.Text == "Update")
                    {
                            SupplierModel model = new SupplierModel();
                            model.ID = supplierID;
                            model.SupplierCode = supplierCode;
                            model.SupplierCodeTag = txtCodeTag.Text;
                            model.SupplierName = txtSupplierName.Text;
                            model.businessAddress = txtAddress.Text;
                            model.Tin = txtTIN.Text;
                            model.CellNumber = txtCellNum.Text;
                            model.TelNumber = txtTelNum.Text;
                            model.ContactPerson = txtConPerson.Text;
                            model.ProductAvailed = txtProdAvailed.Text;
                            model.PT = txtPT.Text;
                            model.Country = txtCountry.Text;
                            model.SupplierCurrency = txtCurrency.Text;

                            int retVal = supplier.updateSuplier(model);

                            if (retVal == 1)
                            {
                                MessageBox.Show("DATA HAS BEEN UPDATED SUCCESSFULLY!", "CONFIRM!");
                                this.Close();
                            }
                            else if (retVal == 2)
                            {
                                MessageBox.Show("MULTIPLE DATA FOUND!", "ERROR!");
                            }
                            else if (retVal == 3)
                            {
                                MessageBox.Show("INVALID ID", "ERROR!");
                            }
                            else
                            {
                                MessageBox.Show("SOMETHING WENT WRONG!", "ERROR!");
                            }
                        
                    }
                    displayData();
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int retVal = 0;
                retVal = supplier.insertSupplier(ToXML.Toxml(dtUpload));

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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (Stat != 1)
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedrow = dataGridView1.Rows[selectedindex];

                    supplierID = int.Parse(Convert.ToString(selectedrow.Cells["ID"].Value));
                    supplierCode = Convert.ToString(selectedrow.Cells["SupplierCode"].Value);
                    txtCodeTag.Text = Convert.ToString(selectedrow.Cells["SupplierCodeTag"].Value);
                    txtAddress.Text = Convert.ToString(selectedrow.Cells["BusinessAddress"].Value);
                    txtProdAvailed.Text = Convert.ToString(selectedrow.Cells["ProductsAvailed"].Value);
                    txtCountry.Text = Convert.ToString(selectedrow.Cells["Country"].Value);
                    txtSupplierName.Text = Convert.ToString(selectedrow.Cells["SupplierName"].Value);
                    txtTIN.Text = Convert.ToString(selectedrow.Cells["TIN"].Value);
                    txtCellNum.Text = Convert.ToString(selectedrow.Cells["CellNumber"].Value);
                    txtTelNum.Text = Convert.ToString(selectedrow.Cells["TelNumber"].Value);
                    txtConPerson.Text = Convert.ToString(selectedrow.Cells["ContactPerson"].Value);
                    txtPT.Text = Convert.ToString(selectedrow.Cells["PT"].Value);
                    txtCurrency.Text = Convert.ToString(selectedrow.Cells["SupplierCurrency"].Value);
                    //textBox1.Text = Convert.ToString(selectedrow.Cells["BranchName"].Value);
                    //textBox2.Text = Convert.ToString(selectedrow.Cells["ID"].Value);

                    btnSave.Text = "Update";
                } 
            }
        }

        private void Supplier_frm_Load(object sender, EventArgs e)
        {
            displayData();
        }
        public void displayData()
        {
            DataSet ds = supplier.SelectSupplier();
            if (ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           supplierID = 0;
           txtCodeTag.Text = "";
           txtAddress.Text = "";
           txtProdAvailed.Text = "";
           txtCountry.Text = "";
           txtSupplierName.Text = "";
           txtTIN.Text = "";
           txtCellNum.Text = "";
           txtTelNum.Text = "";
           txtConPerson.Text = "";
           txtPT.Text = "";
           txtCurrency.Text = "";
            btnSave.Text = "Save";
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {

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
    }
}
